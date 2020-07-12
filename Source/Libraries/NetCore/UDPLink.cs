namespace RTCV.NetCore
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    public class UDPLink
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private NetCoreSpec spec;
        private string IP => spec.IP;
        private int PortServer => spec.Port;
        private int PortClient => spec.Port + (spec.Loopback ? 1 : 0);  //If running on loopback, will use port+1 for client

        private Thread ReaderThread;
        private UdpClient Sender = null;
        private static volatile bool Running = false;

        internal UDPLink(NetCoreSpec _spec)
        {
            spec = _spec;

            int port = (spec.Side == NetworkSide.SERVER ? PortServer : PortClient);
            Sender = new UdpClient(IP, port);
            logger.Info("UDP Client sending at {IP}:{port}", IP, port);
            ReaderThread = new Thread(new ThreadStart(ListenToReader))
            {
                IsBackground = true,
                Name = "UDP READER"
            };
            ReaderThread.Start();
        }

        internal void Kill()
        {
            Running = false;

            try { ReaderThread.Abort(); } catch { }
            while (ReaderThread != null && ReaderThread.IsAlive)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            } //Lets wait for the thread to die
            ReaderThread = null;

            try { Sender.Close(); } catch { }
        }

        internal void SendMessage(NetCoreSimpleMessage message)
        {
            if (Running)
            {
                byte[] sdata = Encoding.ASCII.GetBytes(message.Type);
                Sender.Send(sdata, sdata.Length);
                //Todo - Refactor this into a way to blacklist specific commands
                if (message.Type != "UI|KILLSWITCH_PULSE" || ConsoleEx.ShowDebug)
                {
                    logger.Trace("UDP : Sent simple message \"{type}\"", message.Type);
                }
            }
        }

        private void ListenToReader()
        {
            int port = (spec.Side == NetworkSide.SERVER ? PortClient : PortServer);
            int UdpReceiveTimeout = int.MaxValue;

            UdpClient Listener = null;
            IPEndPoint groupEP = new IPEndPoint((IP == "127.0.0.1" ? IPAddress.Loopback : IPAddress.Any), port);

            try
            {
                Running = true;
                logger.Info("UDP Server listening on Port {port}", port);

                while (Running)
                {
                    try
                    {
                        if (Listener == null)
                        {
                            Listener = new UdpClient(groupEP);
                            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, UdpReceiveTimeout);
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        {
                            logger.Error("UDP Socket Port Collision");
                        }
                        else
                        {
                            logger.Error(ex, "Error when opening socket.");
                        }

                        return;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Unknown error.");
                        return;
                    }

                    byte[] bytes = null;

                    try
                    {
                        if (Listener.Available > 0)
                        {
                            bytes = Listener.Receive(ref groupEP);
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            Listener?.Client?.Close();
                            Listener?.Close();
                            Listener = null;
                            continue;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    if (bytes != null)
                    {
                        spec.Connector.hub.QueueMessage(new NetCoreSimpleMessage(Encoding.ASCII.GetString(bytes, 0, bytes.Length)));
                    }

                    //Sleep if there's no more data
                    if (Listener.Available == 0)
                    {
                        Thread.Sleep(spec.messageReadTimerDelay);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                logger.Debug("Ongoing UDPLink Thread Killed");
            }
            catch (Exception e)
            {
                logger.Debug(e, "Exception in ListenToReader");
            }
            finally
            {
                Listener?.Client?.Close();
                Listener?.Close();
            }
        }
    }
}
