using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkSide = RTCV.NetCore.NetworkSide;

namespace RTCV.Vanguard
{
    public class VanguardConnector : IRoutable
    {
        public NetCoreReceiver receiver;

        public NetCoreConnector netConn;

        public VanguardConnector(NetCoreReceiver _receiver)
        {
            receiver = _receiver;

			LocalNetCoreRouter.registerEndpoint(this, "VANGUARD");

			var netCoreSpec = new NetCoreSpec();
            netCoreSpec.Side = NetworkSide.CLIENT;
            netCoreSpec.Port = 42169;
            netCoreSpec.MessageReceived += OnMessageReceivedProxy;;
		
            netConn = LocalNetCoreRouter.registerEndpoint(new NetCoreConnector(netCoreSpec), "WGH");
            LocalNetCoreRouter.registerEndpoint(netConn, "DEFAULT"); //Will send mesages to netcore if can't find the destination

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

                return NetCore.LocalNetCoreRouter.Route(endpoint, e);
            }
            else
            {   //This is for the Vanguard Implementation
                receiver.OnMessageReceived(e);
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

		public void KillNetcore()
		{
			netConn.Kill();
		}
	}
}
