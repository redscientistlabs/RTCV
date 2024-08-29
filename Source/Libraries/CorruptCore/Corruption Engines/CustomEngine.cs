namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Windows.Forms;
    using RTCV.Common.CustomExtensions;
    using RTCV.NetCore;
    using RTCV.CorruptCore.Extensions;

    public static class CustomEngine
    {
        private static Dictionary<string, Type> name2TypeDico = new Dictionary<string, Type>();
        public static readonly Dictionary<string, PartialSpec> Name2TemplateDico = new Dictionary<string, PartialSpec>();

        public static ulong MinValue8Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE8BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE8BIT, value);
        }

        public static ulong MaxValue8Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE8BIT, value);
        }

        public static ulong MinValue16Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE16BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE16BIT, value);
        }

        public static ulong MaxValue16Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE16BIT, value);
        }

        public static ulong MinValue32Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE32BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE32BIT, value);
        }

        public static ulong MaxValue32Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE32BIT, value);
        }

        public static ulong MinValue64Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE64BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE64BIT, value);
        }

        public static ulong MaxValue64Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE64BIT, value);
        }

        public static BlastUnitSource Source
        {
            get => (BlastUnitSource)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_SOURCE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_SOURCE, value);
        }

        public static StoreType StoreType
        {
            get => (StoreType)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETYPE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORETYPE, value);
        }

        public static StoreTime StoreTime
        {
            get => (StoreTime)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETIME];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORETIME, value);
        }

        public static CustomStoreAddress StoreAddress
        {
            get => (CustomStoreAddress)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STOREADDRESS];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STOREADDRESS, value);
        }

        public static int Delay
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_DELAY];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_DELAY, value);
        }

        public static int Lifetime
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIFETIME];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIFETIME, value);
        }

        public static BigInteger TiltValue
        {
            get => (BigInteger)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_TILTVALUE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_TILTVALUE, value);
        }

        public static LimiterTime LimiterTime
        {
            get => (LimiterTime)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERTIME];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME, value);
        }

        public static bool LimiterInverted
        {
            get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERINVERTED];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERINVERTED, value);
        }

        public static StoreLimiterSource StoreLimiterSource
        {
            get => (StoreLimiterSource)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORELIMITERMODE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORELIMITERMODE, value);
        }

        public static bool Loop
        {
            get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LOOP];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LOOP, value);
        }

        public static CustomValueSource ValueSource
        {
            get => (CustomValueSource)AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUESOURCE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE, value);
        }

        public static string LimiterListHash
        {
            //Intentionally nullable cast
            get => AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH] as string ?? "";
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERLISTHASH, value);
        }

        public static string ValueListHash
        {
            //Intentionally nullable cast
            get => AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUELISTHASH] as string ?? "";
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_VALUELISTHASH, value);
        }

        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment, bool useAlignment)
        {
            try
            {
                if (domain == null)
                {
                    return null;
                }

                MemoryInterface mi = MemoryDomains.GetInterface(domain);
                if (mi == null)
                {
                    return null;
                }

                byte[] value = new byte[precision];
                long safeAddress = address;
                if (useAlignment)
                    safeAddress = safeAddress - (address % precision) + alignment;
                if (safeAddress > mi.Size - precision && mi.Size > precision)
                {
                    safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
                }

                BlastUnit bu = new BlastUnit();

                switch (Source)
                {
                    case BlastUnitSource.VALUE:
                        {
                            switch (ValueSource)
                            {
                                case CustomValueSource.VALUELIST:
                                    {
                                        value = Filtering.GetRandomConstant(ValueListHash, precision, mi.PeekBytes(safeAddress, safeAddress + precision, !mi.BigEndian));
                                        //value = Filtering.GetRandomConstant(ValueListHash, precision);
                                    }
                                    break;

                                case CustomValueSource.RANGE:
                                    {
                                        ulong randomValue = 0;
                                        bool def = false;
                                        switch (precision)
                                        {
                                            case 1:
                                                randomValue = RtcCore.RND.NextULong(MinValue8Bit, MaxValue8Bit, true);
                                                break;
                                            case 2:
                                                randomValue = RtcCore.RND.NextULong(MinValue16Bit, MaxValue16Bit, true);
                                                break;
                                            case 4:
                                                randomValue = RtcCore.RND.NextULong(MinValue32Bit, MaxValue32Bit, true);
                                                break;
                                            case 8:
                                                randomValue = RtcCore.RND.NextULong(MinValue64Bit, MaxValue64Bit, true);
                                                break;
                                            default:
                                                def = true;
                                                break;
                                        }

                                        if (def)
                                        {
                                            for (int i = 0; i < precision; i++)
                                            {
                                                value[i] = (byte)RtcCore.RND.Next();
                                            }
                                        }
                                        else
                                        {
                                            value = ByteArrayExtensions.GetByteArrayValue(precision, randomValue, true);
                                        }
                                    }
                                    break;

                                case CustomValueSource.RANDOM:
                                    {
                                        for (int i = 0; i < precision; i++)
                                        {
                                            value[i] = (byte)RtcCore.RND.Next();
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case BlastUnitSource.STORE:
                        {
                            bu.StoreType = StoreType;
                            bu.StoreTime = StoreTime;

                            switch (StoreAddress)
                            {
                                case CustomStoreAddress.RANDOM:
                                    {
                                        BlastTarget bt = RtcCore.GetBlastTarget();
                                        MemoryInterface _mi = MemoryDomains.GetInterface(bt.Domain);
                                        
                                        long safeStartAddress = bt.Address;
                                        if (useAlignment)
                                            safeStartAddress = safeStartAddress - (bt.Address % precision) + alignment;

                                        if (safeStartAddress > _mi.Size - precision)
                                        {
                                            safeStartAddress = _mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
                                        }

                                        bu.SourceDomain = bt.Domain;
                                        bu.SourceAddress = safeStartAddress;
                                    }
                                    break;
                                case CustomStoreAddress.SAME:
                                    {
                                        bu.SourceDomain = domain;
                                        bu.SourceAddress = safeAddress;
                                    }
                                    break;
                            }
                        }
                        break;
                }
                //Precision has to be before Value
                bu.Precision = precision;
                bu.Value = value;

                bu.Address = safeAddress;
                bu.Domain = domain;
                bu.Source = Source;
                bu.ExecuteFrame = Delay;
                bu.Lifetime = Lifetime;
                bu.LimiterTime = LimiterTime;
                bu.Loop = Loop;
                bu.InvertLimiter = LimiterInverted;
                bu.TiltValue = TiltValue;
                bu.StoreLimiterSource = StoreLimiterSource;
                bu.GeneratedUsingValueList = (Source == BlastUnitSource.VALUE && ValueSource == CustomValueSource.VALUELIST);
                bu.BigEndian = mi.BigEndian;

                //Only set a list if it's used
                if (LimiterTime != LimiterTime.NONE)
                {
                    bu.LimiterListHash = LimiterListHash;
                }

                //Limiter handling
                if (LimiterTime == LimiterTime.GENERATE)
                {
                    if (!bu.LimiterCheck(mi))
                    {
                        return null;
                    }
                }

                return bu;
            }
            catch (Exception ex)
            {
                throw new Exception("Custom Engine GenerateUnit Threw Up\n" + ex);
            }
        }

        public static bool IsConstant(byte[] bytes, string[] list, bool bigEndian)
        {
            if (list == null || bytes == null)
            {
                return true;
            }

            if (!bigEndian)
            {
                return list.Contains(ByteArrayToString(bytes));
            }

            Array.Reverse(bytes);
            return list.Contains(ByteArrayToString(bytes));
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static byte[] GetRandomConstant(string[] list)
        {
            if (list == null)
            {
                byte[] buffer = new byte[4];
                RtcCore.RND.NextBytes(buffer);
                return buffer;
            }

            return StringToByteArray(list[RtcCore.RND.Next(list.Length)]);
        }

        internal static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        internal static void getDefaultPartial(PartialSpec pSpec)
        {
            pSpec = InitializeNightmareEngineTemplate(pSpec.Name);
            pSpec[RTCSPEC.CUSTOM_PATH] = "";
            foreach (var k in pSpec.GetKeys())
            {
                name2TypeDico[k] = pSpec[k].GetType();
            }
        }

        public static void InitTemplates()
        {
            var nightmare = InitializeNightmareEngineTemplate();
            var hellgenie = InitializeHellgenieEngineTemplate();
            var freeze = InitializeFreezeEngineTemplate();
            var distortion = InitializeDistortionEngineTemplate();
            var pipe = InitializePipeEngineTemplate();
            var vector = InitializeVectorEngineTemplate();

            Name2TemplateDico[nightmare[RTCSPEC.CUSTOM_NAME].ToString()] = nightmare;
            Name2TemplateDico[hellgenie[RTCSPEC.CUSTOM_NAME].ToString()] = hellgenie;
            Name2TemplateDico[freeze[RTCSPEC.CUSTOM_NAME].ToString()] = freeze;
            Name2TemplateDico[distortion[RTCSPEC.CUSTOM_NAME].ToString()] = distortion;
            Name2TemplateDico[pipe[RTCSPEC.CUSTOM_NAME].ToString()] = pipe;
            Name2TemplateDico[vector[RTCSPEC.CUSTOM_NAME].ToString()] = vector;

            LoadUserTemplates();
        }

        public static void LoadUserTemplates()
        {
            if (!Directory.Exists(RtcCore.EngineTemplateDir))
            {
                Directory.CreateDirectory(RtcCore.EngineTemplateDir);
            }

            string[] paths = Directory.GetFiles(RtcCore.EngineTemplateDir);
            paths = paths.OrderBy(x => x).ToArray();
            foreach (var p in paths)
            {
                PartialSpec _p = LoadTemplateFile(p);
                Name2TemplateDico[_p[RTCSPEC.CUSTOM_NAME].ToString()] = _p;
            }
        }

        //Don't set a Limiter or Value list hash in any of these. We just leave it on whatever is currently set and set that it shouldn't be used.

        //This is because we need to be able to have the UI select some item (the comboboxes don't have an "empty" state)
        public static PartialSpec InitializeNightmareEngineTemplate(string name = null)
        {
            PartialSpec pSpec;
            if (name == null)
            {
                pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);
            }
            else
            {
                pSpec = new PartialSpec(name);
            }

            pSpec[RTCSPEC.CUSTOM_NAME] = "Nightmare Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 1;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = 0;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = true;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 0;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 1;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.NONE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.RANDOM;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.VALUE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.RANDOM;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.IMMEDIATE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.ONCE;
            return pSpec;
        }

        public static PartialSpec InitializeHellgenieEngineTemplate()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);
            pSpec[RTCSPEC.CUSTOM_NAME] = "Hellgenie Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 1;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = 0;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = true;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 0;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 0;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.NONE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.RANDOM;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.VALUE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.RANDOM;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.IMMEDIATE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.ONCE;
            return pSpec;
        }

        public static PartialSpec InitializeDistortionEngineTemplate()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);

            pSpec[RTCSPEC.CUSTOM_NAME] = "Distortion Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 1;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = 0;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = true;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 50;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 1;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.NONE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.RANDOM;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.STORE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.SAME;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.IMMEDIATE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.ONCE;
            return pSpec;
        }

        public static PartialSpec InitializeFreezeEngineTemplate()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);

            pSpec[RTCSPEC.CUSTOM_NAME] = "Freeze Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 1;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 0;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 0;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.NONE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.RANDOM;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.STORE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.SAME;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.PREEXECUTE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.ONCE;
            return pSpec;
        }

        public static PartialSpec InitializePipeEngineTemplate()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);
            pSpec[RTCSPEC.CUSTOM_NAME] = "Pipe Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 1;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = 0;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = true;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 0;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 0;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.NONE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.RANDOM;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.STORE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.RANDOM;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.PREEXECUTE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.CONTINUOUS;
            return pSpec;
        }

        public static PartialSpec InitializeVectorEngineTemplate()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);

            pSpec[RTCSPEC.CUSTOM_NAME] = "Vector Engine";

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = 4;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = 0;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = true;

            pSpec[RTCSPEC.CUSTOM_DELAY] = 0;
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = 1;
            pSpec[RTCSPEC.CUSTOM_LOOP] = false;

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = new BigInteger(0);

            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = LimiterTime.GENERATE;
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = StoreLimiterSource.ADDRESS;
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = false;

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = 0UL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = 0xFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = 0xFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = 0xFFFFFFFFUL;
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = CustomValueSource.VALUELIST;

            pSpec[RTCSPEC.CUSTOM_SOURCE] = BlastUnitSource.VALUE;

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = CustomStoreAddress.RANDOM;
            pSpec[RTCSPEC.CUSTOM_STORETIME] = StoreTime.IMMEDIATE;
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = StoreType.ONCE;
            return pSpec;
        }

        public static PartialSpec getCurrentConfigSpec()
        {
            PartialSpec pSpec = new PartialSpec(AllSpec.CorruptCoreSpec.name);

            pSpec[RTCSPEC.CUSTOM_NAME] = pSpec[RTCSPEC.CUSTOM_NAME];

            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = AllSpec.CorruptCoreSpec[RTCSPEC.CORE_CURRENTPRECISION];
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = AllSpec.CorruptCoreSpec[RTCSPEC.CORE_CURRENTALIGNMENT];
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = AllSpec.CorruptCoreSpec[RTCSPEC.CORE_USEALIGNMENT];

            pSpec[RTCSPEC.CUSTOM_DELAY] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_DELAY];
            pSpec[RTCSPEC.CUSTOM_LIFETIME] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIFETIME];
            pSpec[RTCSPEC.CUSTOM_LOOP] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LOOP];

            pSpec[RTCSPEC.CUSTOM_TILTVALUE] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_TILTVALUE];

            pSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH];
            pSpec[RTCSPEC.CUSTOM_LIMITERTIME] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERTIME];
            pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERINVERTED];

            pSpec[RTCSPEC.CUSTOM_VALUELISTHASH] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUELISTHASH];

            pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE8BIT];
            pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE16BIT];
            pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE32BIT];
            pSpec[RTCSPEC.CUSTOM_MINVALUE64BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE64BIT];
            pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT];
            pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT];
            pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT];
            pSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE64BIT];

            pSpec[RTCSPEC.CUSTOM_VALUESOURCE] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUESOURCE];

            pSpec[RTCSPEC.CUSTOM_SOURCE] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_SOURCE];

            pSpec[RTCSPEC.CUSTOM_STOREADDRESS] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STOREADDRESS];
            pSpec[RTCSPEC.CUSTOM_STORETIME] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETIME];
            pSpec[RTCSPEC.CUSTOM_STORETYPE] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETYPE];
            pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE] = AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORELIMITERMODE];

            return pSpec;
        }

        public static string CustomPath
        {
            get => AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_PATH].ToString();
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_PATH, value);
        }

        private static NetCore.SafeJsonTypeSerialization.JsonKnownTypesBinder InitSpecKnownTypes()
        {
            var t = new NetCore.SafeJsonTypeSerialization.JsonKnownTypesBinder();
            t.KnownTypes.Add(typeof(long));
            t.KnownTypes.Add(typeof(string));
            return t;
        }

        public static bool LoadTemplate(string template)
        {
            PartialSpec spec = Name2TemplateDico[template];
            if (spec == null)
            {
                return false;
            }

            AllSpec.CorruptCoreSpec.Update(spec);
            return true;
        }

        public static PartialSpec LoadTemplateFile(string Filename = null)
        {
            if (Filename == null)
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    DefaultExt = "cet",
                    Title = "Open Engine Template File",
                    Filter = "CET files|*.cet",
                    RestoreDirectory = true
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Filename = ofd.FileName;
                }
                else
                {
                    return null;
                }
            }

            PartialSpec pSpec = new PartialSpec("RTCSpec");
            try
            {
                using (FileStream fs = File.Open(Filename, FileMode.OpenOrCreate))
                {
                    Dictionary<string, object> d = JsonHelper.Deserialize<Dictionary<string, object>>(fs);

                    //We don't want to store type data in the serialized data but specs store object
                    //To work around this, we store the type in a dictionary and pass the data through a typeconverter
                    foreach (var k in d.Keys)
                    {
                        var t = d[k];
                        //If the type doesn't exist, just use what it's parsed as
                        if (!name2TypeDico.ContainsKey(k))
                        {
                            pSpec[k] = t;
                            continue;
                        }

                        var type = name2TypeDico[k];
                        if (t.GetType() != type)
                        {
                            //There's no typeconverter for bigint so we have to handle it manually. Convert it to a string then bigint it
                            if (type == typeof(BigInteger))
                            {
                                if (BigInteger.TryParse(t.ToString(), out BigInteger a))
                                {
                                    t = a;
                                }
                                else
                                {
                                    throw new Exception("Couldn't convert " + t.ToString() +
                                                        " to BigInteger! Something is wrong with your template.");
                                }
                            }
                            //ULong64 gets deserialized to bigint for some reason?????
                            else if (t is BigInteger _t && _t <= ulong.MaxValue)
                            {
                                t = (ulong)(_t & ulong.MaxValue);
                            }
                            //handle the enums
                            else if (type.BaseType == typeof(Enum))
                            {
                                //We can't use tryparse here so we have to catch the exception
                                try
                                {
                                    t = Enum.Parse(type, t.ToString());
                                }
                                catch (ArgumentException e)
                                {
                                    throw new Exception("Couldn't convert " + t.ToString() +
                                        " to " + type.Name + "! Something is wrong with your template.", e);
                                }
                            }
                            else
                            {
                                t = TypeDescriptor.GetConverter(t).ConvertTo(t, type);
                            }
                        }
                        pSpec[k] = t;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The Template file could not be loaded" + e);
                return null;
            }

            //Overwrites spec path with loaded path
            pSpec[RTCSPEC.CUSTOM_PATH] = Filename;

            return pSpec;
        }

        public static string SaveTemplateFile(bool SaveAs = false)
        {
            PartialSpec pSpec = getCurrentConfigSpec();

            string path;
            string templateName;

            if (SaveAs || CustomPath == null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    DefaultExt = "cet",
                    Title = "Save Engine Template File",
                    Filter = "CET files|*.cet",
                    RestoreDirectory = true,
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = saveFileDialog1.FileName;
                    templateName = Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                path = CustomPath;
                templateName = Path.GetFileNameWithoutExtension(path);
            }

            pSpec[RTCSPEC.CUSTOM_NAME] = templateName;
            pSpec[RTCSPEC.CUSTOM_PATH] = path;
            pSpec[RTCSPEC.CORE_CURRENTPRECISION] = RtcCore.CurrentPrecision;
            pSpec[RTCSPEC.CORE_CURRENTALIGNMENT] = RtcCore.Alignment;
            pSpec[RTCSPEC.CORE_USEALIGNMENT] = RtcCore.UseAlignment;

            string jsonString = pSpec.GetSerializedDico();
            File.WriteAllText(path, jsonString);
            Name2TemplateDico[templateName] = pSpec;
            return templateName;
        }
    }
}
