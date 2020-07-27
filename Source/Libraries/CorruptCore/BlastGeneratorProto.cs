namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

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
                    bl = RTC_StoreGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGStoreMode)Enum.Parse(typeof(BGStoreMode), Mode, true));
                    break;
                default:
                    return null;
            }

            return bl;
        }
    }
}
