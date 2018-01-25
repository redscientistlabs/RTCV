using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV.NetCore;
using System.Threading;

namespace RTCV.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {

            TestServer.StartServer();

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


                
                int counter = 0;
                while (message == ".")
                {
                    TestServer.connector.SendSyncedMessage((++counter).ToString(), null);
                    Thread.Sleep(TestServer.connector.spec.messageReadTimerDelay);
                }
                

                
                if (message.Length > 0 && message[0] == '#')
                {
                    if (message.Length > 1 && message[1] == '!')
                        TestServer.connector.SendSyncedMessage(message.Substring(2));
                    else
                        TestServer.connector.SendMessage(message.Substring(1), new object());
                }
                else
                    TestServer.connector.SendMessage(message);
                

            }
        }
    }




    public static class TestServer
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

                case "JUST A MESSAGE":
                    //do something
                    break;

                case "ADVANCED MESSAGE THAT CONTAINS A VALUE":
                    object value = advancedMessage.objectValue; //This is how you get the value from a message
                    break;

                case "#!RETURNTEST": //ADVANCED MESSAGE (SYNCED) WANTS A RETURN VALUE
                    e.setReturnValue(new Random(666));
                    break;

                case "#!WAIT":
                    ConsoleEx.WriteLine("Simulating 20 sec of workload");
                    Thread.Sleep(20000);
                    break;

                case "#!HANG":
                    ConsoleEx.WriteLine("Hanging forever");
                    Thread.Sleep(int.MaxValue);
                    break;

                default:
                    ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
                    break;
            }

        }

    }
}
