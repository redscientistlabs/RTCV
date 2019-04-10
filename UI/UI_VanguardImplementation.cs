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


					case REMOTE_HOTKEY_AUTOCORRUPTTOGGLE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_Core_Form>().btnAutoCorrupt_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_ERRORDELAYDECREASE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
						//	if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value > 1)
						//		S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value--;
						});
						break;

					case REMOTE_HOTKEY_ERRORDELAYINCREASE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
						//	if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value < S.GET<RTC_GeneralParameters_Form>().track_ErrorDelay.Maximum)
						//		S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value++;
						});
						break;

					case REMOTE_HOTKEY_INTENSITYDECREASE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
						//	if (S.GET<RTC_GeneralParameters_Form>().multiTB.Value > 1)
						//		S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value--;
						});
						break;

					case REMOTE_HOTKEY_INTENSITYINCREASE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
						//	if (S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value < S.GET<RTC_GeneralParameters_Form>().track_Intensity.Maximum)
						//		S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value++;
						});
						break;

					case REMOTE_HOTKEY_GHLOADCORRUPT:
						{
							SyncObjectSingleton.FormExecute((o, ea) =>
							{
								S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = true;
								S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);
							});
						}
						break;

					case REMOTE_HOTKEY_GHCORRUPT:
						RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);

						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							bool isload = S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked;
							S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = false;
							S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);
							S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = isload;
						});

						break;

					case REMOTE_HOTKEY_GHREROLL:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().btnRerollSelected_Click(null, null);
						});

						break;

					case REMOTE_HOTKEY_GHLOAD:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "LOAD";
							S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_GHSAVE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "SAVE";
							S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_GHSTASHTOSTOCKPILE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().AddStashToStockpile(false);
						});
						break;

					case REMOTE_HOTKEY_SENDRAWSTASH:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
						});
						break;

					case REMOTE_HOTKEY_BLASTRAWSTASH:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);
							LocalNetCoreRouter.Route(CORRUPTCORE, ASYNCBLAST, null, true);

							S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_BLASTLAYERTOGGLE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().btnBlastToggle_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_BLASTLAYERREBLAST:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							if (StockpileManager_UISide.CurrentStashkey == null || StockpileManager_UISide.CurrentStashkey.BlastLayer.Layer.Count == 0)
							{
								S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = false;
								return;
							}
							S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = true;
							StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey, false);
						});
						break;

					case REMOTE_HOTKEY_GAMEPROTECTIONBACK:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var f = S.GET<RTC_Core_Form>();
							var b = f.btnGpJumpBack;
							if (b.Visible && b.Enabled)
								f.btnGpJumpBack_Click(null, null);
						});
						break;

					case REMOTE_HOTKEY_GAMEPROTECTIONNOW:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var f = S.GET<RTC_Core_Form>();
							var b = f.btnGpJumpNow;
							if (b.Visible && b.Enabled)
								f.btnGpJumpNow_Click(null, null);
						});
						break;
					case REMOTE_HOTKEY_BEDISABLE50:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnDisable50_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BEINVERTDISABLED:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnInvertDisabled_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BEREMOVEDISABLED:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnRemoveDisabled_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BESHIFTUP:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnShiftBlastLayerUp_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BESHIFTDOWN:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnShiftBlastLayerDown_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BELOADCORRUPT:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && bef.Focused)
							{
								bef.btnLoadCorrupt_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BEAPPLY:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && bef.Focused)
							{
								bef.btnCorrupt_Click(null, null);
							}
						});
						break;
					case REMOTE_HOTKEY_BESENDSTASH:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							var bef = S.GET<RTC_NewBlastEditor_Form>();
							if (bef != null && Form.ActiveForm == bef)
							{
								bef.btnSendToStash_Click(null, null);
							}
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
