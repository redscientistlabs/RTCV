using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.NetCore.NetcoreCommands;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public static class UI_VanguardImplementation
	{
		public static UIConnector connector = null;

		public static void StartServer()
		{
			ConsoleEx.WriteLine("Starting UI Vanguard Implementation");

			var spec = new NetCoreReceiver();
			spec.MessageReceived += OnMessageReceived;

			spec.Attached = CorruptCore.CorruptCore.Attached;

			connector = new UIConnector(spec);
		}

		public static void RestartServer()
		{
			connector.Kill();
			connector = null;
			StartServer();
		}

		private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			try
			{
				var message = e.message;
				var simpleMessage = message as NetCoreSimpleMessage;
				var advancedMessage = message as NetCoreAdvancedMessage;

				switch (message.Type) //Handle received messages here
				{
					case REMOTE_PUSHVANGUARDSPEC:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							if (!CorruptCore.CorruptCore.Attached)
								RTCV.NetCore.AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !CorruptCore.CorruptCore.Attached);

							e.setReturnValue(true);

						//Push the UI and CorruptCore spec (since we're master)
						LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPEC, RTCV.NetCore.AllSpec.UISpec.GetPartialSpec(), true);
							LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPEC, RTCV.NetCore.AllSpec.CorruptCoreSpec.GetPartialSpec(), true);

						//Specs are all set up so UI is clear.
						LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ALLSPECSSENT, true);
						});
						break;


					case REMOTE_ALLSPECSSENT:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_Core_Form>().Show();
							if (UICore.FirstConnect)
							{
								UICore.FirstConnect = false;
								S.GET<RTC_Core_Form>().btnEngineConfig_Click(null, null);
							}
							else
							{
							//Push the VMDs since we store them out of spec
							var vmdProtos = MemoryDomains.VmdPool.Values.Cast<VirtualMemoryDomain>().Select(x => x.Proto).ToArray();
								LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHVMDPROTOS, vmdProtos, true);

							//Configure the UI based on the vanguard spec
							UICore.ConfigureUIFromVanguardSpec();

							//Return to the main form. If the form is null for some reason, default to engineconfig
							if(S.GET<RTC_Core_Form>().previousForm == null)
								S.GET<RTC_Core_Form>().previousForm = S.GET<RTC_EngineConfig_Form>();
							S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);

							//Unhide the GH
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Size = S.GET<RTC_GlitchHarvester_Form>().Size;
								S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Hide();
							}

							S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = 0;

							if (!CorruptCore.CorruptCore.Attached)
								AutoKillSwitch.Enabled = true;
						});
						break;

					case REMOTE_PUSHVANGUARDSPECUPDATE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							RTCV.NetCore.AllSpec.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
						});
						e.setReturnValue(true);
						break;

					//CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
					case REMOTE_PUSHCORRUPTCORESPECUPDATE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							RTCV.NetCore.AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
						});
						e.setReturnValue(true);
						break;

					case REMOTE_EVENT_DOMAINSUPDATED:

						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
							S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] as string[] ?? new string[]{});
						});
						break;

					case REMOTE_GETBLASTGENERATOR_LAYER:

						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							e.setReturnValue(S.GET<RTC_BlastGenerator_Form>().GenerateBlastLayers(true));
						});
						break;
					case ERROR_DISABLE_AUTOCORRUPT:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_Core_Form>().AutoCorrupt = false;
						});
						break;



					case REMOTE_BACKUPKEY_STASH:
						StockpileManager_UISide.BackupedState = (StashKey)advancedMessage.objectValue;
						StockpileManager_UISide.AllBackupStates.Push((StashKey)advancedMessage.objectValue);
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_Core_Form>().btnGpJumpBack.Visible = true;
							S.GET<RTC_Core_Form>().btnGpJumpNow.Visible = true;
						});
						break;

					case KILLSWITCH_PULSE:
						AutoKillSwitch.Pulse();
						break;

				}
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return;
			}
		}
	}
}
