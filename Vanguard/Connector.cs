using RTCV.NetCore;
using RTCV.CorruptCore;
using RTCV.DolphinCorrupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.Vanguard
{
    public class VanguardConnector : IRoutable
    {
        TargetSpec spec;

        CorruptCoreConnector corruptConn;
        DolphinCorruptConnector dolphinConn;
        NetCoreConnector netConn;

        public VanguardConnector(TargetSpec _spec)
        {
            spec = _spec;

            LocalNetCoreRouter.registerEndpoint(this, "VANGUARD");
            corruptConn = LocalNetCoreRouter.registerEndpoint(new CorruptCoreConnector(spec.specDetails), "CORRUPTCORE");
            dolphinConn = LocalNetCoreRouter.registerEndpoint(new DolphinCorruptConnector(), "DOLPHIN");


            var netCoreSpec = new NetCoreSpec();
            netCoreSpec.Side = NetworkSide.CLIENT;
            netCoreSpec.MessageReceived += OnMessageReceivedProxy;

            netConn = LocalNetCoreRouter.registerEndpoint(new NetCoreConnector(netCoreSpec), "RTCV");
            //LocalNetCoreRouter.registerEndpoint(netConn, "WGH"); //We can make an alias for WGH


            //Once everything is registered, we can trigger the first update
            //The first update is most likely going to be just the template
            spec.specDetails.OnSpecUpdated(null);


        }

        public void OnMessageReceivedProxy(object sender, NetCoreEventArgs e) => OnMessageReceived(sender, e);
        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            //No implementation here, we simply route and return

            if (e.message.Type.Contains('|'))
            {   //This needs to be routed

                var msgParts = e.message.Type.Split('|');
                string endpoint = msgParts[0];
                e.message.Type = msgParts[1]; //remove endpoint from type

                return NetCore.LocalNetCoreRouter.Route(endpoint, sender, e);
            }
            else
            {   //This is for the Vanguard Implementation
                spec.OnMessageReceived(e);
                return e.returnMessage;
            }

            
        }

        //Ship everything to netcore, any needed routing will be handled in there
        public void SendMessage(string message) => netConn.SendMessage(message);
        public void SendMessage(string message, object value) => netConn.SendMessage(message,value);
        public object SendSyncedMessage(string message) { return netConn.SendSyncedMessage(message); }
        public object SendSyncedMessage(string message, object value) { return netConn.SendSyncedMessage(message, value); }

        public void Kill()
        {

        }
    }
}
