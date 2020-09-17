namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

    [Serializable]
    [MemberConfig(TargetMember.AllPublic)]
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

        public BlastGeneratorProto(string note, string blastType, string domain, string mode, int precision, long stepSize, long startAddress, long endAddress, ulong param1, ulong param2, int lifetime, int executeframe, bool loop, int seed)
        {
            Note = note;
            BlastType = blastType;
            Domain = domain;
            Precision = precision;
            StartAddress = startAddress;
            EndAddress = endAddress;
            Param1 = param1;
            Param2 = param2;
            Mode = mode;
            StepSize = stepSize;
            Lifetime = lifetime;
            ExecuteFrame = executeframe;
            Loop = loop;
            Seed = seed;
        }

        public BlastLayer GenerateBlastLayer()
        {
            switch (BlastType)
            {
                case "Value":
                    bl = ValueGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGValueMode)Enum.Parse(typeof(BGValueMode), Mode, true));
                    break;
                case "Store":
                    bl = StoreGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGStoreMode)Enum.Parse(typeof(BGStoreMode), Mode, true));
                    break;
                default:
                    return null;
            }

            return bl;
        }
    }
}
