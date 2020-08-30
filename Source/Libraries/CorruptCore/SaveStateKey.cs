namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class SaveStateKey
    {
        public StashKey StashKey { get; set; }
        public string Text { get; set; }

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
        public List<StashKey> StashKeys { get; set; }
        public List<string> Text { get; set; }

        public SaveStateKeys()
        {
            StashKeys = new List<StashKey>();
            Text = new List<string>();
            VanguardImplementation = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
        }
    }
}
