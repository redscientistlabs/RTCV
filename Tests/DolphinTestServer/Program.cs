using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV.NetCore;
using System.Threading;

namespace RTCV.DolphinTestServer
{
    class Program
    {
        static void Main(string[] args)
        {

            DolphinTestServer.StartServer();

            while (true)
            {


                /*
                
                //This is how you send a Simple message (Goes through UDP Link)
                TestClient.connector.SendMessage("Some command");

                //This is how you send an Advanced message with an object parameter (Goes through TCP Link)
                TestClient.connector.SendMessage("Some command", someObject);

                //This is how you send a synced Advanced Message, it will block the thread until it finishes on the other side.
                You can also use it to query a value. the object parameter is facultative
                var someValue = TestClient.connector.SendMessage("Some command", someObject);

                ---------------------------------------------------------
                Test console:

                SOMEMESSAGE     -> Goes through UDP Link
                #SOMEMESSAGE    -> Goes through TCP Link
                #!SOMEMESSAGE   -> Goes through TCP Link in Synced Mode
                .               -> Sends massive flood with an incrementing counter as the message

                */


                string message = Console.ReadLine();
                Random rnd = new Random();
                int iterations = 10000;

                byte[] bytes = new byte[] { 0, 0, 0, 0 };
                object[] values = new object[iterations];

                int counter = 0;
                while (message == ".")
                {
                    DolphinTestServer.connector.SendSyncedMessage((++counter).ToString(), null);
                    Thread.Sleep(DolphinTestServer.connector.spec.messageReadTimerDelay);
                }


                if (message.Length > 0 && message[0] == '#')
                {
                    //if (message.Length > 1 && message[1] == '!')
                    //    TestServer.connector.SendSyncedMessage(message);
                    //else
                    //   TestServer.connector.SendMessage(message, new object());

                    if (message.Length > 1 && message[1] == '1')
                        for (int i = 0; i < iterations; i++)
                        {
                            ConsoleEx.WriteLine(PeekByte(i).ToString());
                        }

                    else if (message.Length > 1 && message[1] == '2')
                        for (int i = 0; i < iterations; i++)
                        {
                            PokeByte(i, 1);
                            Thread.Sleep(DolphinTestServer.connector.spec.messageReadTimerDelay);
                        }


                    else if (message.Length > 1 && message[1] == '3')
                    {
                        for (int i = 0; i < iterations; i++)
                        {
                            byte[] _bytes = new byte[4];
                            values[i] = PeekBytes(i, 4);
                        }
                        for (int i = 0; i < iterations; i++)
                        {
                            byte[] _bytes = new byte[4];
                            _bytes = (byte[])values[i];
                            ConsoleEx.WriteLine(_bytes[0].ToString() + " " + _bytes[1].ToString() + " " + _bytes[2].ToString() + " " + _bytes[3].ToString());
                        }

                    }


                    else if (message.Length > 1 && message[1] == '4')
                    {
                        bytes[0] = 1;
                        bytes[1] = 1;
                        bytes[2] = 1;
                        bytes[3] = 1;
                        for (int i = 0; i < iterations; i++)
                        {
                            PokeBytes(i, bytes);
                            Thread.Sleep(DolphinTestServer.connector.spec.messageReadTimerDelay);
                        }
                    }

                    else
                        DolphinTestServer.connector.SendMessage(message, new object());

                }
                else
                    DolphinTestServer.connector.SendMessage(message);
                

            }
        }

        public static Byte PeekByte(long address)
        {
            return Convert.ToByte(DolphinTestServer.connector.SendSyncedMessage("PEEKBYTE", ((Object)address)?.ToString() ?? "NULL"));
        }

        public static void PokeByte(long address, byte value)
        {
            Object[] message = new Object[2];
            message[0] = address;
            message[1] = value;

            DolphinTestServer.connector.SendSyncedMessage("POKEBYTE", message);
        }

        public static Byte[] PeekBytes(long address, int range)
        {
            Object[] message = new Object[2];
            message[0] = address;
            message[1] = range;
            return (Byte[])(DolphinTestServer.connector.SendSyncedMessage("PEEKBYTES", message));
        }

        public static void PokeBytes(long address, byte[] value)
        {
            Object[] message = new Object[3];
            message[0] = address;
            message[1] = value.Length;
            message[2] = value;

            DolphinTestServer.connector.SendMessage("POKEBYTES", message);
        }
    }




    public static class DolphinTestServer
    {
        public static NetCoreConnector connector = null;

        public static void StartServer()
        {
            var spec = new NetCoreSpec();
            spec.Side = NetworkSide.SERVER;
            spec.MessageReceived += OnMessageReceived;
            connector = new NetCoreConnector(spec);
        }

        public static void RestartServer()
        {
            connector.Kill();
            connector = null;
            StartServer();
        }

        private static void OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            // This is where you implement interaction
            // Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

            var message = e.message;
            var simpleMessage = message as NetCoreSimpleMessage;
            var advancedMessage = message as NetCoreAdvancedMessage;

            switch (message.Type) //Handle received messages here
            {
                default:
                    ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
                    break;
            }

        }

    }
}
