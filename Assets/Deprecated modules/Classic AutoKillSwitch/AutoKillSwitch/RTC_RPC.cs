using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RTC
{


    public static class RTC_RPC
    {
        private static string ip = "127.0.0.1";
        private static int listenPort = 56667;
        private static Thread t;
        public static volatile bool Heartbeat = false;
        public static volatile bool Freeze = false;
        private static UdpClient publisher = new UdpClient(ip, listenPort);

        public static void Start()
        {
            t = new Thread(new ThreadStart(Listen));
            t.IsBackground = true;
            t.Start();
        }

        private static void Listen()
        {
            bool done = false;

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ip), listenPort);

            try
            {
                while (!done)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    string[] splits = msg.Split('|');

                    switch (splits[0])
                    {
                        default:
                            break;

                        case "RTC_Heartbeat":

                            switch (splits[1])
                            {
                                default:
                                    break;

                                case "TICK":
                                    Heartbeat = true;
                                    break;
                                case "FREEZE":
                                    Heartbeat = true;
                                    Freeze = true;
                                    break;
                                case "UNFREEZE":
                                    Heartbeat = true;
                                    Freeze = false;
                                    break;
                                case "CLOSE":
                                    Heartbeat = true;
                                    Application.Exit();
                                    break;

                            }
                            break;
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }

        public static void Send(string msg)
        {
            Byte[] sdata = Encoding.ASCII.GetBytes(msg);
            publisher.Send(sdata, sdata.Length);
        }
    }
}
