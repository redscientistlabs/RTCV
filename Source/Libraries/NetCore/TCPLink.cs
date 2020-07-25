namespace RTCV.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Threading;
    using Ceras;
    using RTCV.NetCore.NetCore_Extensions;

    public class TCPLink : IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly NetCoreSpec spec;
        private readonly TCPLinkWatch linkWatch = null;

        private volatile NetworkStatus _status = NetworkStatus.DISCONNECTED;

        internal NetworkStatus status
        {
            get => _status;
            set
            {
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_NETWORKSTATUS}", value.ToString()));

                if (value != _status)
                {
                    logger.Debug("TCPLink status {STATUS}", value);
                }

                _status = value;
            }
        }

        private string IP => spec.IP;
        private int Port => spec.Port;

        internal volatile TcpClient client;
        private volatile NetworkStream clientStream;

        private readonly object PeerMessageQueueLock = new object();
        private readonly object serializationLock = new object();
        private readonly LinkedList<NetCoreAdvancedMessage> PeerMessageQueue = new LinkedList<NetCoreAdvancedMessage>();

        private volatile Thread streamReadingThread = null;
        private System.Timers.Timer BoopMonitoringTimer = null;

        private bool supposedToBeConnected = false;
        private bool expectingSomeone = false;

        private readonly int DefaultBoopMonitoringCounter;
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
            {
                StartNetworking();
            }
        }

        private Socket KillableAcceptSocket(TcpListener listener)
        {
            Socket socket = null;

            try
            {
                var clientConnected = new ManualResetEvent(false);
                listener.Start();
                clientConnected.Reset();
                var iasyncResult = listener.BeginAcceptSocket((ar) =>
                {
                    try
                    {
                        socket = listener?.EndAcceptSocket(ar);
                        clientConnected?.Set();
                    }
                    catch (Exception ex) { DiscardException(ex); }
                }, null);
                clientConnected.WaitOne();

                return socket;
            }
            catch
            {
                throw;
            }
            finally
            {
                listener?.Stop();
            }
        }

        private void StoreMessages(NetworkStream providedStream)
        {
            var serializer = CreateSerializer();
            TcpListener server = null;
            Socket socket = null;
            NetworkStream networkStream = providedStream;

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

                networkStream.ReadTimeout = 10000;
                networkStream.WriteTimeout = int.MaxValue;  //Using {BOOP} commands routed through UDP/TCP

                if (spec.Side == NetworkSide.CLIENT)
                {
                    SendMessage(new NetCoreAdvancedMessage("{HI}"));    //The exchange of {HI} command confirms that link is established on Receiving
                }

                while (true)
                {
                    if (networkStream?.DataAvailable == true)
                    {
                        if (spec.Side == NetworkSide.SERVER && (!socket?.Connected ?? true))
                        {
                            return;
                        }

                        NetCoreAdvancedMessage message = null;
                        try
                        {
                            message = ReadMessageFromNetworkStream(ref serializer, ref networkStream);
                        }
                        catch { throw; }

                        if (message != null)
                        {
                            if (message.Type == "{RETURNVALUE}")
                            {
                                spec.Connector.watch.AddReturn(message);
                            }
                            else
                            {
                                spec.Connector.hub.QueueMessage(message);
                            }
                        }
                    }

                    while (PeerMessageQueue.Count > 0)
                    {
                        var stopThread = ProcessPeerMessage(ref serializer, ref networkStream);
                        if (stopThread)
                            return;
                    }

                    Thread.Sleep(spec.messageReadTimerDelay);
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    logger.Warn("Ongoing TCPLink Thread Killed");
                }
                else if (ex.InnerException != null && ex.InnerException is SocketException)
                {
                    logger.Warn(ex, "Ongoing TCPLink Socket Closed during use");
                    logger.Debug(ex.StackTrace);
                }
                else if (ex is SerializationException)
                {
                    logger.Warn(ex, "Ongoing TCPLink Closed during Serialization operation");
                    logger.Debug(ex.StackTrace);
                }
                else if (ex is ObjectDisposedException)
                {
                    logger.Warn(ex, "Ongoing TCPLink Closed during Socket acceptance");
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
                    networkStream?.Close();
                    networkStream?.Dispose();
                }
                catch { } //nobody cares why this failed

                try
                {
                    socket?.Shutdown(SocketShutdown.Both);
                    socket?.Dispose();
                }
                catch { } //nobody cares why this failed

                try
                {
                    server?.Stop();
                }
                catch (Exception ex)
                {
                    DiscardException(ex);
                }

                if (status == NetworkStatus.CONNECTED)
                {
                    status = (expectingSomeone ? NetworkStatus.CONNECTIONLOST : NetworkStatus.DISCONNECTED);
                }
                else if (status != NetworkStatus.CONNECTIONLOST)
                {
                    status = NetworkStatus.DISCONNECTED;
                }

                //Kill synced query if happenning
                spec.Connector.watch.Kill();
            }
        }

        private CerasSerializer CreateSerializer()
        {
            var config = new SerializerConfig();
            config.Advanced.PersistTypeCache = true;
            config.Advanced.UseReinterpretFormatter = false;
            config.OnResolveFormatter.Add((c, t) =>
            {
                if (t == typeof(HashSet<byte[]>))
                {
                    return new NetCore.NetCore_Extensions.HashSetFormatterThatKeepsItsComparer();
                }
                else if (t == typeof(HashSet<byte?[]>))
                {
                    return new NetCore.NetCore_Extensions.NullableByteHashSetFormatterThatKeepsItsComparer();
                }

                return null; // continue searching
            });
            return new CerasSerializer(config);
        }

        private NetCoreAdvancedMessage ReadMessageFromNetworkStream(ref CerasSerializer serializer, ref NetworkStream networkStream)
        {
            NetCoreAdvancedMessage message = null;
            using (var ms = new MemoryStream())
            {
                var sw = new Stopwatch();
                sw.Start();

                //Read the size
                var buffer = new byte[4];
                networkStream.Read(buffer, 0, buffer.Length);
                var lengthToReceive = BitConverter.ToInt32(buffer, 0);

                //Console.WriteLine("I want this many bytes: " + lengthToReceive);
                //Now read until we have that many bytes
                var bytesRead = CopyBytes(lengthToReceive, networkStream, ms);
                //Console.WriteLine("I got this many bytes: " + bytesRead);

                //Deserialize it
                ms.Position = 0;

                //cmd = (RTC_Command)binaryFormatter.Deserialize(ms);
                var temp = ms.ToArray();
                lock (serializationLock)
                {
                    message = serializer.Deserialize<NetCoreAdvancedMessage>(temp);
                }

                sw.Stop();
                if (message.Type != "{BOOP}" && sw.ElapsedMilliseconds > 50)
                {
                    logger.Info($"It took {sw.ElapsedMilliseconds} ms to deserialize cmd {message.Type} of {temp.Length} bytes");
                }
            }

            return message;
        }

        //returns true if the thread should be stopped
        private bool ProcessPeerMessage(ref CerasSerializer serializer, ref NetworkStream networkStream)
        {
            NetCoreMessage pendingMessage;

            lock (PeerMessageQueueLock)
            {
                pendingMessage = PeerMessageQueue.First.Value;
                PeerMessageQueue.RemoveFirst();
            }

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                //Write the length of the command to the first four bytes
                byte[] buf;
                lock (serializationLock)
                {
                    if (pendingMessage is NetCoreAdvancedMessage am && am.objectValue != null)
                    {
                        lock (am.objectValue)
                        {
                            buf = serializer.Serialize(pendingMessage);
                        }
                    }
                    else
                    {
                        buf = serializer.Serialize(pendingMessage);
                    }
                }
                //Write the length of the incoming object to the NetworkStream
                var length = BitConverter.GetBytes(buf.Length);
                networkStream.Write(length, 0, length.Length);

                networkStream.Write(buf, 0, buf.Length);
                sw.Stop();
                if (pendingMessage.Type != "{BOOP}" && sw.ElapsedMilliseconds > 50)
                {
                    logger.Info($"It took {sw.ElapsedMilliseconds} ms to serialize backCmd {pendingMessage.Type} of {buf.Length} bytes");
                }
            }
            catch
            {
                throw;
            }

            if (pendingMessage.Type == "{BYE}")
            {
                lock (PeerMessageQueueLock) //Since we're shutting down, let's clear the message queue
                {
                    PeerMessageQueue?.Clear();
                }
            }

            if (status == NetworkStatus.DISCONNECTED || status == NetworkStatus.CONNECTIONLOST)
            {
                //If the link's status changed from an outside factor, we want to stop the thread.

                lock (PeerMessageQueueLock)
                {
                    PeerMessageQueue?.Clear();
                }

                return true;
            }

            return false;
        }

        private void DiscardException(Exception ex) =>
            //Discarded exception but write it in console
            logger.Warn(ex, "DiscardException: {spec.Side}:{status} -> Supposed to be connected -> {supposedToBeConnected} expectingsomeone -> {expectingSomeone} status -> {status}\n{stacktrace}", spec.Side, status, supposedToBeConnected, expectingSomeone, status, ex.StackTrace);

        internal void Kill()
        {
            linkWatch?.Kill();
            StopNetworking(false);
        }

        public void Dispose()
        {
            BoopMonitoringTimer.Dispose();
            client.Dispose();
            clientStream.Dispose();
            linkWatch.Dispose();
            spec.Dispose();
        }

        private void KillConnections(TcpClient clientRef)
        {
            try
            {
                streamReadingThread?.Abort();
                while (streamReadingThread?.IsAlive == true)
                {
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
                if (object.ReferenceEquals(clientRef, client))
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
            {
                expectingSomeone = false;
            }

            if (spec.Side == NetworkSide.CLIENT)
            {
                if (stayConnected)
                {
                    logger.Warn($"TCP Client Connection Lost");
                    //spec.OnClientConnectionLost(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTIONLOST}"));
                }
                else
                {
                    logger.Warn("TCP Client Disconnected");
                    //spec.OnClientDisconnected(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTDISCONNECTED}"));
                }
            }
            else if (spec.Side == NetworkSide.SERVER)
            {
                if (stayConnected)
                {
                    logger.Warn("TCP Server Connection Lost");
                    //spec.OnServerConnectionLost(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERCONNECTIONLOST}"));
                }
                else
                {
                    logger.Warn("TCP Server Disconnected");
                    //spec.OnServerDisconnected(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERDISCONNECTED}"));
                }
            }

            status = (stayConnected ? NetworkStatus.CONNECTIONLOST : NetworkStatus.DISCONNECTED);

            lock (PeerMessageQueueLock)
            {
                PeerMessageQueue.Clear();
            }

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
                var success = false;

                try
                {
                    result = client.BeginConnect(IP, Port, null, null);
                    success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(200)); //This will block the thread for 200ms
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "TCPClient connection failed");
                }

                if (!success || result == null || client == null)
                {
                    throw new Exception("Failed to connect");
                }

                client.EndConnect(result);
                clientStream = client.GetStream();

                if (streamReadingThread != null)
                {
                    streamReadingThread.Abort();
                    while (streamReadingThread?.IsAlive == true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(10);
                    } //Lets wait for the thread to die
                }

                streamReadingThread = new Thread(() => StoreMessages(clientStream))
                {
                    Name = "TCP CLIENT",
                    IsBackground = true
                };
                streamReadingThread.Start();
                logger.Debug("Started new TCPLink Thread for CLIENT");
            }
            catch (Exception ex)
            {
                if (ex.Message != "Failed to connect")
                {
                    DiscardException(ex);
                }

                try
                {
                    clientStream?.Close();
                    clientStream = null;
                }
                catch { }

                try
                {
                    if (object.ReferenceEquals(clientRef, client))
                    {
                        client?.Close();
                        client = null;
                    }
                }
                catch { }

                status = NetworkStatus.DISCONNECTED;
                logger.Debug($"Connecting Failed");
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
                    while (streamReadingThread?.IsAlive == true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(10);
                    } //Lets wait for the thread to die
                }

                streamReadingThread = new Thread(() => StoreMessages(null))
                {
                    Name = "TCP SERVER",
                    IsBackground = true
                };
                streamReadingThread.Start();
                logger.Debug("Started new TCPLink Thread for SERVER");
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
            {
                return;
            }

            if (spec.Side == NetworkSide.CLIENT)
            {
                status = NetworkStatus.CONNECTING;
                logger.Debug($"TCP Client connecting to {IP}:{Port}");
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_CLIENTCONNECTING}"));
                //spec.OnClientConnecting(null);

                if (!StartClient())
                {
                    return;
                }
            }
            else //(spec.Side == NetworkSide.SERVER)
            {
                expectingSomeone = false;
                status = NetworkStatus.LISTENING;
                logger.Debug("TCP Server listening on Port {Port}", Port);
                //spec.OnServerListening(null);
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERLISTENING}"));

                if (!StartServer())
                {
                    return;
                }
            }

            supposedToBeConnected = true;

            BoopMonitoringCounter = DefaultBoopMonitoringCounter;

            BoopMonitoringTimer?.Stop();
            BoopMonitoringTimer = new System.Timers.Timer
            {
                Interval = 500
            };
            BoopMonitoringTimer.Elapsed += BoopMonitoringTimer_Tick;
            BoopMonitoringTimer.Start();
        }

        private void BoopMonitoringTimer_Tick(object sender, EventArgs e)
        {
            if (!supposedToBeConnected || status != NetworkStatus.CONNECTED)
            {
                return;
            }

            BoopMonitoringCounter--;

            if (spec.Connector.watch.IsWaitingForReturn)
            {
                spec.Connector.SendMessage("{BOOP}");   //If waiting for the return of a synced message or sending flood is happenning, send {BOOP} messages through UDP
            }
            else
            {
                SendMessage(new NetCoreAdvancedMessage("{BOOP}"), true);
            }

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
            {
                message = (_message as NetCoreAdvancedMessage);
            }
            else
            {
                message = new NetCoreAdvancedMessage(_message.Type); // promote message to Advanced if simple ({BOOP} command goes through UDP Link)
            }

            if ((!message.Type.StartsWith("{EVENT_") && message.Type != "{BOOP}") || ConsoleEx.ShowDebug)
            {
                logger.Info("{side} Process advanced message -> {message}", spec.Side, message.Type);
            }

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
                        logger.Debug("TCP Server Connected");
                        //spec.OnServerConnected(null);
                        spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SERVERCONNECTED}"));

                        SendMessage(new NetCoreAdvancedMessage("{HI}"));
                    }
                    else //(side == NetworkSide.CLIENT)
                    {
                        //Client always sends the first {HI} but will wait for the Server to reply with one
                        logger.Debug($"TCP Client Connected to {IP}:{Port}");
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
            if (message.Type == "{HI}") // {HI} is the only command that can go blindly through the pipe before the link is established
            {
                return true;
            }
            else if (status != NetworkStatus.CONNECTED)
            {
                logger.Warn("{side} -> Can't send message \"{message}\", link is not established", spec.Side, message.Type);
                return false;
            }

            return true;
        }

        internal void SendMessage(NetCoreAdvancedMessage message, bool priority = false)
        {
            if (!IsSendingAdvancedMessageAuthorized(message))
            {
                return;
            }

            lock (PeerMessageQueueLock)
            {
                if (priority) //If there is a stream of data occuring, priority ensures that a message will skip the line and gets sent ASAP
                {
                    PeerMessageQueue.AddFirst(message);
                }
                else
                {
                    PeerMessageQueue.AddLast(message);
                }
            }
            if (ConsoleEx.ShowDebug)
            {
                logger.Trace("side} -> Sent advanced message \"{Type}\", priority:{Priority}", spec.Side, message.Type, priority);
            }
        }

        internal object SendSyncedMessage(NetCoreAdvancedMessage message, bool priority = false)
        {
            //A synced will block the sender's thread until a response is received

            if (!IsSendingAdvancedMessageAuthorized(message))
            {
                return null;
            }

            message.requestGuid = Guid.NewGuid();

            lock (PeerMessageQueueLock)
            {
                if (priority)
                {
                    PeerMessageQueue.AddFirst(message);
                }
                else
                {
                    PeerMessageQueue.AddLast(message);
                }
            }

            logger.Info("{side} -> Sent advanced message {Type}, priority:{Priority}", spec.Side, message.Type, priority);

            return spec.Connector.watch.GetValue((Guid)message.requestGuid, message.Type); //This will lock here until value is returned from peer
        }

        #region STREAM EXTENSIONS
        //Thanks! https://stackoverflow.com/a/13021983
        public static long CopyBytes(long bytesRequired, Stream inStream, Stream outStream)
        {
            var readSoFar = 0L;
            var buffer = new byte[64 * 1024];
            do
            {
                var toRead = Math.Min(bytesRequired - readSoFar, buffer.Length);
                var readNow = inStream.Read(buffer, 0, (int)toRead);
                if (readNow == 0)
                {
                    break; // End of stream
                }

                outStream.Write(buffer, 0, readNow);
                readSoFar += readNow;
            } while (readSoFar < bytesRequired);
            return readSoFar;
        }
        #endregion

    }
}
