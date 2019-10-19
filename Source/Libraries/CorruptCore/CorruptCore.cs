using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using Newtonsoft.Json;
using RTCV.NetCore;
using Timer = System.Windows.Forms.Timer;

namespace RTCV.CorruptCore
{
	public static class RtcCore
	{
        //General RTC Values
        public const string RtcVersion = "5.0.3-b9";

        private static volatile int seed = DateTime.Now.Millisecond;
        public static int Seed => ++seed;

        [ThreadStatic]
        private static Random _RND = null;
        public static Random RND
        {
            get
            {
                if (_RND == null)
                    _RND = new Random(Seed);

                return _RND;
            }
        }

        public static bool Attached = false;

		public static List<ProblematicProcess> ProblematicProcesses;

		public static Timer KillswitchTimer = new Timer();

        public static bool EmuDirOverride = false;

        public static string EmuDir
		{
			get
			{
				//In attached mode we can just use the directory we're in.
				//We do this as the EmuDir is not set in attached
				if (Attached || EmuDirOverride)
					return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                return (string) AllSpec.VanguardSpec?[VSPEC.EMUDIR];
			}
			set => AllSpec.VanguardSpec.Update(VSPEC.EMUDIR, value);
		}

        public static string EmuAssetsDir
        {
            get => Path.Combine(EmuDir, "ASSETS");
        }

