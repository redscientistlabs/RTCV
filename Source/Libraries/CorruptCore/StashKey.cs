namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Ceras;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using RTCV.NetCore;

    [Serializable]
    [MemberConfig(TargetMember.AllPublic)]
    public class StashKey : ICloneable, INote
    {
        [Exclude]
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string RomFilename { get; set; }
        public string RomShortFilename { get; set; }
        public byte[] RomData { get; set; }

        public string StateShortFilename { get; set; }
        public string StateFilename { get; set; }
        public byte[] StateData { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StashKeySavestateLocation StateLocation { get; set; } = StashKeySavestateLocation.SESSION;

        public Dictionary<string, string> KnownLists { get; set; } = new Dictionary<string, string>();

        public string SystemName { get; set; }
        public string SystemDeepName { get; set; }
        public string SystemCore { get; set; }
        public List<string> SelectedDomains { get; set; } = new List<string>();
        public string GameName { get; set; }
        public string SyncSettings { get; set; }
        public string Note { get; set; }

        public string Key { get; set; }
        public string ParentKey { get; set; }
        public BlastLayer BlastLayer { get; set; }

        private string _alias;
        public string Alias
        {
            get => _alias ?? Key;
            set => _alias = value;
        }

        public StashKey()
        {
            var key = RtcCore.GetRandomKey();
            string parentkey = null;
            BlastLayer blastlayer = new BlastLayer();
            StashKeyConstructor(key, parentkey, blastlayer);
        }

        public StashKey(string key, string parentkey, BlastLayer blastlayer)
        {
            StashKeyConstructor(key, parentkey, blastlayer);
        }

        private void StashKeyConstructor(string key, string parentkey, BlastLayer blastlayer)
        {
            Key = key;
            ParentKey = parentkey;
            BlastLayer = blastlayer;

            RomFilename = (string)AllSpec.VanguardSpec?[VSPEC.OPENROMFILENAME] ?? "ERROR";
            SystemName = (string)AllSpec.VanguardSpec?[VSPEC.SYSTEM] ?? "ERROR";
            SystemCore = (string)AllSpec.VanguardSpec?[VSPEC.SYSTEMCORE] ?? "ERROR";
            GameName = (string)AllSpec.VanguardSpec?[VSPEC.GAMENAME] ?? "ERROR";
            SyncSettings = (string)AllSpec.VanguardSpec?[VSPEC.SYNCSETTINGS] ?? "";

            this.SelectedDomains = ((string[])AllSpec.UISpec[UISPEC.SELECTEDDOMAINS]).ToList();
        }

        public object Clone()
        {
            object sk = ObjectCopierCeras.Clone(this);
            ((StashKey)sk).Key = RtcCore.GetRandomKey();
            ((StashKey)sk).Alias = null;
            return sk;
        }

        internal static void SetCore(StashKey sk)
        {
            SetCore(sk.SystemName, sk.SystemCore);
        }

        public static void SetCore(string systemName, string systemCore)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.KeySetSystemCore, new object[] { systemName, systemCore }, true);
        }

        public override string ToString()
        {
            return Alias;
        }

        /// <summary>
        /// Can be called from UI Side
        /// </summary>
        public bool Run()
        {
            StockpileManagerUISide.CurrentStashkey = this;
            return StockpileManagerUISide.ApplyStashkey(this);
        }

        /// <summary>
        /// Can be called from UI Side
        /// </summary>
        public void RunOriginal()
        {
            StockpileManagerUISide.CurrentStashkey = this;
            StockpileManagerUISide.OriginalFromStashkey(this);
        }

        public byte[] EmbedState()
        {
            if (StateFilename == null)
            {
                return null;
            }

            if (this.StateData != null)
            {
                return this.StateData;
            }

            byte[] stateData = File.ReadAllBytes(StateFilename);
            this.StateData = stateData;

            return stateData;
        }

        public bool DeployState()
        {
            if (StateShortFilename == null || this.StateData == null)
            {
                return false;
            }

            var deployedStatePath = GetSavestateFullPath();

            if (File.Exists(deployedStatePath))
            {
                return true;
            }

            File.WriteAllBytes(deployedStatePath, this.StateData);

            return true;
        }

        public string GetSavestateFullPath()
        {
            return Path.Combine(RtcCore.workingDir, this.StateLocation.ToString(), this.GameName + "." + this.ParentKey + ".timejump.State"); // get savestate name
        }

        //Todo - Replace this when compat is broken
        public void PopulateKnownLists()
        {
            if (BlastLayer.Layer == null) {
                MessageBox.Show($"Something went really wrong. Stashkey {Alias}.\nThere doesn't appear to be a linked blastlayer.\nWill attempt to continue saving. If save fails, remove {Alias} from your stockpile and save again.\nSend this stockpile and any info on how you got into this state to the devs.");
                return;
            }
            List<string> knownListKeys = new List<string>();
            foreach (var bu in BlastLayer.Layer.Where(x => x.LimiterListHash != null))
            {
                logger.Trace("Looking for knownlist {bu.LimiterListHash}", bu.LimiterListHash);
                if (knownListKeys.Contains(bu.LimiterListHash))
                {
                    logger.Trace("knownListKeys already contains {bu.LimiterListHash}", bu.LimiterListHash);
                    logger.Trace("Done\n");
                    continue;
                }

                logger.Trace("Adding {bu.LimiterListHash} to knownListKeys", bu.LimiterListHash);
                knownListKeys.Add(bu.LimiterListHash);

                logger.Trace("Getting name of {bu.LimiterListHash} from Hash2NameDico", bu.LimiterListHash);
                Filtering.Hash2NameDico.TryGetValue(bu.LimiterListHash, out string name);

                if (name == null)
                {
                    name = "UNKNOWN_" + Filtering.StockpileListCount++;
                }
                else
                {
                    name = Path.GetFileNameWithoutExtension(name);
                }

                logger.Trace("Setting KnownLists[{bu.LimiterListHash}] to {name}", bu.LimiterListHash, name);
                this.KnownLists[bu.LimiterListHash] = name;
                logger.Trace("Done");
            }
        }
    }
}
