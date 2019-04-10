using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RTCV.TestVanguardClient
{
    class Program
    {
        static void Main(string[] args)
        {

            TestClient.StartClient();

            while (true)
            {

                /*
                
                This implements NetCore through Vanguard. Pretty much the same code is here except that it goes through Vanguard
                When Vanguard is initialized, it sets up an additionnal Routing for messages inside the same process.

                Routing works by adding a routing command, a pipe and then the original command
                ROUTING|COMMAND

                As the command is processed, the routing command is removed and the message is received on the other side.

                Local communication across projects within the same process should go through the Route function directly
                var someVar = NetCore.LocalNetCoreRouter.Route("ENDPOINT", this, NetCoreEventArgs)

                */


                string message = Console.ReadLine();

                

                if (message.Length > 0 && message[0] == '#')
                {
                    if (message.Length > 1 && message[1] == '!')
                        TestClient.connector.SendSyncedMessage(message.Substring(2));
                    else
                        TestClient.connector.SendMessage(message.Substring(1), new object());
                }
                else
                    TestClient.connector.SendMessage(message);


            }
        }
    }




    public static class TestClient
    {
        public static Vanguard.VanguardConnector connector = null;

        public static void StartClient()
        {
            Thread.Sleep(500); //When starting in Multiple Startup Project, the first try will be uncessful since
                               //the server takes a bit more time to start then the client.

            var spec = new Vanguard.TargetSpec();
            spec.MessageReceived += OnMessageReceived;

            var partial = new NetCore.PartialSpec("VanguardSpec");
            partial["SOME_VARIABLE"] = 420;
            partial["NOT_SATAN"] = 666;

            spec.specDetails = new NetCore.FullSpec(partial);

            connector = new Vanguard.VanguardConnector(spec);
        }

        public static void RestartClient()
        {
            connector.Kill();
            connector = null;
            StartClient();
        }

        private static void OnMessageReceived(object sender, NetCore.NetCoreEventArgs e)
        {
            // This is where you implement interaction.
            // Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

            var message = e.message;
            var simpleMessage = message as NetCore.NetCoreSimpleMessage;
            var advancedMessage = message as NetCore.NetCoreAdvancedMessage;

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
                    NetCore.ConsoleEx.WriteLine("Simulating 20 sec of workload");
                    Thread.Sleep(20000);
                    break;

                case "#!HANG":
                    NetCore.ConsoleEx.WriteLine("Hanging forever");
                    Thread.Sleep(int.MaxValue);
                    break;

                default:
                    NetCore.ConsoleEx.WriteLine($"Received unassigned {(message is NetCore.NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
                    break;
            }

        }

    }
}