        public static string RtcDir
		{
			get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.RTCDIR];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.RTCDIR, value);
		}
		public static string workingDir
		{
			get => Path.Combine(RtcDir,"WORKING");
		}
		public static string assetsDir
		{
			get => Path.Combine(RtcDir,"ASSETS");
		}
		public static string listsDir
        {
			get => Path.Combine(RtcDir,"LISTS");
        }
		public static string engineTemplateDir
        {
			get => Path.Combine(RtcDir,"ENGINETEMPLATES");
        }

        public static event ProgressBarEventHandler ProgressBarHandler;

        //This is for the UI only but needs to be in here as well
        public static BindingList<ComboBoxItem<string>> LimiterListBindingSource = new BindingList<ComboBoxItem<string>>();
		public static BindingList<ComboBoxItem<string>> ValueListBindingSource = new BindingList<ComboBoxItem<string>>();

		public static bool AllowCrossCoreCorruption
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString(), value);
		}

		public static CorruptionEngine SelectedEngine
		{
			get => (CorruptionEngine)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_SELECTEDENGINE.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_SELECTEDENGINE.ToString(), value);
		}

        public static int CurrentPrecision
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_CURRENTPRECISION.ToString(), value);
        }

        public static int Alignment
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_CURRENTALIGNMENT.ToString()];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_CURRENTALIGNMENT.ToString(), value);
        }

        public static long Intensity
		{
			get => (long)AllSpec.CorruptCoreSpec?[RTCSPEC.CORE_INTENSITY.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_INTENSITY.ToString(), value);
		}

		public static long ErrorDelay
		{
			get => (long)AllSpec.CorruptCoreSpec?[RTCSPEC.CORE_ERRORDELAY.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_ERRORDELAY.ToString(), value);
		}

		public static BlastRadius Radius
		{
			get => (BlastRadius)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_RADIUS.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_RADIUS.ToString(), value);
		}

		public static bool AutoCorrupt
		{
			get => (bool)(AllSpec.CorruptCoreSpec?[RTCSPEC.CORE_AUTOCORRUPT.ToString()] ?? false);
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_AUTOCORRUPT.ToString(), value);
		}

		public static bool DontCleanSavestatesOnQuit
		{
			get => (bool)(AllSpec.CorruptCoreSpec[RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString()] ?? false);
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString(), value);
		}

		public static bool ShowConsole
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_SHOWCONSOLE.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_SHOWCONSOLE.ToString(), value);
		}

		public static bool RerollAddress
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLADDRESS.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLADDRESS.ToString(), value);
				Params.SetParam("REROLL_ADDRESS", value.ToString());
			}
		}

		public static bool RerollSourceAddress
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString(), value);
				Params.SetParam("REROLL_SOURCEADDRESS", value.ToString());
			}
		}
		public static bool RerollDomain
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLDOMAIN.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLDOMAIN.ToString(), value);
				Params.SetParam("REROLL_DOMAIN", value.ToString());
			}
		}

		public static bool RerollSourceDomain
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLSOURCEDOMAIN.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLSOURCEDOMAIN.ToString(), value);
				Params.SetParam("REROLL_SOURCEDOMAIN", value.ToString());
			}
		}
		public static bool RerollIgnoresOriginalSource
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLIGNOREORIGINALSOURCE.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLIGNOREORIGINALSOURCE.ToString(), value);
				Params.SetParam("REROLL_IGNOREORIGINALSOURCE", value.ToString());
			}
		}
		public static bool RerollFollowsCustomEngine
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_REROLLFOLLOWENGINESETTINGS.ToString()];
			set
			{
				AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLFOLLOWENGINESETTINGS.ToString(), value);
				Params.SetParam("REROLL_FOLLOWSCUSTOMENGINE", value.ToString());
			}
		}

		public static bool ExtractBlastlayer
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString(), value);
		}

		public static bool EmulatorOsdDisabled
		{
			get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CORE_EMULATOROSDDISABLED.ToString()];
			set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CORE_EMULATOROSDDISABLED.ToString(), value);
		}

        public static string VanguardImplementationName
        {
            get => (String)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Vanguard Implementation";
        }


        public static bool IsStandaloneUI;
		public static bool IsEmulatorSide;

		public static void Start()
		{
			
		}

		private static  void OneTimeSettingsInitialize()
		{
			RtcCore.RerollSourceAddress = true;
			RtcCore.RerollSourceDomain = true;
            RtcCore.RerollFollowsCustomEngine = true;
            Params.SetParam(RTCSPEC.CORE_EMULATOROSDDISABLED);
        }

		public static void StartUISide()
		{
			try
			{
				Start();
				RegisterCorruptcoreSpec();

                CorruptCore_Extensions.DirectoryRequired(paths: new string[] {
                    RtcCore.workingDir
                    , Path.Combine(RtcCore.workingDir,"TEMP")
                    , Path.Combine(RtcCore.workingDir, "SKS")
                    , Path.Combine(RtcCore.workingDir, "SSK")
                    , Path.Combine(RtcCore.workingDir, "SESSION")
                    , Path.Combine(RtcCore.workingDir, "MEMORYDUMPS")
                    , Path.Combine(RtcCore.workingDir, "MP")
                    , Path.Combine(RtcCore.assetsDir, "CRASHSOUNDS")
                    , Path.Combine(RtcCore.RtcDir, "PARAMS")
                    , Path.Combine(RtcCore.RtcDir, "LISTS")
                    , Path.Combine(RtcCore.RtcDir, "RENDEROUTPUT")
                    , Path.Combine(RtcCore.RtcDir, "ENGINETEMPLATES")
                    , Path.Combine(RtcCore.assetsDir, "PLATESHD")
                });

                if (!Params.IsParamSet("DISCLAIMER_READ"))
					OneTimeSettingsInitialize();

				IsStandaloneUI = true;
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new AbortEverythingException();
			}
		}
		public static void StartEmuSide()
		{
			if (!Attached)
			{
				Start();
				if (KillswitchTimer == null)
					KillswitchTimer = new Timer();

				KillswitchTimer.Interval = 250;
				KillswitchTimer.Tick += KillswitchTimer_Tick;
				KillswitchTimer.Start();
			}
			IsEmulatorSide = true;
		}

		private static void KillswitchTimer_Tick(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.KILLSWITCH_PULSE);
		}

		/**
		* Register the spec on the rtc side
		*/
		public static void RegisterCorruptcoreSpec()
		{
			try { 
			PartialSpec rtcSpecTemplate = new PartialSpec("RTCSpec");
			rtcSpecTemplate["RTCVERSION"] = RtcVersion;

			rtcSpecTemplate[RTCSPEC.RTCDIR] = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "RTC");

            //Engine Settings
            rtcSpecTemplate.Insert(RtcCore.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_NightmareEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_HellgenieEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_DistortionEngine.getDefaultPartial());

			//Custom Engine Config with Nightmare Engine
			RTC_CustomEngine.getDefaultPartial(rtcSpecTemplate);

			rtcSpecTemplate.Insert(StepActions.getDefaultPartial());
			rtcSpecTemplate.Insert(Filtering.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_VectorEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(MemoryDomains.getDefaultPartial());
			rtcSpecTemplate.Insert(StockpileManager_EmuSide.getDefaultPartial());
			rtcSpecTemplate.Insert(Render.getDefaultPartial());


			AllSpec.CorruptCoreSpec = new FullSpec(rtcSpecTemplate, !RtcCore.Attached); //You have to feed a partial spec as a template


			AllSpec.CorruptCoreSpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;
				if(IsStandaloneUI)
					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
				else
					LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
			};

				/*
				if (RTC_StockpileManager.BackupedState != null)
					RTC_StockpileManager.BackupedState.Run();
				else
					CorruptCoreSpec.Update(RTCSPEC.CORE_AUTOCORRUPT.ToString(), false);
					*/
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new AbortEverythingException();
			}
		}

		public static PartialSpec getDefaultPartial()
		{
			try
			{
				var partial = new PartialSpec("RTCSpec");



				partial[RTCSPEC.CORE_SELECTEDENGINE.ToString()] = CorruptionEngine.NIGHTMARE;

                partial[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;
                partial[RTCSPEC.CORE_CURRENTALIGNMENT.ToString()] = 0;
                partial[RTCSPEC.CORE_INTENSITY.ToString()] = 1L;
				partial[RTCSPEC.CORE_ERRORDELAY.ToString()] = 1L;
				partial[RTCSPEC.CORE_RADIUS.ToString()] = BlastRadius.SPREAD;

				partial[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()] = false;
				partial[RTCSPEC.CORE_AUTOCORRUPT.ToString()] = false;

				partial[RTCSPEC.CORE_EMULATOROSDDISABLED.ToString()] = true;
				partial[RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString()] = false;
				partial[RTCSPEC.CORE_SHOWCONSOLE.ToString()] = false;


				if (Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION"))
					partial[RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION] = (Params.ReadParam("ALLOW_CROSS_CORE_CORRUPTION").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString()] = false;

				if (Params.IsParamSet("REROLL_SOURCEADDRESS"))
					partial[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()] = (Params.ReadParam("REROLL_SOURCEADDRESS").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()] = false;

				if (Params.IsParamSet("REROLL_SOURCEDOMAIN"))
					partial[RTCSPEC.CORE_REROLLSOURCEDOMAIN.ToString()] = (Params.ReadParam("REROLL_SOURCEDOMAIN").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLSOURCEDOMAIN.ToString()] = false;

				if (Params.IsParamSet("REROLL_ADDRESS"))
					partial[RTCSPEC.CORE_REROLLADDRESS.ToString()] = (Params.ReadParam("REROLL_ADDRESS").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLADDRESS.ToString()] = false;
				if (Params.IsParamSet("REROLL_DOMAIN"))
					partial[RTCSPEC.CORE_REROLLDOMAIN.ToString()] = (Params.ReadParam("REROLL_DOMAIN").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLDOMAIN.ToString()] = false;


				if (Params.IsParamSet("REROLL_FOLLOWSCUSTOMENGINE"))
					partial[RTCSPEC.CORE_REROLLFOLLOWENGINESETTINGS.ToString()] = (Params.ReadParam("REROLL_FOLLOWSCUSTOMENGINE").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLFOLLOWENGINESETTINGS.ToString()] = false;

				if (Params.IsParamSet("REROLL_USESVALUELIST"))
					partial[RTCSPEC.CORE_REROLLIGNOREORIGINALSOURCE.ToString()] = (Params.ReadParam("REROLL_USESVALUELIST").ToUpper() == "TRUE");
				else
					partial[RTCSPEC.CORE_REROLLIGNOREORIGINALSOURCE.ToString()] = false;


				return partial;
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new AbortEverythingException();

				return null;
			}
		}


		public static void  DownloadProblematicProcesses()
		{
			//Windows does the big dumb: part 11
			WebRequest.DefaultWebProxy = null;

			//Do this on its own thread as downloading the json is slow
			(new Thread(() =>
			{
				string LocalPath = Path.Combine(Params.ParamsDir, "BADPROCESSES");

				string json = "";
				try
				{
					if (File.Exists(LocalPath))
					{
						DateTime lastModified = File.GetLastWriteTime(LocalPath);
						if (lastModified.Date == DateTime.Today)
						{
							ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(File.ReadAllText(LocalPath));
							CheckForProblematicProcesses();
							return;
						}
					}

					using (HttpClient client = new HttpClient())
					{
						client.Timeout = TimeSpan.FromMilliseconds(5000);
						//Using .Result makes it synchronous
						json = client.GetStringAsync("http://redscientist.com/software/rtc/ProblematicProcesses.json")
							.Result;
					}

					File.WriteAllText(LocalPath, json);
				}
				catch (Exception ex)
				{
					if (ex is WebException)
					{
						//Couldn't download the new one so just fall back to the old one if it's there
						Console.WriteLine(ex.ToString());
						if (File.Exists(LocalPath))
						{
							try
							{
								json = File.ReadAllText(LocalPath);
							}
							catch (Exception _ex)
							{
								Console.WriteLine("Couldn't read BADPROCESSES\n\n" + _ex.ToString());
								return;
							}
						}
						else
							return;
					}
					else
					{
						Console.WriteLine(ex.ToString());
					}
				}

				try
				{
					ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(json);
					CheckForProblematicProcesses();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					if (File.Exists(LocalPath))
						File.Delete(LocalPath);
					throw ex;
				}
			})).Start();
		}

		//Checks if any problematic processes are found
		public static bool Warned = false;
		public static void CheckForProblematicProcesses()
		{
			Console.WriteLine(DateTime.Now + "Entering CheckForProblematicProcesses");
			if (Warned || ProblematicProcesses == null)
				return;

			try
			{
				var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x)
					.ToArray();

				//Warn based on loaded processes
				foreach (var item in ProblematicProcesses)
				{
					if (processes.Contains(item.Name.ToUpper()))
					{
						MessageBox.Show(item.Message, "Incompatible Program Detected!");
						Warned = true;
						return;
					}
				}
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new AbortEverythingException();

				return;
			}
			finally
			{
				Console.WriteLine(DateTime.Now + "Exiting CheckForProblematicProcesses");
			}
		}


		public static BlastUnit GetBlastUnit(string _domain, long _address, int precision, int alignment, CorruptionEngine engine)
		{
			try
			{
				//Will generate a blast unit depending on which Corruption Engine is currently set.
				//Some engines like Distortion may not return an Unit depending on the current state on things.

				BlastUnit bu = null;

				switch (engine)
				{
					case CorruptionEngine.NIGHTMARE:
						bu = RTC_NightmareEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.HELLGENIE:
						bu = RTC_HellgenieEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.DISTORTION:
						bu = RTC_DistortionEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.FREEZE:
						bu = RTC_FreezeEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.PIPE:
						bu = RTC_PipeEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.VECTOR:
						bu = RTC_VectorEngine.GenerateUnit(_domain, _address, alignment);
						break;
					case CorruptionEngine.CUSTOM:
						bu = RTC_CustomEngine.GenerateUnit(_domain, _address, precision, alignment);
						break;
					case CorruptionEngine.NONE:
						return null;
				}

				return bu;
			}
			catch (Exception ex)
			{
				if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new AbortEverythingException();

				return null;
			}
		}

		//Generates or applies a blast layer using one of the multiple BlastRadius algorithms

		public static BlastLayer GenerateBlastLayer(string[] selectedDomains, long overrideIntensity = -1)
		{
            if (overrideIntensity == 0)
                return null;

			try
			{
				string Domain = null;
				long MaxAddress = -1;
				long RandomAddress = -1;
				BlastUnit bu;
				BlastLayer bl;

				try
				{
					if (RtcCore.SelectedEngine == CorruptionEngine.BLASTGENERATORENGINE)
					{
						//It will query a BlastLayer generated by the Blast Generator
						bl = RTC_BlastGeneratorEngine.GetBlastLayer();
						if (bl == null)
							//We return an empty blastlayer so when it goes to apply it, it doesn't find a null blastlayer and try and apply to the domains which aren't enabled resulting in an exception
							return new BlastLayer();
                        return bl;
					}

					bl = new BlastLayer();

					if (selectedDomains == null || selectedDomains.Count() == 0)
						return null;

					long intensity = RtcCore.Intensity; //general RTC intensity

                    if (overrideIntensity != -1)
                        intensity = overrideIntensity;

				// Capping intensity at engine-specific maximums
					if ((RtcCore.SelectedEngine == CorruptionEngine.HELLGENIE ||
						RtcCore.SelectedEngine == CorruptionEngine.FREEZE ||
						RtcCore.SelectedEngine == CorruptionEngine.PIPE ||
						RtcCore.SelectedEngine == CorruptionEngine.CUSTOM && RTC_CustomEngine.Lifetime == 0) &&
						intensity > StepActions.MaxInfiniteBlastUnits)
						intensity = StepActions.MaxInfiniteBlastUnits; //Capping for cheat max


					//Spec lookups add up really fast if you have a high intensity so we cache stuff we're going to be looking up over and over again
					var cachedPrecision = CurrentPrecision;
					var cachedDomainSizes = new long[selectedDomains.Length];
					var cachedEngine = RtcCore.SelectedEngine;
					var cachedAlignment = RtcCore.Alignment;

                    for (int i = 0; i < selectedDomains.Length; i++)
					{
						cachedDomainSizes[i] = MemoryDomains.GetInterface(selectedDomains[i]).Size;
					}


					switch (RtcCore.Radius) //Algorithm branching
					{
						case BlastRadius.SPREAD: //Randomly spreads all corruption bytes to all selected domains
						{
							for (int i = 0; i < intensity; i++)
							{
								var r = RtcCore.RND.Next(selectedDomains.Length);
								Domain = selectedDomains[r];

								MaxAddress = cachedDomainSizes[r];
								RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - cachedPrecision);

								bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;
						}

						case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain
						{
							var r = RtcCore.RND.Next(selectedDomains.Length);
							Domain = selectedDomains[r];

							MaxAddress = cachedDomainSizes[r];

							for (int i = 0; i < intensity; i++)
							{
								RandomAddress = RtcCore.RND.NextLong(0,MaxAddress - cachedPrecision);

								bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;
						}
						case BlastRadius.BURST: // 10 shots of 10% chunk
						{
							for (int j = 0; j < 10; j++)
							{
								var r = RtcCore.RND.Next(selectedDomains.Length);
								Domain = selectedDomains[r];

								MaxAddress = cachedDomainSizes[r];

								for (int i = 0; i < (int)((double)intensity / 10); i++)
								{
									RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - cachedPrecision);

									bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;
						}


						case BlastRadius.NORMALIZED: // Blasts based on the size of the largest selected domain. Intensity =  Intensity / (domainSize[largestdomain]/domainSize[currentdomain])
						{
							//Find the smallest domain and base our normalization around it
							//Domains aren't IComparable so I used keys

							long[] domainSize = new long[selectedDomains.Length];
							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];
								domainSize[i] = MemoryDomains.GetInterface(Domain)
									.Size;
							}

							//Sort the arrays
							Array.Sort(domainSize, selectedDomains);

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								//Get the intensity divider. The size of the largest domain divided by the size of the current domain
								long normalized = ((domainSize[selectedDomains.Length - 1] / (domainSize[i])));

								for (int j = 0; j < (intensity / normalized); j++)
								{
									MaxAddress = domainSize[i];
									RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - cachedPrecision);

									bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;
						}

						case BlastRadius.PROPORTIONAL: //Blasts proportionally based on the total size of all selected domains

							long totalSize = cachedDomainSizes.Sum(); //Gets the total size of all selected domains

							long[] normalizedIntensity = new long[selectedDomains.Length]; //matches the index of selectedDomains
							for (int i = 0; i < selectedDomains.Length; i++)
							{   //calculates the proportionnal normalized Intensity based on total selected domains size
								double proportion = (double)cachedDomainSizes[i] / (double)totalSize;
								normalizedIntensity[i] = Convert.ToInt64((double)intensity * proportion);
							}

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								for (int j = 0; j < normalizedIntensity[i]; j++)
								{
									MaxAddress = cachedDomainSizes[i];
									RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - cachedPrecision);

									bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.EVEN: //Evenly distributes the blasts through all selected domains

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								for (int j = 0; j < (intensity / selectedDomains.Length); j++)
								{
									MaxAddress = cachedDomainSizes[i];
									RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - cachedPrecision);

									bu = GetBlastUnit(Domain, RandomAddress, cachedPrecision, cachedAlignment, cachedEngine);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NONE: //Shouldn't ever happen but handled anyway
							return null;
					}

					if (bl.Layer.Count == 0)
						return null;
					else
						return bl;
				}
				catch (Exception ex)
				{
					string additionalInfo = "";

					if (MemoryDomains.GetInterface(Domain) == null)
					{
						additionalInfo = "Unable to get an interface to the selected memory domain! \nTry clicking the Auto-Select Domains button to refresh the domains!\n\n";
					}

					throw new CustomException(ex.Message, additionalInfo + ex.StackTrace + ex.InnerException);

				}
			}
			catch (Exception ex)
			{
				var ex2 = new CustomException("Something went wrong in the RTC Core | " + ex.Message, (RtcCore.AutoCorrupt ? "Autocorrupt was turned off for your safety\n\n" : "") + ex.StackTrace);
				var dr = CloudDebug.ShowErrorDialog(ex2, true);


				if (RtcCore.AutoCorrupt)
				{
					RtcCore.AutoCorrupt = false;
					LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.ERROR_DISABLE_AUTOCORRUPT);
				}

				if (dr == DialogResult.Abort)
					throw new AbortEverythingException();

				return null;
			}
		}

		public static BlastTarget GetBlastTarget()
		{
			//Standalone version of BlastRadius SPREAD

			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;

			string[] _selectedDomains = (string[])AllSpec.UISpec["SELECTEDDOMAINS"];

			Domain = _selectedDomains[RtcCore.RND.Next(_selectedDomains.Length)];

			MaxAddress = MemoryDomains.GetInterface(Domain).Size;
			RandomAddress = RtcCore.RND.NextLong(0, MaxAddress - 1);

			return new BlastTarget(Domain, RandomAddress);
		}

		public static string GetRandomKey()
		{
			//Generates unique string ids that are human-readable, unlike GUIDs
			string Key = RtcCore.RND.Next(1, 9999).ToString() + RtcCore.RND.Next(1, 9999).ToString() + RtcCore.RND.Next(1, 9999).ToString() + RtcCore.RND.Next(1, 9999).ToString();
			return Key;
		}


		public static void GenerateAndBlast()
        {
            BlastLayer bl = null;
            void a()
            {
                    //We pull the domains here because if the syncsettings changed, there's a chance the domains changed
                    string[] domains = (string[])AllSpec.UISpec["SELECTEDDOMAINS"];


                    var cpus = Environment.ProcessorCount;

                    if (cpus == 1 || AllSpec.VanguardSpec[VSPEC.SUPPORTS_MULTITHREAD] == null)
                    {
                        bl = RtcCore.GenerateBlastLayer(domains);
                    }
                    else
                    {
                        //if emulator supports multithreaded access of the domains, disregard the emulation thread and just span threads...
                        long reminder = RtcCore.Intensity % (cpus - 1);
                        long splitintensity = (RtcCore.Intensity - reminder) / (cpus - 1);

                        Task<BlastLayer>[] tasks = new Task<BlastLayer>[cpus];
                        for (int i = 0; i < cpus; i++)
                        {
                            long requestedIntensity = splitintensity;

                            if (i == 0 && reminder != 0)
                                requestedIntensity = reminder;

                            tasks[i] = Task.Factory.StartNew(() => RtcCore.GenerateBlastLayer(domains, requestedIntensity));
                        }

                        Task.WaitAll(tasks);

                        bl = tasks[0].Result ?? new BlastLayer();

                        if (tasks.Length > 1)
                            for (int i = 1; i < tasks.Length; i++)
                                if (tasks[i].Result != null)
                                    bl.Layer.AddRange(tasks[i].Result.Layer);

                        if (bl.Layer.Count == 0)
                            bl = null;
                    }

                    bl?.Apply(false, true);
            }
            //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
            if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
            {
                SyncObjectSingleton.FormExecute(a);
            }
            else //We can just do everything on the emulation thread as it'll block
                SyncObjectSingleton.EmuThreadExecute(a, true);
		}

        /*
		public static void ApplyBlastLayer(BlastLayer bl)
		{
			if(bl.Layer != null)
				bl.Apply();
		}*/

        public static void OnProgressBarUpdate(object sender, ProgressBarEventArgs e )
        {
            ProgressBarHandler?.Invoke(sender, e);
        }
    }
}
