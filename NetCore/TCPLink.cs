using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using Ceras;
using Ceras.Helpers;

namespace RTCV.NetCore
{
    public class TCPLink
    {
        private NetCoreSpec spec;
        private TCPLinkWatch linkWatch = null;

        private volatile NetworkStatus _status = NetworkStatus.DISCONNECTED;
        internal NetworkStatus status
        {
            get
            {
                return _status;
            }
            set
            {

                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_NETWORKSTATUS}", value.ToString()));

                if (value != _status)
                    ConsoleEx.WriteLine($"TCPLink status {value}");

                _status = value;
            }
        }

        private string IP { get { return spec.IP; } }
        private int Port { get { return spec.Port; } }

        internal volatile TcpClient client;
        private volatile NetworkStream clientStream;

        private object PeerMessageQueueLock = new object();
        private LinkedList<NetCoreAdvancedMessage> PeerMessageQueue = new LinkedList<NetCoreAdvancedMessage>();

        private volatile Thread streamReadingThread = null;
        private System.Timers.Timer BoopMonitoringTimer = null;

        private bool supposedToBeConnected = false;
        private bool expectingSomeone = false;

        private int DefaultBoopMonitoringCounter;
        private int BoopMonitoringCounter;

        internal TCPLink(NetCoreSpec _spec)
        {
            spec = _spec;

            DefaultBoopMonitoringCounter = spec.DefaultBoopMonitoringCounter;
            BoopMonitoringCounter = spec.DefaultBoopMonitoringCounter;

            if (spec.AutoReconnect)
            {
                linkWatch = new TCPLinkWatch(this, spec);
            }
            else
                StartNetworking();
        }


        private Socket KillableAcceptSocket(TcpListener listener)
        {
            Socket socket = null;

            try
            {
                ManualResetEvent clientConnected = new ManualResetEvent(false);
                listener.Start();
                clientConnected.Reset();
                var iasyncResult = listener.BeginAcceptSocket((ar) =>
                {
                    try
                    {   
                        socket = listener.EndAcceptSocket(ar);
                        clientConnected.Set();
                    }
                    catch (ObjectDisposedException) { }
                    catch (Exception ex) { DiscardException(ex); }

                }, null);
                clientConnected.WaitOne();

                return socket;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                listener.Stop();
            }
        }

