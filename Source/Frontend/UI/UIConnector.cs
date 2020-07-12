namespace RTCV.UI
{
    using System;
    using System.Linq;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public class UIConnector : IRoutable
    {
        private NetCoreReceiver receiver;
        public NetCoreConnector netConn;

        public UIConnector(NetCoreReceiver _receiver)
        {
            receiver = _receiver;

            LocalNetCoreRouter.registerEndpoint(this, NetcoreCommands.UI);

            if (receiver.Attached)
            {
                return;
            }

            var netCoreSpec = new NetCore.NetCoreSpec
            {
                Side = NetCore.NetworkSide.SERVER,
                Attached = receiver.Attached,
                Loopback = true
            };
            netCoreSpec.MessageReceived += OnMessageReceivedProxy;
            netCoreSpec.ServerConnected += Spec_ServerConnected;
            netCoreSpec.ServerConnectionLost += NetCoreSpec_ServerConnectionLost;
            netCoreSpec.ServerDisconnected += NetCoreSpec_ServerConnectionLost;

            netConn = new NetCoreConnector(netCoreSpec);
            LocalNetCoreRouter.registerEndpoint(netConn, NetcoreCommands.VANGUARD);
            LocalNetCoreRouter.registerEndpoint(netConn, NetcoreCommands.DEFAULT); //Will send mesages to netcore if can't find the destination
        }

        private void NetCoreSpec_ServerConnectionLost(object sender, EventArgs e)
        {
            if (UICore.isClosing || UICore.FirstConnect)
            {
                return;
            }

            SyncObjectSingleton.FormExecute(() =>
            {
                if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>()
                        .IsDisposed)
                {
                    S.GET<RTC_ConnectionStatus_Form>()
                            .lbConnectionStatus.Text =
                        $"{(string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Vanguard"} connection timed out";

                    UICore.LockInterface();
                    UI_DefaultGrids.connectionStatus.LoadToMain();
                }

                S.GET<RTC_VmdAct_Form>()
                    .cbAutoAddDump.Checked = false;
                GameProtection.WasAutoCorruptRunning = CorruptCore.RtcCore.AutoCorrupt;
                S.GET<UI_CoreForm>().AutoCorrupt = false;
            });
            GameProtection.Stop(false);

            if (S.GET<UI_CoreForm>()
                    .cbUseAutoKillSwitch.Checked && AllSpec.VanguardSpec != null)
            {
                AutoKillSwitch.KillEmulator();
            }
        }

        private static void Spec_ServerConnected(object sender, EventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text =
                    $"Connected to {(string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Vanguard"}";
            });
        }

        public void OnMessageReceivedProxy(object sender, NetCoreEventArgs e)
        {
            OnMessageReceived(sender, e);
        }

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
        public void SendMessage(string message)
        {
            netConn.SendMessage(message);
        }

        public void SendMessage(string message, object value)
        {
            netConn.SendMessage(message, value);
        }

        public object SendSyncedMessage(string message) { return netConn.SendSyncedMessage(message); }
        public object SendSyncedMessage(string message, object value) { return netConn.SendSyncedMessage(message, value); }

        public void Kill()
        {
            netConn.Kill();
        }
        public void Restart()
        {
            netConn.Restart();
        }
    }
}
