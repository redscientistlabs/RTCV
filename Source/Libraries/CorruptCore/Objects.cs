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
