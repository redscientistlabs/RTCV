using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace RTCV.NetCore
{

    public class UDPLink
    {
        private NetCoreSpec spec;
        private string IP { get { return spec.IP; } }
        private int PortServer{ get { return spec.Port; } }
        private int PortClient{get{return spec.Port + (spec.Loopback ? 1 : 0 );}} //If running on loopback, will use port+1 for client

        private Thread ReaderThread;
        private UdpClient Sender = null;
        private static volatile bool Running = false;

        internal UDPLink(NetCoreSpec _spec)
        {
            spec = _spec;

            int port = (spec.Side == NetworkSide.SERVER ? PortServer : PortClient);
            Sender = new UdpClient(IP, port);
            ConsoleEx.WriteLine($"UDP Client sending at {IP}:{port}");
            ReaderThread = new Thread(new ThreadStart(ListenToReader));
            ReaderThread.IsBackground = true;
            ReaderThread.Name = "UDP READER";
            ReaderThread.Start();
        }


        internal void Kill()
        {
            Running = false;

            try { ReaderThread.Abort(); } catch { }
            while (ReaderThread != null && ReaderThread.IsAlive) {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            } //Lets wait for the thread to die
            ReaderThread = null;

            try { Sender.Close(); } catch { }
        }

        internal void SendMessage(NetCoreSimpleMessage message)
        {
            if(Running)
            {
                Byte[] sdata = Encoding.ASCII.GetBytes(message.Type);
                Sender.Send(sdata, sdata.Length);
				//Todo - Refactor this into a way to blacklist specific commands 
				if(message.Type != "UI|KILLSWITCH_PULSE" || ConsoleEx.ShowDebug)
					ConsoleEx.WriteLine($"UDP : Sent simple message \"{message.Type}\"");
            }
        }

        private void ListenToReader()
        {
            int port = (spec.Side == NetworkSide.SERVER ? PortClient : PortServer);
            int UdpReceiveTimeout = Int32.MaxValue;

            UdpClient Listener = null;
            IPEndPoint groupEP = new IPEndPoint((IP == "127.0.0.1" ? IPAddress.Loopback : IPAddress.Any), port);

            try
            {
                Running = true;
                ConsoleEx.WriteLine($"UDP Server listening on Port {port}");

                while (Running)
                {

                    try
                    {
                        if (Listener == null)
                        {
                            Listener = new UdpClient(port);
                            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, UdpReceiveTimeout);
                        }
                    }
                    catch (SocketException ex)
                    {
                        if(ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        {
                            ConsoleEx.WriteLine("UDP Socket Port Collision");
                        }
                        else
                        {
                            ConsoleEx.WriteLine(ex.ToString());
                        }
                        
                        return;
                    }
                    catch (Exception ex)
                    {
                        ConsoleEx.WriteLine(ex.ToString());
                        return;
                    }

                    byte[] bytes = null;

                    try
                    {
						//if(Listener.Available > 0)
						bytes = Listener.Receive(ref groupEP);
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
                            throw ex;
                    }
					if(bytes != null)
						spec.Connector.hub.QueueMessage(new NetCoreSimpleMessage(Encoding.ASCII.GetString(bytes, 0, bytes.Length)));

                }

            }
            catch (ThreadAbortException)
            {
                ConsoleEx.WriteLine("Ongoing UDPLink Thread Killed");
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLine(e.ToString());
            }
            finally
            {
                Listener?.Client?.Close();
                Listener?.Close();
            }

        }
    }
}
