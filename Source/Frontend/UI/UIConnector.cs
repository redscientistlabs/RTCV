using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.UI.Modular;

namespace RTCV.UI
{
	public class UIConnector : IRoutable
	{
		NetCoreReceiver receiver;
		public NetCoreConnector netConn;

		public UIConnector(NetCoreReceiver _receiver)
		{
			receiver = _receiver;

			LocalNetCoreRouter.registerEndpoint(this, NetcoreCommands.UI);

			if (receiver.Attached)
				return;

			var netCoreSpec = new NetCore.NetCoreSpec();
			netCoreSpec.Side = NetCore.NetworkSide.SERVER;
			netCoreSpec.Attached = receiver.Attached;
			netCoreSpec.Loopback = true;
			netCoreSpec.MessageReceived += OnMessageReceivedProxy;
			netCoreSpec.ServerConnected += Spec_ServerConnected;
			netCoreSpec.ServerConnectionLost += NetCoreSpec_ServerConnectionLost;
			netCoreSpec.ServerDisconnected += NetCoreSpec_ServerConnectionLost;

			netConn = new NetCoreConnector(netCoreSpec);
			LocalNetCoreRouter.registerEndpoint(netConn, "VANGUARD");
			LocalNetCoreRouter.registerEndpoint(netConn, "DEFAULT"); //Will send mesages to netcore if can't find the destination
		}

		private void NetCoreSpec_ServerConnectionLost(object sender, EventArgs e)
		{
			if(UICore.isClosing)
				return;

			SyncObjectSingleton.FormExecute((o, ea) =>
			{
				if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>().IsDisposed)
				{
					S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text = "Connection status: Emulator timed out";
                    UI_DefaultGrids.connectionStatus.LoadToMain();
				}

				if (S.GET<RTC_GlitchHarvester_Form>() != null && !S.GET<RTC_GlitchHarvester_Form>().IsDisposed)
				{
					S.GET<RTC_GlitchHarvester_Form>().lbConnectionStatus.Text = "Connection status: Emulator timed out";
					S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.BringToFront();
					S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Show();
				}

				S.GET<RTC_VmdAct_Form>().cbAutoAddDump.Checked = false;
                GameProtection.WasAutoCorruptRunning = CorruptCore.CorruptCore.AutoCorrupt;
                S.GET<UI_CoreForm>().AutoCorrupt = false;
			});
			GameProtection.Stop();

			if(S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked)
				AutoKillSwitch.KillEmulator("KILL + RESTART");
		}

		private static void Spec_ServerConnected(object sender, EventArgs e)
		{
			SyncObjectSingleton.FormExecute((o, ea) =>
			{
				S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text = "Connection status: Connected to Emulator";
			});
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
		public object SendSyncedMessage(string message) { return netConn.SendSyncedMessage(message); }
		public object SendSyncedMessage(string message, object value) { return netConn.SendSyncedMessage(message, value); }

		public void Kill()
		{
			netConn.Kill();
		}
	}
}