        private void StoreMessages(NetworkStream providedStream)
		{
			var config = new SerializerConfig();
			config.PersistTypeCache = true;
			var serializer = new CerasSerializer(config);

            TcpListener server = null;
            Socket socket = null;
            NetworkStream networkStream = null;

            if (providedStream != null)
                networkStream = providedStream;

            try
            {

                if (networkStream == null)
                {
                    server = new TcpListener((IP == "127.0.0.1" ? IPAddress.Loopback : IPAddress.Any), Port);
                    server.Start();
                    socket = KillableAcceptSocket(server);
                    networkStream = new NetworkStream(socket);

                    server.Stop();

                }

                networkStream.ReadTimeout = int.MaxValue;   //We don't put a timeout because we can detect broken links
                networkStream.WriteTimeout = int.MaxValue;  //Using {BOOP} commands routed through UDP/TCP

                if (spec.Side == NetworkSide.CLIENT)
                    SendMessage(new NetCoreAdvancedMessage("{HI}"));    //The exchange of {HI} command confirms that link is established on Receiving

                while (true)
                {

                    if (networkStream != null && networkStream.DataAvailable)
                    {
                        NetCoreAdvancedMessage message = null;

                        try
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                Stopwatch sw = new Stopwatch();
                                sw.Start();

                                //Read the size
                                int lengthToReceive = 0;
                                byte[] _lengthToReceive = new byte[4];
                                networkStream.Read(_lengthToReceive, 0, _lengthToReceive.Length);
                                lengthToReceive = BitConverter.ToInt32(_lengthToReceive, 0);

                                //Console.WriteLine("I want this many bytes: " + lengthToReceive);
                                //Now read until we have that many bytes
                                long bytesRead = CopyBytes(lengthToReceive, networkStream, ms);
                                //Console.WriteLine("I got this many bytes: " + bytesRead);

                                //Deserialize it
                                ms.Position = 0;

                                //cmd = (RTC_Command)binaryFormatter.Deserialize(ms);
                                var temp = ms.ToArray();
                                message = serializer.Deserialize<NetCoreAdvancedMessage>(temp);

                                sw.Stop();
								if(message.Type != "{BOOP}" && sw.ElapsedMilliseconds > 50)
									Console.WriteLine("It took " + sw.ElapsedMilliseconds + " ms to deserialize cmd " + message.Type + " of " + temp.Length + " bytes");
                            }
                        }
                        catch { throw; }

                        if (message != null)
                        {
                            if (message.Type == "{RETURNVALUE}")
                                spec.Connector.watch.AddReturn(message);
                            else
                                spec.Connector.hub.QueueMessage(message);
                        }
                    }

                    while (PeerMessageQueue.Count > 0)
                    {
                        NetCoreMessage pendingMessage;

                        lock (PeerMessageQueueLock)
                        {
                            pendingMessage = PeerMessageQueue.First.Value;
                            PeerMessageQueue.RemoveFirst();
                        }

                        try
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            //Write the length of the command to the first four bytes
                            byte[] buf = serializer.Serialize(pendingMessage);

                            //Write the length of the incoming object to the NetworkStream
                            byte[] length = BitConverter.GetBytes(buf.Length);
                            networkStream.Write(length, 0, length.Length);

                            networkStream.Write(buf, 0, buf.Length);
                            sw.Stop();
							if (pendingMessage.Type != "{BOOP}" && sw.ElapsedMilliseconds > 50)
								Console.WriteLine("It took " + sw.ElapsedMilliseconds + " ms to serialize backCmd " + pendingMessage.Type + " of " + buf.Length + " bytes");
                        }
                        catch
                        {
                            throw;
                        }

                        if (pendingMessage.Type == "{BYE}")
                        {
                            lock (PeerMessageQueueLock) //Since we're shutting down, let's clear the message queue
                                PeerMessageQueue.Clear();
                        }

                        if (status == NetworkStatus.DISCONNECTED || status == NetworkStatus.CONNECTIONLOST)
                        {
                            //If the link's status changed from an outside factor, we want to stop the thread.

                            lock (PeerMessageQueueLock)
                                PeerMessageQueue.Clear();

                            return;
                        }
                    }

