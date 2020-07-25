namespace RTCV.Vanguard
{
    using System;
    using System.Linq;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using NetworkSide = RTCV.NetCore.NetworkSide;

    public class VanguardConnector : IRoutable, IDisposable
    {
        public NetCoreReceiver receiver;

        public NetCoreConnector netConn;
        public CorruptCoreConnector corruptConn;

        public NetworkStatus netcoreStatus => netConn.status;

        public VanguardConnector(NetCoreReceiver _receiver)
        {
            receiver = _receiver;

            LocalNetCoreRouter.registerEndpoint(this, NetcoreCommands.VANGUARD);
            corruptConn = new CorruptCoreConnector();
            LocalNetCoreRouter.registerEndpoint(corruptConn, NetcoreCommands.CORRUPTCORE);

            if (receiver.Attached)//attached mode
            {
                CorruptCore.RtcCore.Attached = true;
                RTCV.UI.UICore.Start(null);
                return;
            }

            var netCoreSpec = new NetCoreSpec
            {
                Side = NetworkSide.CLIENT
            };
            netCoreSpec.MessageReceived += OnMessageReceivedProxy;
            netCoreSpec.ClientConnected += NetCoreSpec_ClientConnected;
            netConn = new NetCoreConnector(netCoreSpec);

            //netConn = LocalNetCoreRouter.registerEndpoint(new NetCoreConnector(netCoreSpec), "WGH");
            LocalNetCoreRouter.registerEndpoint(netConn, NetcoreCommands.DEFAULT); //Will send mesages to netcore if can't find the destination
        }

        public static void ImplyClientConnected() => NetCoreSpec_ClientConnected(null, null);

        private static void NetCoreSpec_ClientConnected(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHVANGUARDSPEC, RTCV.NetCore.AllSpec.VanguardSpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_ALLSPECSSENT, true);
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
        public void SendMessage(string message, object value) => netConn.SendMessage(message, value);
        public object SendSyncedMessage(string message) => netConn.SendSyncedMessage(message);
        public object SendSyncedMessage(string message, object value) => netConn.SendSyncedMessage(message, value);

        public void Kill()
        {
        }

        public void Dispose()
        {
            netConn?.Dispose();
        }

        public static void PushVanguardSpecRef(FullSpec spec) => RTCV.NetCore.AllSpec.VanguardSpec = spec;

        public static bool IsUIForm() => (bool?)RTCV.NetCore.AllSpec.UISpec?[NetcoreCommands.RTC_INFOCUS] ?? false;

        public void KillNetcore() => netConn.Kill();
    }
}
