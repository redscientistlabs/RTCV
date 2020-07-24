namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;

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