                    Thread.Sleep(spec.messageReadTimerDelay);
                }

            }
            catch (Exception ex)
            {


                if (ex is ThreadAbortException)
                {
                    ConsoleEx.WriteLine("Ongoing TCPLink Thread Killed");
                }
                else if (ex.InnerException != null && ex.InnerException is SocketException)
                {
                    ConsoleEx.WriteLine("Ongoing TCPLink Socket Closed during use");
                }
                else if (ex is SerializationException)
                {
                    ConsoleEx.WriteLine("Ongoing TCPLink Closed during Serialization operation");
                }
                else if (ex is ObjectDisposedException)
                {
                    ConsoleEx.WriteLine("Ongoing TCPLink Closed during Socket acceptance");
                }
                else
                {
                    DiscardException(ex);
                }

            }
            finally
            {

                //Let's force close everything JUST IN CASE

                try
                {
                    networkStream.Close();
                    networkStream.Dispose();
                }
                catch { } //nobody cares why this failed


                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Dispose();
                }
                catch { } //nobody cares why this failed


                try
                {
                    server.Stop();
                }
                catch { } //nobody cares why this failed

                if (status == NetworkStatus.CONNECTED)
                    status = (expectingSomeone ? NetworkStatus.CONNECTIONLOST : NetworkStatus.DISCONNECTED);
                else if (status != NetworkStatus.CONNECTIONLOST)
                    status = NetworkStatus.DISCONNECTED;

                //Kill synced query if happenning
                spec.Connector.watch.Kill();
            }

        }

        private void DiscardException(Exception ex)
        {
            //Discarded exception but write it in console
            ConsoleEx.WriteLine($"{spec.Side}:{status} -> {ex}");
        }

        internal void Kill()
        {
            linkWatch?.Kill();
            StopNetworking(false);
        }

        private void KillConnections(TcpClient clientRef)
        {


            try
            {
                streamReadingThread?.Abort();
                while (streamReadingThread != null && streamReadingThread.IsAlive) {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(10);
                } //Lets wait for the thread to die
                streamReadingThread = null;
            }
            catch { }

            try
            {
                clientStream?.Close();
                clientStream = null;
            }
            catch { }

            try
            {
                if (Object.ReferenceEquals(clientRef, client))
                {
                    client?.Close();
                    client = null;
                }
            }
            catch { }

        }

        internal void StopNetworking(bool stopGracefully = true, bool stayConnected = false)
        {
            TcpClient clientRef = client;

            if (stopGracefully)
            {   //If this is a graceful stop, try to say {BYE} before leaving
                lock (PeerMessageQueueLock)
                {
                    PeerMessageQueue.Clear();
                    spec.Connector.hub.SendMessage(new NetCoreAdvancedMessage("{BYE}"));
                    //When the {BYE} command gets sent, a {SAYBYE} command will be queued in own side's Message Hub.
                    //Both sides will then call StopNetworking with the stopGracefully flag set to false.
                }

                return;
            }

            BoopMonitoringTimer?.Stop();
            BoopMonitoringTimer = null;

            supposedToBeConnected = false;

            if (!stayConnected)
                expectingSomeone = false;

            if (spec.Side == NetworkSide.CLIENT )
            {
                if (stayConnected)
                {
                    ConsoleEx.WriteLine($"TCP Client Connection Lost");
                    //spec.OnClientConnectionLost(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTIONLOST}"));
                }
                else
                {
                    ConsoleEx.WriteLine($"TCP Client Disconnected");
                    //spec.OnClientDisconnected(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTDISCONNECTED}"));
                }
            }
            else if (spec.Side == NetworkSide.SERVER)
            {
                if (stayConnected)
                {
                    ConsoleEx.WriteLine($"TCP Server Connection Lost");
                    //spec.OnServerConnectionLost(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERCONNECTIONLOST}"));
                }
                else
                {
                    ConsoleEx.WriteLine($"TCP Server Disconnected");
                    //spec.OnServerDisconnected(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERDISCONNECTED}"));
                }
            }

            status = (stayConnected ? NetworkStatus.CONNECTIONLOST : NetworkStatus.DISCONNECTED);

            lock (PeerMessageQueueLock)
                PeerMessageQueue.Clear();

            
            KillConnections(clientRef);

            spec.Connector.watch.Kill(); //Kills the ReturnWatch if waiting for a value

        }

        private bool StartClient()
        {
            client = new TcpClient();
            TcpClient clientRef = client;

            try
            {
                IAsyncResult result = null;
                bool success = false;

                try
                {
                    result = client.BeginConnect(IP, Port, null, null);
                    success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(200)); //This will block the thread for 200ms
                }
                catch { }

                if (!success || result == null || client == null)
                    throw new Exception("Failed to connect");

                client.EndConnect(result);
                clientStream = client.GetStream();

                if (streamReadingThread != null)
                {
                    streamReadingThread.Abort();
                    while (streamReadingThread != null && streamReadingThread.IsAlive)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(10);
                    } //Lets wait for the thread to die
                }

                streamReadingThread = new Thread(() => StoreMessages(clientStream));
                streamReadingThread.Name = "TCP CLIENT";
                streamReadingThread.IsBackground = true;
                streamReadingThread.Start();
                ConsoleEx.WriteLine($"Started new TCPLink Thread for CLIENT");

            }
            catch (Exception ex)
            {

                if (ex.Message != "Failed to connect")
                    DiscardException(ex);

                try
                {
                    clientStream?.Close();
                    clientStream = null;
                } catch { }

                try
                {
                    if (Object.ReferenceEquals(clientRef, client))
                    {
                        client?.Close();
                        client = null;
                    }
                    
                }
                catch { }

                status = NetworkStatus.DISCONNECTED;
                ConsoleEx.WriteLine($"Connecting Failed");
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTINGFAILED}"));
                //spec.OnClientConnectingFailed(null);

                return false;
            }

            return true;
        }

        private bool StartServer()
        {
            try
            {
                if (streamReadingThread != null)
                {
                    streamReadingThread.Abort();
                    while (streamReadingThread != null && streamReadingThread.IsAlive)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(10);
                    } //Lets wait for the thread to die
                }

                streamReadingThread = new Thread(() => StoreMessages(null));
                streamReadingThread.Name = "TCP SERVER";
                streamReadingThread.IsBackground = true;
                streamReadingThread.Start();
                ConsoleEx.WriteLine($"Started new TCPLink Thread for SERVER");
            }
            catch (Exception ex)
            {
                DiscardException(ex);
                return false;
            }

            return true;
        }

        internal void StartNetworking()
        {

            if (supposedToBeConnected && (status == NetworkStatus.CONNECTED || status == NetworkStatus.CONNECTING || status == NetworkStatus.LISTENING))
                return;


            if (spec.Side == NetworkSide.CLIENT)
            {
                status = NetworkStatus.CONNECTING;
                ConsoleEx.WriteLine($"TCP Client connecting to {IP}:{Port}");
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTING}"));
                //spec.OnClientConnecting(null);

                if (!StartClient())
                    return;

            }
            else //(spec.Side == NetworkSide.SERVER)
            {
                expectingSomeone = false;
                status = NetworkStatus.LISTENING;
                ConsoleEx.WriteLine($"TCP Server listening on Port {Port}");
                //spec.OnServerListening(null);
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERLISTENING}"));

                if (!StartServer())
                    return;

            }

            supposedToBeConnected = true;


            BoopMonitoringCounter = DefaultBoopMonitoringCounter;

            BoopMonitoringTimer?.Stop();
            BoopMonitoringTimer = new System.Timers.Timer();
            BoopMonitoringTimer.Interval = 500;
            BoopMonitoringTimer.Elapsed += BoopMonitoringTimer_Tick;
            BoopMonitoringTimer.Start();

        }

        private void BoopMonitoringTimer_Tick(object sender, EventArgs e)
        {
            if (!supposedToBeConnected || status != NetworkStatus.CONNECTED)
                return;

            BoopMonitoringCounter--;

            if (spec.Connector.watch.IsWaitingForReturn)
                spec.Connector.SendMessage("{BOOP}");   //If waiting for the return of a synced message or sending flood is happenning, send {BOOP} messages through UDP
            else
                SendMessage(new NetCoreAdvancedMessage("{BOOP}"), true);


            if (BoopMonitoringCounter == 0 && status == NetworkStatus.CONNECTED)//If no boops have been received in some time, shutdown netcore's TCP
            {
                //Shutdown TCP Link
                StopNetworking(false, expectingSomeone);
            }

        }

        internal bool ProcessAdvancedMessage(NetCoreMessage _message)
        {
            NetCoreAdvancedMessage message;

            if (_message is NetCoreAdvancedMessage)
                message = (_message as NetCoreAdvancedMessage);
            else
                message = new NetCoreAdvancedMessage(_message.Type); // promote message to Advanced if simple ({BOOP} command goes through UDP Link)

            if(!message.Type.StartsWith("{EVENT_") || ConsoleEx.ShowDebug)
                ConsoleEx.WriteLine(spec.Side.ToString() + ":Process advanced message -> " + message.Type.ToString());

            switch (message.Type)
            {
                // NetCore Internal Commands
                // Some of these commands go through the UDP Link but are upgraded to an Advanced Message
                // As they are parsed by the Message Hub

                case "{HI}": //Greetings to confirm that communication is established

                    expectingSomeone = true;
                    status = NetworkStatus.CONNECTED;

                    if (spec.Side == NetworkSide.SERVER)
                    {
                        //Server receives {HI} after client has established connection
                        ConsoleEx.WriteLine($"TCP Server Connected");
                        //spec.OnServerConnected(null);
                        spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERCONNECTED}"));

                        SendMessage(new NetCoreAdvancedMessage("{HI}"));
                    }
                    else //(side == NetworkSide.CLIENT)
                    {
                        //Client always sends the first {HI} but will wait for the Server to reply with one
                        ConsoleEx.WriteLine($"TCP Client Connected to {IP}:{Port}");
                        //spec.OnClientConnected(null);
                        spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTED}"));
                    }

                    

                    break;

                case "{BYE}": // End of disconnect
                    Kill();
                    break;

                case "{BOOP}":  // Ping system to confirm is TCP link is still in order
                                // Boops are redirected through the UDP pipe so if the TCP Link is busy transfering a lot of data
                                // or is jammed waiting for a return, these will still go through
                    BoopMonitoringCounter = DefaultBoopMonitoringCounter;
                    break;

                
                //THREADSAFE EVENT FIRING
                case "{EVENT_CLIENTCONNECTING}":
                    spec.OnClientConnecting(null);
                    break;

                case "{EVENT_CLIENTCONNECTINGFAILED}":
                    spec.OnClientConnectingFailed(null);
                    break;

                case "{EVENT_CLIENTCONNECTED}":
                    spec.OnClientConnected(null);
                    break;

                case "{EVENT_CLIENTDISCONNECTED}":
                    spec.OnClientDisconnected(null);
                    break;

                case "{EVENT_CLIENTCONNECTIONLOST}":
                    spec.OnClientConnectionLost(null);
                    break;

                case "{EVENT_SERVERLISTENING}":
                    spec.OnServerListening(null);
                    break;

                case "{EVENT_SERVERCONNECTED}":
                    spec.OnServerConnected(null);
                    break;

                case "{EVENT_SERVERDISCONNECTED}":
                    spec.OnServerDisconnected(null);
                    break;

                case "{EVENT_SERVERCONNECTIONLOST}":
                    spec.OnServerConnectionLost(null);
                    break;

                case "{EVENT_SYNCEDMESSAGESTART}":
                    spec.OnSyncedMessageStart(null);
                    break;

                case "{EVENT_SYNCEDMESSAGEEND}":
                    spec.OnSyncedMessageEnd(null);
                    break;


                default:
                    //If message wasn't procesed, just return false
                    //Message may be handled on an upper level
                    return false;
            }


            return true;
        }

        internal bool IsSendingAdvancedMessageAuthorized(NetCoreAdvancedMessage message)
        {

            if(message.Type == "{HI}") // {HI} is the only command that can go blindly through the pipe before the link is established
            {
                return true;
            }
            else if (status != NetworkStatus.CONNECTED) 
            {
                ConsoleEx.WriteLine($"{spec.Side.ToString()} -> Can't send message \"{message.Type.ToString()}\", link is not established");
                return false;
            }

            return true;
        }

        internal void SendMessage(NetCoreAdvancedMessage message, bool priority = false)
        {

            if (!IsSendingAdvancedMessageAuthorized(message))
                return;

            lock (PeerMessageQueueLock)
            {
                if (priority) //If there is a stream of data occuring, priority ensures that a message will skip the line and gets sent ASAP
                    PeerMessageQueue.AddFirst(message);
                else
                    PeerMessageQueue.AddLast(message);
            }

            ConsoleEx.WriteLine($"{spec.Side.ToString()} -> Sent advanced message \"{message.Type.ToString()}\", priority:{priority.ToString()}");

        }

        internal object SendSyncedMessage(NetCoreAdvancedMessage message, bool priority = false)
        {
            //A synced will block the sender's thread until a response is received

            if (!IsSendingAdvancedMessageAuthorized(message))
                return null;

            message.requestGuid = Guid.NewGuid();

            lock (PeerMessageQueueLock)
            {
                if (priority)
                    PeerMessageQueue.AddFirst(message);
                else
                    PeerMessageQueue.AddLast(message);
            }

            ConsoleEx.WriteLine($"{spec.Side.ToString()}:Sent synced advanced message \"{message.Type.ToString()}\", priority:{priority.ToString()}");

            return spec.Connector.watch.GetValue((Guid)message.requestGuid, message.Type); //This will lock here until value is returned from peer
        }

        #region STREAM EXTENSIONS
        //Thanks! https://stackoverflow.com/a/13021983
        public static long CopyBytes(long bytesRequired, Stream inStream, Stream outStream)
        {
            long readSoFar = 0L;
            var buffer = new byte[64 * 1024];
            do
            {
                var toRead = Math.Min(bytesRequired - readSoFar, buffer.Length);
                var readNow = inStream.Read(buffer, 0, (int)toRead);
                if (readNow == 0)
                    break; // End of stream
                outStream.Write(buffer, 0, readNow);
                readSoFar += readNow;
            } while (readSoFar < bytesRequired);
            return readSoFar;
        }
        #endregion

    }
}
