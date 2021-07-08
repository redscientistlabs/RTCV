namespace RTCV.Vanguard
{
    using System;
    using System.Linq;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.NetCore.Enums;

    public class VanguardConnector : IRoutable, IDisposable
    {
        private NetCoreReceiver _receiver;

        public NetCoreConnector netConn { get; private set; }
        private CorruptCoreConnector corruptConn;

        public NetworkStatus netcoreStatus => netConn.status;

        public VanguardConnector(NetCoreReceiver receiver)
        {
            RtcCore.PluginHost.LoadPluginAssemblies("../RTCV/RTC/PLUGINS", "./PLUGINS");

            _receiver = receiver;

            LocalNetCoreRouter.registerEndpoint(this, NetCore.Endpoints.Vanguard);
            corruptConn = new CorruptCoreConnector();
            LocalNetCoreRouter.registerEndpoint(corruptConn, NetCore.Endpoints.CorruptCore);

            if (_receiver.Attached)//attached mode
            {
                RtcCore.Attached = true;
                UI.UICore.Start(null);
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
            LocalNetCoreRouter.registerEndpoint(netConn, NetCore.Endpoints.Default); //Will send mesages to netcore if can't find the destination
        }

        public static void ImplyClientConnected() => NetCoreSpec_ClientConnected(null, null);

        private static void NetCoreSpec_ClientConnected(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.UI, NetCore.Commands.Remote.PushVanguardSpec, AllSpec.VanguardSpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(NetCore.Endpoints.UI, NetCore.Commands.Remote.AllSpecSent, true);
        }

        public void OnMessageReceivedProxy(object sender, NetCoreEventArgs e) => OnMessageReceived(sender, e);
        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            //No implementation here, we simply route and return

            if (e.message.Type.Contains('|'))
            {   //This needs to be routed
                var msgParts = e.message.Type.Split('|');
                string endpoint = msgParts[0];
                e.message.Type = msgParts[1]; //remove endpoint from type

                return LocalNetCoreRouter.Route(endpoint, e);
            }
            else
            {   //This is for the Vanguard Implementation
                _receiver.OnMessageReceived(e);
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
            netConn?.Kill();
        }

        public void Dispose()
        {
            netConn?.Dispose();
        }

        public static void PushVanguardSpecRef(FullSpec spec) => AllSpec.VanguardSpec = spec;

        public static bool IsUIForm() => (bool?)AllSpec.UISpec?[NetCore.Commands.Basic.RTCInFocus] ?? false;

        public void KillNetcore() => netConn.Kill();
    }
}
