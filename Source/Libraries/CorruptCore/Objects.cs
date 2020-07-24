namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Ceras;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using RTCV.Common.CustomExtensions;
    using RTCV.NetCore;
    using Exception = System.Exception;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class SaveStateKey
    {
        public StashKey StashKey = null;
        public string Text = "";

        public SaveStateKey(StashKey stashKey, string text)
        {
            StashKey = stashKey;
            Text = text;
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class SaveStateKeys
    {
        public string VanguardImplementation { get; set; }
        public List<StashKey> StashKeys = new List<StashKey>();
        public List<string> Text = new List<string>();

        public SaveStateKeys()
        {
            VanguardImplementation = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BlastTarget
    {
        public string Domain = null;
        public long Address = 0;

        public BlastTarget(string _domain, long _address)
        {
            Domain = _domain;
            Address = _address;
        }
    }

    [Ceras.MemberConfig(TargetMember.All)]
    [Serializable]
    [XmlInclude(typeof(BlastUnit))]
    public class BlastLayer : ICloneable, INote
    {
        public List<BlastUnit> Layer;

        public BlastLayer()
        {
            Layer = new List<BlastUnit>();
        }

        public BlastLayer(List<BlastUnit> _layer)
        {
            Layer = _layer;
        }

        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        public void Apply(bool storeUncorruptBackup, bool followMaximums = false, bool mergeWithPrevious = false)
        {
            if (storeUncorruptBackup && this != StockpileManager_EmuSide.UnCorruptBL)
            {
                BlastLayer UnCorruptBL_Backup = null;
                BlastLayer CorruptBL_Backup = null;

                if (mergeWithPrevious)
                {
                    //UnCorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.UnCorruptBL?.Clone();
                    //CorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.CorruptBL?.Clone();

                    UnCorruptBL_Backup = StockpileManager_EmuSide.UnCorruptBL;
                    CorruptBL_Backup = StockpileManager_EmuSide.CorruptBL;
                }

                StockpileManager_EmuSide.UnCorruptBL = GetBackup();
                StockpileManager_EmuSide.CorruptBL = this;

                if (mergeWithPrevious)
                {
                    if (UnCorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.UnCorruptBL.Layer == null)
                            StockpileManager_EmuSide.UnCorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.UnCorruptBL.Layer.AddRange(UnCorruptBL_Backup.Layer);
                    }

                    if (CorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.CorruptBL.Layer == null)
                            StockpileManager_EmuSide.CorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.CorruptBL.Layer.AddRange(CorruptBL_Backup.Layer);
                    }
                }
            }

            bool success;
            bool UseRealtime = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME];

            try
            {
                foreach (BlastUnit bb in Layer)
                {
                    if (bb == null) //BlastCheat getBackup() always returns null so they can happen and they are valid
                    {
                        success = true;
                    }
                    else
                    {
                        success = bb.Apply(true);
                    }

                    if (!success)
                    {
                        throw new Exception(
                        "One of the BlastUnits in the BlastLayer failed to Apply().\n\n" +
                        "The operation was cancelled");
                    }
                }

                //Only filter if there are actually enabled units
                if (Layer.Any(x => x.IsEnabled))
                {
                    StepActions.FilterBuListCollection();

                    //If we're not using realtime, we execute right away.
                    if (!UseRealtime)
                    {
                        StepActions.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                            "An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
                            "The operation was cancelled\n\n" + ex.Message
                            );
            }
            finally
            {
                if (followMaximums)
                {
                    StepActions.RemoveExcessInfiniteStepUnits();
                }
            }
        }

        public BlastLayer GetBakedLayer()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBakedUnit()));

            return new BlastLayer(BackupLayer);
        }

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBackup()).Where(it => it != null));

            return new BlastLayer(BackupLayer);
        }

        public void Reroll()
        {
            foreach (BlastUnit bu in Layer.Where(x => x.IsLocked == false))
            {
                bu.Reroll();
            }
        }

        public void RasterizeVMDs(string vmdToRasterize = null)
        {
            List<BlastUnit> l = new List<BlastUnit>();
            //Inserting turned out to be WAY too cpu intensive, so just sacrifice the ram and rebuild the layer
            foreach (var bu in Layer)
            {
                var u = bu.GetRasterizedUnits(vmdToRasterize);
                l.AddRange(u);
            }
            this.Layer = l;
        }

        private string shared = "[DIFFERENT]";

        [JsonIgnore]
        public string Note
        {
            get
            {
                if (Layer.All(x => x.Note == Layer.First().Note))
                {
                    return Layer.FirstOrDefault()?.Note;
                }
                return shared;
            }
            set
            {
                if (value == shared)
                {
                    return;
                }
                foreach (BlastUnit bu in Layer)
                {
                    bu.Note = value;
                }
            }
        }

        public void SanitizeDuplicates()
        {
            /*
            Layer = Layer.GroupBy(x => new { x.Address, x.Domain })
              .Where(g => g.Count() > 1)
              .Select(y => y.First())
              .ToList();
              */

            List<BlastUnit> bul = new List<BlastUnit>(Layer.ToArray().Reverse());
            List<ValueTuple<string, long>> usedAddresses = new List<ValueTuple<string, long>>();

            foreach (BlastUnit bu in bul)
            {
                if (!usedAddresses.Contains(new ValueTuple<string, long>(bu.Domain, bu.Address)) && !bu.IsLocked)
                {
                    usedAddresses.Add(new ValueTuple<string, long>(bu.Domain, bu.Address));
                }
                else if (!bu.IsLocked)
                {
                    Layer.Remove(bu);
                }
            }
        }
    }

    /// <summary>
    /// Working data for BlastUnits.
    /// Not serialized
    /// </summary>
    [Ceras.MemberConfig(TargetMember.None)]
    public class BlastUnitWorkingData
    {
        //We Calculate a LastFrame at the beginning of execute
        [NonSerialized]
        public int LastFrame = -1;
        //We calculate ExecuteFrameQueued which is the ExecuteFrame + the currentframe that was calculated at the time of it entering the execution pool
        [NonSerialized]
        public int ExecuteFrameQueued = 0;

        //We use ApplyValue so we don't need to keep re-calculating the tiled value every execute if we don't have to.
        [NonSerialized]
        public byte[] ApplyValue = null;

        //The data that has been backed up. This is a list of bytes so if they start backing up at IMMEDIATE, they can have historical backups
        [NonSerialized]
        public Queue<byte[]> StoreData = new Queue<byte[]>();
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BlastUnit : INote
    {
        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        [Category("Settings")]
        [Description("Whether or not the BlastUnit will apply if the stashkey is run")]
        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; } = true;

        [Category("Settings")]
        [Description("Whether or not this unit will be affected by batch operations (disable 50, invert, etc)")]
        [DisplayName("Locked")]
        public bool IsLocked { get; set; } = false;

        [Category("Data")]
        [Description("Whether or not the unit's values need to be flipped due to endianess")]
        [DisplayName("Big Endian")]
        public bool BigEndian { get; set; }

        [Category("Data")]
        [Description("The domain this unit will target")]
        [DisplayName("Domain")]
        public string Domain { get; set; }

        [Category("Data")]
        [Description("The address this unit will target")]
        [DisplayName("Address")]
        public long Address { get; set; }

        private int precision;

        [Category("Data")]
        [Description("The precision of this unit")]
        [DisplayName("Precision")]
        public int Precision
        {
            get => precision;
            set
            {
                int max = 16348; //The textbox breaks if I go over 20k
                if (value > max)
                {
                    value = max;
                }
                //Cache the old precision
                int oldPrecision = precision;
                //Update the precision
                precision = value;

                //If the user is changing the precision and already has a Value set, we need to update that array
                if (Value != null && oldPrecision != precision && Value.Length != precision)
                {
                    //If the precision is being set to 0, force it back to 1 and fill in an empty value
                    if (precision < 1)
                    {
                        Value = new byte[1];
                        precision = 1;
                    }
                    //If Value was 0 bytes long for some reason (deserialization?), just make a new byte of the correct length
                    else if (Value.Length == 0)
                    {
                        Value = new byte[value];
                    }
                    //Figure out the new length
                    else
                    {
                        //If there was no precision set, set it to 1
                        if (oldPrecision == 0)
                        {
                            oldPrecision = 1;
                        }

                        byte[] temp = new byte[precision];
                        //If the new unit is larger, copy it over left padded
                        if (precision > oldPrecision)
                        {
                            Value.CopyTo(temp, precision - oldPrecision);
                        }
                        //If the new unit is smaller, truncate it (first X bytes cut off)
                        else
                        {
                            int j = 0;
                            for (int i = oldPrecision - precision; i < oldPrecision; i++)
                            {
                                temp[j] = Value[i];
                                j++;
                            }
                        }
                        Value = temp;
                    }
                }
            }
        }

        private BlastUnitSource source;

        [Category("Source")]
        [Description("The source for the value for this unit for STORE mode")]
        [DisplayName("Source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BlastUnitSource Source
        {
            get => source;
            set
            {
                //Cleanup from other types of units
                switch (value)
                {
                    case BlastUnitSource.STORE:
                        {
                            Value = null;
                            break;
                        }

                    case BlastUnitSource.VALUE:
                        {
                            if (Value == null)
                            {
                                Value = new byte[Precision];
                            }

                            break;
                        }
                }
                source = value;
            }
        }

        [Category("Store")]
        [Description("The time when the store will take place")]
        [DisplayName("Store Time")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreTime StoreTime { get; set; }

        [Category("Store")]
        [Description("The type of store that when the store will take place")]
        [DisplayName("Store Type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreType StoreType { get; set; }

        [JsonIgnore]
        [Category("Value")]
        [Description("The value used for the BlastUnit in VALUE mode")]
        [DisplayName("Value")]
        public byte[] Value { get; set; }

        [Category("Value")]
        [Description("Gets and sets Value[] through a string. Used for Textboxes")]
        [DisplayName("ValueString")]
        [Ceras.Exclude]
        public string ValueString
        {
            get
            {
                if (Value == null)
                {
                    return string.Empty;
                }

                return BitConverter.ToString(this.Value).Replace("-", string.Empty);
            }
            set
            {
                //If there's no precision, use the length of the string rounded up
                int p = this.Precision;
                if (p == 0 && value.Length != 0)
                {
                    p = (value.Length / 2) + (Value.Length % 2);
                }
                var temp = CorruptCore_Extensions.StringToByteArrayPadLeft(value, p);
                if (temp != null)
                {
                    this.Value = temp;
                }
            }
        }

        [Category("Store")]
        [Description("The domain used for the STORE operation")]
        [DisplayName("Source Domain")]
        public string SourceDomain { get; set; }

        [Category("Store")]
        [Description("The address used for the STORE operation")]
        [DisplayName("Source Address")]
        public long SourceAddress { get; set; }

        [Category("Modifiers")]
        [Description("How much to tilt the value before poking memory")]
        [DisplayName("Tilt Value")]
        public BigInteger TiltValue { get; set; }

        public int ExecuteFrame { get; set; }
        public int Lifetime { get; set; }
        public bool Loop { get; set; } = false;

        public int? LoopTiming { get; set; } = null;

        [Category("Limiter")]
        [Description("What mode to use for the limiter in STORE mode")]
        [DisplayName("Store Limiter Source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreLimiterSource StoreLimiterSource { get; set; }

        [Category("Limiter")]
        [Description("When to apply the limiter list")]
        [DisplayName("Limiter List")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LimiterTime LimiterTime { get; set; }

        [Category("Limiter")]
        [Description("The hash of the Limiter List in use")]
        [DisplayName("Limiter List Hash")]
        public string LimiterListHash { get; set; }

        [Category("Limiter")]
        [Description("Invert the limiter so the unit only applies if the value doesn't match the limiter")]
        [DisplayName("Invert Limiter")]
        public bool InvertLimiter { get; set; }

        [Category("Data")]
        [Description("Whether or not the unit was originally seeded with a value list")]
        [DisplayName("Generated Using Value List")]
        public bool GeneratedUsingValueList { get; set; }

        [Category("Misc")]
        [Description("Note associated with this unit")]
        public string Note { get; set; }

        //Don't serialize this
        [NonSerialized, XmlIgnore, JsonIgnore, Ceras.Exclude]
        public BlastUnitWorkingData Working;

        /// <summary>
        /// Creates a Blastunit that utilizes a backup.
        /// </summary>
        /// <param name="storeType">The type of store</param>
        /// <param name="storeTime">The time of the store</param>
        /// <param name="domain">The domain of the blastunit</param>
        /// <param name="address">The address of the blastunit</param>
        /// <param name="bigEndian">If the Blastunit is being applied to a big endian system. Results in the bytes being flipped before apply</param>
        /// <param name="applyFrame">The frame on which the BlastUnit will start executing</param>
        /// <param name="lifetime">How many frames the BlastUnit will execute for. 0 for infinite</param>
        /// <param name="note"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isLocked"></param>
        public BlastUnit(StoreType storeType, StoreTime storeTime,
            string domain, long address, string sourceDomain, long sourceAddress, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
            string note = null, bool isEnabled = true, bool isLocked = false, int? loopTiming = null)
        {
            Source = BlastUnitSource.STORE;
            StoreTime = storeTime;
            StoreType = storeType;

            Domain = domain;
            Address = address;
            SourceDomain = sourceDomain;
            SourceAddress = sourceAddress;
            Precision = precision;
            BigEndian = bigEndian;
            ExecuteFrame = executeFrame;
            Lifetime = lifetime;
            Note = note;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
            LoopTiming = loopTiming;
        }

        /// <summary>
        /// Creates a BlastUnit that uses a byte array value as the value
        /// </summary>
        /// <param name="value">The value of the BlastUnit</param>
        /// <param name="domain">The domain the blastunit lies in</param>
        /// <param name="address"></param>
        /// <param name="bigEndian"></param>
        /// <param name="executeFrame"></param>
        /// <param name="lifetime"></param>
        /// <param name="note"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isLocked"></param>
        public BlastUnit(byte[] value,
            string domain, long address, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
            string note = null, bool isEnabled = true, bool isLocked = false, bool generatedUsingValueList = false, int? loopTiming = null)
        {
            Source = BlastUnitSource.VALUE;
            //Precision has to be set before value
            Precision = precision;
            Value = value;

            Domain = domain;
            Address = address;
            ExecuteFrame = executeFrame;
            Lifetime = lifetime;
            Note = note;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
            GeneratedUsingValueList = generatedUsingValueList;
            BigEndian = bigEndian;
            LoopTiming = loopTiming;
        }

        public BlastUnit()
        {
        }

        /// <summary>
        /// Returns a blastunit that's a subunit of the current unit
        /// </summary>
        /// <param name="start">Where to start in the unit</param>
        /// <param name="end">Where to end (INCLUSIVE)</param>
        /// <returns></returns>
        public BlastUnit GetSubUnit(int start, int end)
        {
            BlastUnit bu = new BlastUnit()
            {
                Precision = end - start,
                Address = this.Address + start,
                Domain = this.Domain,
                SourceAddress = this.SourceAddress,
                SourceDomain = this.SourceDomain,
                Source = this.Source,
                ExecuteFrame = this.ExecuteFrame,
                Lifetime = this.Lifetime,
                LimiterTime = this.LimiterTime,
                Loop = this.Loop,
                InvertLimiter = this.InvertLimiter,
                StoreLimiterSource = this.StoreLimiterSource,
                GeneratedUsingValueList = this.GeneratedUsingValueList,
                BigEndian = this.BigEndian,
                IsLocked = this.IsLocked,
                Note = this.Note,
                StoreTime = this.StoreTime,
                StoreType = this.StoreType,
                IsEnabled = this.IsEnabled,
                LimiterListHash = this.LimiterListHash,
                LoopTiming = this.LoopTiming,
            };

            if (bu.Source == BlastUnitSource.STORE)
            {
                bu.SourceAddress += start;

                if (BigEndian && start == (precision - 1))
                {
                    bu.TiltValue = TiltValue;
                }
                else if (!BigEndian && start == 0)
                {
                    bu.TiltValue = TiltValue;
                }
                else
                {
                    bu.TiltValue = 0;
                }
            }
            else
            {
                bu.Value = new byte[bu.precision];
                for (int i = 0; i < bu.precision; i++)
                {
                    if (BigEndian)
                    {
                        bu.Value[i] = Value[end - (i + 1)];
                    }
                    else
                    {
                        bu.Value[i] = Value[start + i];
                    }

                    //If we have a tilt, calculate it and bake it into the value
                    if (this.TiltValue != 0)
                    {
                        unchecked
                        {
                            if (BigEndian)
                            {
                                bu.Value[i] += (TiltValue.ToByteArray().PadLeft(this.precision))[end - (i + 1)];
                            }
                            else
                            {
                                bu.Value[i] += (TiltValue.ToByteArray().PadLeft(this.precision).FlipBytes())[start + i];
                            }
                        }
                    }
                    else
                    {
                        bu.TiltValue = 0;
                    }
                }
            }

            return bu;
        }

        /// <summary>
        /// Rasterizes VMDs to their underlying domain
        /// This returns a blastunit[] because if we have a non-contiguous vmd, we need to return multiple units
        /// </summary>
        public List<BlastUnit> GetRasterizedUnits(string vmdToRasterize = null)
        {
            if (vmdToRasterize == null)
            {
                vmdToRasterize = "[V]";
            }

            bool breakDown = false;
            BlastLayer l = new BlastLayer();
            //Todo - Change this to a more unique marker than [V]?
            if (Domain.Contains(vmdToRasterize))
            {
                string domain = (string)Domain.Clone();
                long address = Address;

                if (MemoryDomains.VmdPool[domain] is VirtualMemoryDomain vmd)
                {
                    long lastAddress = vmd.GetRealAddress(address);
                    string lastDomain = vmd.GetRealDomain(address);
                    for (int i = 1; i < this.Precision; i++)
                    {
                        var a = vmd.GetRealAddress(address + i);
                        var d = vmd.GetRealDomain(address + i);
                        if (a != lastAddress + 1 || d != lastDomain)
                        {
                            breakDown = true;
                            break;
                        }

                        lastAddress = a;
                        lastDomain = d;
                    }
                    if (!breakDown)
                    {
                        Domain = vmd.GetRealDomain(address);
                        Address = vmd.GetRealAddress(address);
                    }
                }
                else
                {
                    Domain = "ERROR";
                    Address = -1;
                }
            }
            if (SourceDomain?.Contains(vmdToRasterize) ?? false)
            {
                string sourceDomain = (string)SourceDomain.Clone();
                long sourceAddress = SourceAddress;

                if (MemoryDomains.VmdPool[sourceDomain] is VirtualMemoryDomain vmd)
                {
                    long lastAddress = vmd.GetRealAddress(sourceAddress);
                    string lastDomain = vmd.GetRealDomain(sourceAddress);
                    for (int i = 1; i < this.Precision; i++)
                    {
                        var a = vmd.GetRealAddress(sourceAddress + i);
                        var d = vmd.GetRealDomain(sourceAddress + i);
                        if (a != lastAddress + 1 || d != lastDomain)
                        {
                            breakDown = true;
                            break;
                        }
                        lastAddress = a;
                        lastDomain = d;
                    }

                    if (!breakDown)
                    {
                        SourceDomain = vmd.GetRealDomain(sourceAddress);
                        SourceAddress = vmd.GetRealAddress(sourceAddress);
                    }
                }
                else
                {
                    Domain = "ERROR";
                    Address = -1;
                }
            }

            if (breakDown)
            {
                for (int i = 0; i < this.Precision; i++)
                {
                    var bu = this.GetSubUnit(i, i + 1);
                    l.Layer.Add(bu);
                }
                l.RasterizeVMDs(); //recursively do this
            }
            else
            {
                l.Layer.Add(this);
            }

            return l.Layer;
        }

        /// <summary>
        /// Adds a blastunit to the execution pool
        /// </summary>
        /// <returns></returns>
        public bool Apply(bool dontFilter, bool overrideExecuteFrame = false)
        {
            if (!IsEnabled)
            {
                return true;
            }
            //Create our working data object
            this.Working = new BlastUnitWorkingData();

            //We need to grab the value to freeze
            if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.IMMEDIATE)
            {
                //If it's one time, store the backup. Otherwise add it to the backup pool
                if (StoreType == StoreType.ONCE)
                {
                    StoreBackup();
                }
                else
                {
                    StepActions.StoreDataPool.Add(this);
                }
            }
            //Add it to the execution pool
            StepActions.AddBlastUnit(this, overrideExecuteFrame);

            if (dontFilter)
            {
                return true;
            }

            StepActions.FilterBuListCollection();

            return true;
        }

        /// <summary>
        /// Executes (applies) a blastunit. This shouldn't be called manually.
        /// If you want to execute a blastunit, add it to the execution pool using Apply()
        /// Returns false
        /// </summary>
        public ExecuteState Execute(bool UseRealtime = true)
        {
            if (!IsEnabled)
            {
                return ExecuteState.NOTEXECUTED;
            }

            try
            {
                //Get our memory interface
                MemoryInterface mi = MemoryDomains.GetInterface(Domain);
                if (mi == null)
                {
                    return ExecuteState.NOTEXECUTED;
                }

                //Limiter handling
                if (LimiterListHash != null && LimiterTime == LimiterTime.EXECUTE)
                {
                    if (!LimiterCheck(mi))
                    {
                        return ExecuteState.SILENTERROR;
                    }
                }

                if (Working == null)
                {
                    if (Debugger.IsAttached)
                    {
                        throw new Exception("wtf");
                    }

                    RTCV.Common.Logging.GlobalLogger.Error("Blastunit: WORKING WAS NULL {this}", this);
                    return ExecuteState.SILENTERROR;
                }
                switch (Source)
                {
                    case (BlastUnitSource.STORE):
                        {
                            if (Working.StoreData == null)
                            {
                                RTCV.Common.Logging.GlobalLogger.Error("Blastunit: STOREDATA WAS NULL {this}", this);
                                return ExecuteState.SILENTERROR;
                            }

                            //If there's no stored data, return out.
                            if (Working.StoreData.Count == 0)
                            {
                                return ExecuteState.NOTEXECUTED;
                            }

                            //Apply the value we have stored
                            Working.ApplyValue = Working.StoreData.Peek();

                            //Remove it from the store pool if it's a continuous backup
                            if (StoreType == StoreType.CONTINUOUS)
                            {
                                Working.StoreData.Dequeue();
                            }

                            //All the data is already handled by GetStoreBackup, so we can just poke
                            for (int i = 0; i < Precision; i++)
                            {
                                mi.PokeByte(Address + i, Working.ApplyValue[i]);
                            }
                            break;
                        }
                    case (BlastUnitSource.VALUE):
                        {
                            //We only calculate it once for Value and then store it in ApplyValue.
                            //If the length has changed (blast editor) we gotta recalc it
                            if (Working.ApplyValue == null)
                            {
                                //We don't want to modify the original array
                                Working.ApplyValue = (byte[])Value.Clone();

                                //Calculate the actual value to apply
                                CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref Working.ApplyValue, TiltValue, false); //We don't use the endianess toggle here as we always store value units as little endian

                                //Flip it if it's big endian
                                if (this.BigEndian)
                                {
                                    Working.ApplyValue.FlipBytes();
                                }
                            }

                            //Poke the memory
                            for (int i = 0; i < Precision; i++)
                            {
                                mi.PokeByte(Address + i, Working.ApplyValue[i]);
                            }
                            break;
                        }
                }
            }
            catch (IOException)
            {
                var dr = MessageBox.Show(
                    "An IOException occured during Execute().\nThis probably means whatever is being corrupted can't be accessed.\nIf you're corrupting a file, close any program that might be using it.\n\nAborting corrupt.\nSend this error to the devs?",
                    "IOException during Execute()", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    throw;
                }
                return ExecuteState.HANDLEDERROR;
            }
            return ExecuteState.EXECUTED;
        }

        /// <summary>
        /// Adds a backup to the end of the StoreData queue
        /// </summary>
        public void StoreBackup()
        {
            if (SourceDomain == null)
            {
                return;
            }

            //Snag our memory interface
            MemoryInterface mi = MemoryDomains.GetInterface(SourceDomain);

            if (mi == null)
            {
                throw new Exception(
                    $"Memory Domain error. Mi was null. If you know how to reproduce this, let the devs know");
            }

            //Get the value
            byte[] value = new byte[Precision];
            for (int i = 0; i < Precision; i++)
            {
                value[i] = mi.PeekByte(SourceAddress + i);
            }

            //Calculate the final value after adding the tilt value
            if (TiltValue != 0)
            {
                CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref value, TiltValue, this.BigEndian);
            }

            //Enqueue it
            Working.StoreData.Enqueue(value);
        }

        /// <summary>
        /// Returns a unit baked to VALUE with a lifetime of 1
        /// </summary>
        /// <returns></returns>
        public BlastUnit GetBakedUnit()
        {
            if (!IsEnabled)
            {
                return null;
            }

            //Grab our mi
            MemoryInterface mi = MemoryDomains.GetInterface(Domain);
            if (mi == null)
            {
                return null;
            }

            //Grab the value
            byte[] _value = new byte[Precision];
            for (int i = 0; i < Precision; i++)
            {
                _value[i] = mi.PeekByte(Address + i);
            }
            //Return a new unit
            //Note the false on bigEndian. That's because when reading from memory we're always reading from left to right and we don't want to flip the bytes twice
            return new BlastUnit(_value, Domain, Address, Precision, false, 0, 1, Note, IsEnabled, IsLocked);
        }

        private bool ReturnFalseAndDequeueIfContinuousStore()
        {
            if (this.Source == BlastUnitSource.STORE && this.StoreType == StoreType.CONTINUOUS && this.LimiterTime != LimiterTime.GENERATE)
            {
                if (this.Working.StoreData.Count > 0)
                {
                    this.Working.StoreData.Dequeue();
                }
            }

            return false;
        }

        public bool LimiterCheck(MemoryInterface destMI)
        {
            if (Source == BlastUnitSource.STORE)
            {
                if (StoreLimiterSource == StoreLimiterSource.ADDRESS || StoreLimiterSource == StoreLimiterSource.BOTH)
                {
                    if (Filtering.LimiterPeekBytes(Address,
                        Address + Precision, Domain, LimiterListHash, destMI))
                    {
                        if (InvertLimiter)
                        {
                            return ReturnFalseAndDequeueIfContinuousStore();
                        }

                        return true;
                    }
                }
                if (StoreLimiterSource == StoreLimiterSource.SOURCEADDRESS || StoreLimiterSource == StoreLimiterSource.BOTH)
                {
                    //We need an MI for the source domain. We pass a normal one around and pull this when needed
                    MemoryInterface sourceMI = MemoryDomains.GetInterface(SourceDomain);
                    if (sourceMI == null)
                    {
                        return false;
                    }

                    if (Filtering.LimiterPeekBytes(SourceAddress,
                        SourceAddress + Precision, Domain, LimiterListHash, sourceMI))
                    {
                        if (InvertLimiter)
                        {
                            return ReturnFalseAndDequeueIfContinuousStore();
                        }

                        return true;
                    }
                }
            }
            else
            {
                if (Filtering.LimiterPeekBytes(Address,
                    Address + Precision, Domain, LimiterListHash, destMI))
                {
                    if (InvertLimiter)
                    {
                        return ReturnFalseAndDequeueIfContinuousStore();
                    }

                    return true;
                }
            }
            //Note the flipped logic here
            if (InvertLimiter)
            {
                return true;
            }

            return ReturnFalseAndDequeueIfContinuousStore();
        }

        public BlastUnit GetBackup()
        {
            //TODO
            //There's a todo here but I didn't leave a note please help someone tell me why there's a todo here oh god I'm the only one working on this code
            return GetBakedUnit();
        }

        /// <summary>
        /// Rerolls a blastunit and generates new values based on various params
        /// </summary>
        public void Reroll()
        {
            //Don't reroll locked units
            if (this.IsLocked)
            {
                return;
            }

            if (Source == BlastUnitSource.VALUE)
            {
                if (RtcCore.RerollFollowsCustomEngine)
                {
                    if (this.GeneratedUsingValueList && !RtcCore.RerollIgnoresOriginalSource)
                    {
                        Value = Filtering.GetRandomConstant(RTC_CustomEngine.ValueListHash, Precision);
                    }
                    else
                    {
                        if (RTC_CustomEngine.ValueSource == CustomValueSource.VALUELIST)
                        {
                            Value = Filtering.GetRandomConstant(RTC_CustomEngine.ValueListHash, Precision);
                            return;
                        }

                        //Generate a random value based on our precision.
                        //We use a BigInteger as we support arbitrary length, but we do use built in methods for 8,16,32 bit for performance reasons
                        BigInteger randomValue = 0;
                        if (RTC_CustomEngine.ValueSource == CustomValueSource.RANGE)
                        {
                            switch (Precision)
                            {
                                case (1):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue8Bit, RTC_CustomEngine.MaxValue8Bit, true);
                                    break;
                                case (2):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue16Bit, RTC_CustomEngine.MaxValue16Bit, true);
                                    break;
                                case (4):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue32Bit, RTC_CustomEngine.MaxValue32Bit, true);
                                    break;
                                case (8):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue64Bit, RTC_CustomEngine.MaxValue64Bit, true);
                                    break;
                                //No limits if out of normal range
                                default:
                                    byte[] _randomValue = new byte[Precision];
                                    RtcCore.RND.NextBytes(_randomValue);
                                    randomValue = new BigInteger(_randomValue);
                                    break;
                            }
                        }
                        else if (RTC_CustomEngine.ValueSource == CustomValueSource.RANDOM)
                        {
                            switch (this.Precision)
                            {
                                case (1):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFF, true);
                                    break;
                                case (2):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFF, true);
                                    break;
                                case (4):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFF, true);
                                    break;
                                case (8):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFFFFFFFFFF, true);
                                    break;
                                //No limits if out of normal range
                                default:
                                    byte[] _randomValue = new byte[Precision];
                                    RtcCore.RND.NextBytes(_randomValue);
                                    randomValue = new BigInteger(_randomValue);
                                    break;
                            }
                        }
                        byte[] temp = new byte[Precision];
                        //We use this as it properly handles the length for us
                        CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref temp, randomValue, false);
                        Value = temp;
                    }
                }
                else
                {
                    if (this.GeneratedUsingValueList && !RtcCore.RerollIgnoresOriginalSource)
                    {
                        Value = Filtering.GetRandomConstant(RTC_VectorEngine.ValueListHash, Precision);
                    }
                    else
                    {
                        //Generate a random value based on our precision.
                        //We use a BigInteger as we support arbitrary length, but we do use built in methods for 8,16,32 bit for performance reasons
                        BigInteger randomValue;
                        switch (Precision)
                        {
                            case (1):
                                randomValue = RtcCore.RND.NextULong(0, 0xFF, true);
                                break;
                            case (2):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFF, true);
                                break;
                            case (4):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFF, true);
                                break;
                            case (8):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFFFFFFFFFF, true);
                                break;
                            //No limits if out of normal range
                            default:
                                byte[] _randomValue = new byte[Precision];
                                RtcCore.RND.NextBytes(_randomValue);
                                randomValue = new BigInteger(_randomValue);
                                break;
                        }

                        byte[] temp = new byte[Precision];
                        //We use this as it properly handles the length for us
                        CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref temp, randomValue, false);
                        Value = temp;
                    }
                }
            }
            else if (Source == BlastUnitSource.STORE)
            {
                string[] _selectedDomains = (string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"];

                //Always reroll domain before address
                if (RtcCore.RerollSourceDomain)
                {
                    SourceDomain = _selectedDomains[RtcCore.RND.Next(_selectedDomains.Length)];
                }
                if (RtcCore.RerollSourceAddress)
                {
                    long maxAddress = MemoryDomains.GetInterface(SourceDomain)?.Size ?? 1;
                    SourceAddress = RtcCore.RND.NextLong(0, maxAddress - 1);
                }

                if (RtcCore.RerollDomain)
                {
                    Domain = _selectedDomains[RtcCore.RND.Next(_selectedDomains.Length)];
                }
                if (RtcCore.RerollAddress)
                {
                    long maxAddress = MemoryDomains.GetInterface(Domain)?.Size ?? 1;
                    Address = RtcCore.RND.NextLong(0, maxAddress - 1);
                }
            }
        }

        public override string ToString()
        {
            string enabledString = "[ ] BlastUnit -> ";
            if (IsEnabled)
            {
                enabledString = "[x] BlastUnit -> ";
            }

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
            return (enabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString("X") + ")." + Source.ToString() + "(" + ValueString + ")");
        }

        /// <summary>
        /// Called when a unit is moved from the queue into the execution pool
        /// </summary>
        /// <returns></returns>
        public bool EnteringExecution()
        {
            //Snag our MI
            MemoryInterface mi = MemoryDomains.GetInterface(Domain);
            if (mi == null)
            {
                return false;
            }

            //If it's a store unit, store the backup
            if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.PREEXECUTE)
            {
                //One off store vs execution pool
                if (StoreType == StoreType.ONCE)
                {
                    StoreBackup();
                }
                else
                {
                    StepActions.StoreDataPool.Add(this);
                }
            }
            //Limiter handling. Normal operation is to not do anything if it doesn't match the limiter. Inverted is to only continue if it doesn't match
            if (LimiterTime == LimiterTime.PREEXECUTE)
            {
                if (!LimiterCheck(mi))
                {
                    return false;
                }
            }

            return true;
        }

        public BlastUnit[] GetBreakdown()
        {
            BlastUnit[] brokenUnits = new BlastUnit[precision];

            if (precision == 1)
            {
                brokenUnits[0] = this;
                return brokenUnits;
            }

            for (int i = 0; i < this.Precision; i++)
            {
                var bu = this.GetSubUnit(i, i + 1);
                brokenUnits[i] = bu;
            }

            return brokenUnits;
        }
    }

    [Serializable]
    public class ActiveTableObject
    {
        public long[] Data { get; set; }

        public ActiveTableObject()
        {
        }

        public ActiveTableObject(long[] data)
        {
            Data = data;
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.AllPublic)]
    public class BlastGeneratorProto : INote
    {
        public string BlastType { get; set; }
        public string Domain { get; set; }
        public int Precision { get; set; }
        public long StepSize { get; set; }
        public long StartAddress { get; set; }
        public long EndAddress { get; set; }
        public ulong Param1 { get; set; }
        public ulong Param2 { get; set; }
        public string Mode { get; set; }
        public string Note { get; set; }
        public int Lifetime { get; set; }
        public int ExecuteFrame { get; set; }
        public bool Loop { get; set; }
        public int Seed { get; set; }
        public BlastLayer bl { get; set; }

        public BlastGeneratorProto()
        {
        }

        public BlastGeneratorProto(string _note, string _blastType, string _domain, string _mode, int _precision, long _stepSize, long _startAddress, long _endAddress, ulong _param1, ulong _param2, int lifetime, int executeframe, bool loop, int _seed)
        {
            Note = _note;
            BlastType = _blastType;
            Domain = _domain;
            Precision = _precision;
            StartAddress = _startAddress;
            EndAddress = _endAddress;
            Param1 = _param1;
            Param2 = _param2;
            Mode = _mode;
            StepSize = _stepSize;
            Lifetime = lifetime;
            ExecuteFrame = executeframe;
            Loop = loop;
            Seed = _seed;
        }

        public BlastLayer GenerateBlastLayer()
        {
            switch (BlastType)
            {
                case "Value":
                    bl = RTC_ValueGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGValueMode)Enum.Parse(typeof(BGValueMode), Mode, true));
                    break;
                case "Store":
                    bl = RTC_StoreGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGStoreMode)Enum.Parse(typeof(BGStoreMode), Mode, true));
                    break;
                default:
                    return null;
            }

            return bl;
        }
    }

    public class ProblematicProcess
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }

    public interface INote
    {
        string Note { get; set; }
    }

    /// <summary>
    /// A generic object for combobox purposes.
    /// Has a name and a value of type T for storing any object.
    /// </summary>
    /// <typeparam name="T">The type of object you want the comboxbox value to be</typeparam>
    public class ComboBoxItem<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public ComboBoxItem(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public ComboBoxItem()
        {
        }
    }

    public delegate void ProgressBarEventHandler(object source, ProgressBarEventArgs e);

    public class ProgressBarEventArgs : EventArgs
    {
        public string CurrentTask;
        public decimal Progress;

        public ProgressBarEventArgs(string text, decimal progress)
        {
            CurrentTask = text;
            Progress = progress;

            RTCV.Common.Logging.GlobalLogger.Log(NLog.LogLevel.Info, $"ProgressBarEventArgs: {text}");
        }
    }
}
