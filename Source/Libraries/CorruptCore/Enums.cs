namespace RTCV.CorruptCore
{
    using System.Diagnostics.CodeAnalysis;

    public enum BlastRadius
    {
        SPREAD,
        CHUNK,
        BURST,
        NORMALIZED,
        PROPORTIONAL,
        EVEN,
        NONE
    }

    public enum BlastUnitSource
    {
        VALUE,
        STORE
    }

    public enum StoreTime
    {
        IMMEDIATE,  //Frame 0 for the blastunit. Right when it's applied. Used for Distortion
        PREEXECUTE, //For when you want it to happen right before the first step
    }

    public enum LimiterTime
    {
        NONE,       //For when something will never happen
        GENERATE,  //Generate
        PREEXECUTE, //For when you want it to happen right before the first step
        EXECUTE     //For when you want it to happen every step
    }

    public enum StoreType
    {
        ONCE,
        CONTINUOUS
    }

    public enum StoreLimiterSource
    {
        ADDRESS,
        SOURCEADDRESS,
        BOTH
    }

    public enum CustomValueSource
    {
        RANDOM,
        VALUELIST,
        RANGE
    }

    public enum CustomStoreAddress
    {
        SAME,
        RANDOM
    }

    public enum NightmareAlgo
    {
        RANDOM,
        RANDOMTILT,
        TILT
    }

    public enum NightmareType
    {
        SET,
        ADD,
        SUBTRACT
    }

    public enum CorruptionEngine
    {
        NIGHTMARE,
        HELLGENIE,
        DISTORTION,
        FREEZE,
        PIPE,
        VECTOR,
        CLUSTER,
        BLASTGENERATORENGINE,
        CUSTOM,
        PLUGIN,
        NONE
    }

    public enum BGValueMode
    {
        Set,
        Add,
        Subtract,
        Random,
        RandomRange,
        ShiftLeft,
        ShiftRight,
        ReplaceXWithY,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXOr,
        BitwiseComplement,
        BitwiseShiftLeft,
        BitwiseShiftRight,
        BitwiseRotateLeft,
        BitwiseRotateRight
    }

    public enum BGStoreMode
    {
        Chained,
        SourceSet,
        SourceRandom,
        DestRandom,
        SELF,
    }

    public enum ProblematicItemType
    {
        PROCESS,
        ASSEMBLY
    }

    public enum StashKeySavestateLocation
    {
        SKS,
        SSK,
        MP,
        SESSION,
        DEFAULTVALUE
    }

    public enum ExecuteState
    {
        EXECUTED,
        NOTEXECUTED,
        ERROR,
        HANDLEDERROR,
        SILENTERROR,
    }

    [SuppressMessage("Microsoft.Design", "CA1707", Justification = "RTCSPEC members may keep their underscores since changing this would break serialization.")]
    public static class RTCSPEC
    {
        public static readonly string RTCDIR = nameof(RTCDIR);
        public static readonly string CORE_SELECTEDENGINE = nameof(CORE_SELECTEDENGINE);
        public static readonly string CORE_SELECTEDPLUGINENGINE = nameof(CORE_SELECTEDPLUGINENGINE);
        public static readonly string CORE_ALLOWCROSSCORECORRUPTION = nameof(CORE_ALLOWCROSSCORECORRUPTION);
        public static readonly string CORE_CURRENTPRECISION = nameof(CORE_CURRENTPRECISION);
        public static readonly string CORE_CURRENTALIGNMENT = nameof(CORE_CURRENTALIGNMENT);
        public static readonly string CORE_USEALIGNMENT = nameof(CORE_USEALIGNMENT);
        public static readonly string CORE_INTENSITY = nameof(CORE_INTENSITY);
        public static readonly string CORE_ERRORDELAY = nameof(CORE_ERRORDELAY);
        public static readonly string CORE_RADIUS = nameof(CORE_RADIUS);
        public static readonly string STEP_CLEARSTEPACTIONSONREWIND = nameof(STEP_CLEARSTEPACTIONSONREWIND);
        public static readonly string STEP_MAXINFINITEBLASTUNITS = nameof(STEP_MAXINFINITEBLASTUNITS);
        public static readonly string STEP_LOCKEXECUTION = nameof(STEP_LOCKEXECUTION);
        public static readonly string STEP_RUNBEFORE = nameof(STEP_RUNBEFORE);
        public static readonly string CORE_EXTRACTBLASTLAYER = nameof(CORE_EXTRACTBLASTLAYER);
        public static readonly string CORE_AUTOCORRUPT = nameof(CORE_AUTOCORRUPT);
        public static readonly string CORE_AUTOUNCORRUPT = nameof(CORE_AUTOUNCORRUPT);
        public static readonly string CORE_KILLSWITCHINTERVAL = nameof(CORE_KILLSWITCHINTERVAL);
        public static readonly string CORE_EMULATOROSDDISABLED = nameof(CORE_EMULATOROSDDISABLED);
        public static readonly string CORE_SHOWCONSOLE = nameof(CORE_SHOWCONSOLE);
        public static readonly string CORE_DONTCLEANSAVESTATESONQUIT = nameof(CORE_DONTCLEANSAVESTATESONQUIT);
        public static readonly string CORE_REROLLADDRESS = nameof(CORE_REROLLADDRESS);
        public static readonly string CORE_REROLLSOURCEADDRESS = nameof(CORE_REROLLSOURCEADDRESS);
        public static readonly string CORE_REROLLDOMAIN = nameof(CORE_REROLLDOMAIN);
        public static readonly string CORE_REROLLSOURCEDOMAIN = nameof(CORE_REROLLSOURCEDOMAIN);
        public static readonly string CORE_REROLLIGNOREORIGINALSOURCE = nameof(CORE_REROLLIGNOREORIGINALSOURCE);
        public static readonly string CORE_REROLLFOLLOWENGINESETTINGS = nameof(CORE_REROLLFOLLOWENGINESETTINGS);
        public static readonly string NIGHTMARE_ALGO = nameof(NIGHTMARE_ALGO);
        public static readonly string NIGHTMARE_MINVALUE8BIT = nameof(NIGHTMARE_MINVALUE8BIT);
        public static readonly string NIGHTMARE_MAXVALUE8BIT = nameof(NIGHTMARE_MAXVALUE8BIT);
        public static readonly string NIGHTMARE_MAXVALUE16BIT = nameof(NIGHTMARE_MAXVALUE16BIT);
        public static readonly string NIGHTMARE_MINVALUE16BIT = nameof(NIGHTMARE_MINVALUE16BIT);
        public static readonly string NIGHTMARE_MAXVALUE32BIT = nameof(NIGHTMARE_MAXVALUE32BIT);
        public static readonly string NIGHTMARE_MINVALUE32BIT = nameof(NIGHTMARE_MINVALUE32BIT);
        public static readonly string NIGHTMARE_MAXVALUE64BIT = nameof(NIGHTMARE_MAXVALUE64BIT);
        public static readonly string NIGHTMARE_MINVALUE64BIT = nameof(NIGHTMARE_MINVALUE64BIT);
        public static readonly string HELLGENIE_MINVALUE8BIT = nameof(HELLGENIE_MINVALUE8BIT);
        public static readonly string HELLGENIE_MAXVALUE8BIT = nameof(HELLGENIE_MAXVALUE8BIT);
        public static readonly string HELLGENIE_MINVALUE16BIT = nameof(HELLGENIE_MINVALUE16BIT);
        public static readonly string HELLGENIE_MAXVALUE16BIT = nameof(HELLGENIE_MAXVALUE16BIT);
        public static readonly string HELLGENIE_MINVALUE32BIT = nameof(HELLGENIE_MINVALUE32BIT);
        public static readonly string HELLGENIE_MAXVALUE32BIT = nameof(HELLGENIE_MAXVALUE32BIT);
        public static readonly string HELLGENIE_MINVALUE64BIT = nameof(HELLGENIE_MINVALUE64BIT);
        public static readonly string HELLGENIE_MAXVALUE64BIT = nameof(HELLGENIE_MAXVALUE64BIT);
        public static readonly string DISTORTION_DELAY = nameof(DISTORTION_DELAY);
        public static readonly string CUSTOM_NAME = nameof(CUSTOM_NAME);
        public static readonly string CUSTOM_PATH = nameof(CUSTOM_PATH);
        public static readonly string CUSTOM_DELAY = nameof(CUSTOM_DELAY);
        public static readonly string CUSTOM_LIFETIME = nameof(CUSTOM_LIFETIME);
        public static readonly string CUSTOM_LIMITERLISTHASH = nameof(CUSTOM_LIMITERLISTHASH);
        public static readonly string CUSTOM_LIMITERTIME = nameof(CUSTOM_LIMITERTIME);
        public static readonly string CUSTOM_LIMITERINVERTED = nameof(CUSTOM_LIMITERINVERTED);
        public static readonly string CUSTOM_LOOP = nameof(CUSTOM_LOOP);
        public static readonly string CUSTOM_MINVALUE8BIT = nameof(CUSTOM_MINVALUE8BIT);
        public static readonly string CUSTOM_MINVALUE16BIT = nameof(CUSTOM_MINVALUE16BIT);
        public static readonly string CUSTOM_MINVALUE32BIT = nameof(CUSTOM_MINVALUE32BIT);
        public static readonly string CUSTOM_MINVALUE64BIT = nameof(CUSTOM_MINVALUE64BIT);
        public static readonly string CUSTOM_MAXVALUE8BIT = nameof(CUSTOM_MAXVALUE8BIT);
        public static readonly string CUSTOM_MAXVALUE16BIT = nameof(CUSTOM_MAXVALUE16BIT);
        public static readonly string CUSTOM_MAXVALUE32BIT = nameof(CUSTOM_MAXVALUE32BIT);
        public static readonly string CUSTOM_MAXVALUE64BIT = nameof(CUSTOM_MAXVALUE64BIT);
        public static readonly string CUSTOM_SOURCE = nameof(CUSTOM_SOURCE);
        public static readonly string CUSTOM_STOREADDRESS = nameof(CUSTOM_STOREADDRESS);
        public static readonly string CUSTOM_STORETIME = nameof(CUSTOM_STORETIME);
        public static readonly string CUSTOM_STORETYPE = nameof(CUSTOM_STORETYPE);
        public static readonly string CUSTOM_STORELIMITERMODE = nameof(CUSTOM_STORELIMITERMODE);
        public static readonly string CUSTOM_TILTVALUE = nameof(CUSTOM_TILTVALUE);
        public static readonly string CUSTOM_VALUELISTHASH = nameof(CUSTOM_VALUELISTHASH);
        public static readonly string CUSTOM_VALUESOURCE = nameof(CUSTOM_VALUESOURCE);
        public static readonly string FILTERING_HASH2LIMITERDICO = nameof(FILTERING_HASH2LIMITERDICO);
        public static readonly string FILTERING_HASH2VALUEDICO = nameof(FILTERING_HASH2VALUEDICO);
        public static readonly string FILTERING_HASH2NAMEDICO = nameof(FILTERING_HASH2NAMEDICO);
        public static readonly string VECTOR_LIMITERLISTHASH = nameof(VECTOR_LIMITERLISTHASH);
        public static readonly string VECTOR_VALUELISTHASH = nameof(VECTOR_VALUELISTHASH);
        public static readonly string VECTOR_UNLOCKPRECISION = nameof(VECTOR_UNLOCKPRECISION);
        public static readonly string CLUSTER_LIMITERLISTHASH = nameof(CLUSTER_LIMITERLISTHASH);
        public static readonly string CLUSTER_SHUFFLETYPE = nameof(CLUSTER_SHUFFLETYPE);
        public static readonly string CLUSTER_SHUFFLEAMT = nameof(CLUSTER_SHUFFLEAMT);
        public static readonly string CLUSTER_MODIFIER = nameof(CLUSTER_MODIFIER);
        public static readonly string CLUSTER_MULTIOUT = nameof(CLUSTER_MULTIOUT);
        public static readonly string CLUSTER_FILTERALL = nameof(CLUSTER_FILTERALL);
        public static readonly string CLUSTER_DIR = nameof(CLUSTER_DIR);
        public static readonly string RENDER_AT_LOAD = nameof(RENDER_AT_LOAD);
        public static readonly string RENDER_ISRENDERING = nameof(RENDER_ISRENDERING);
        public static readonly string RENDER_RENDERTYPE = nameof(RENDER_RENDERTYPE);
        public static readonly string STOCKPILE_CURRENTSAVESTATEKEY = nameof(STOCKPILE_CURRENTSAVESTATEKEY);
        public static readonly string STOCKPILE_BACKUPEDSTATE = nameof(STOCKPILE_BACKUPEDSTATE);
        public static readonly string STOCKPILE_StashAfterOperation = nameof(STOCKPILE_StashAfterOperation);
        public static readonly string STOCKPILE_StashHistory = nameof(STOCKPILE_StashHistory);
        public static readonly string STOCKPILE_SavestateStashkeyDico = nameof(STOCKPILE_SavestateStashkeyDico);
        public static readonly string STOCKPILE_RenderAtLoad = nameof(STOCKPILE_RenderAtLoad);
    }

    [SuppressMessage("Microsoft.Design", "CA1707", Justification = "VSPEC names may have underscores since changing this would break serialization.")]
    public static class VSPEC
    {
        public static readonly string NAME = nameof(NAME);
        public static readonly string EMUDIR = nameof(EMUDIR);
        public static readonly string CORE_LASTLOADERROM = nameof(CORE_LASTLOADERROM);
        public static readonly string STEP_RUNBEFORE = nameof(STEP_RUNBEFORE);
        public static readonly string SYSTEM = nameof(SYSTEM);
        public static readonly string GAMENAME = nameof(GAMENAME);
        public static readonly string SYSTEMPREFIX = nameof(SYSTEMPREFIX);
        public static readonly string SYSTEMCORE = nameof(SYSTEMCORE);
        public static readonly string SYNCSETTINGS = nameof(SYNCSETTINGS);
        public static readonly string SELECTEDDOMAINS = nameof(SELECTEDDOMAINS);
        public static readonly string OPENROMFILENAME = nameof(OPENROMFILENAME);
        public static readonly string SYNCOBJECT = nameof(SYNCOBJECT);
        public static readonly string MEMORYDOMAINS_INTERFACES = nameof(MEMORYDOMAINS_INTERFACES);
        public static readonly string MEMORYDOMAINS_BLACKLISTEDDOMAINS = nameof(MEMORYDOMAINS_BLACKLISTEDDOMAINS);
        public static readonly string CORE_DISKBASED = nameof(CORE_DISKBASED);
        public static readonly string SUPPORTS_RENDERING = nameof(SUPPORTS_RENDERING);
        public static readonly string SUPPORTS_CONFIG_MANAGEMENT = nameof(SUPPORTS_CONFIG_MANAGEMENT);
        public static readonly string SUPPORTS_SAVESTATES = nameof(SUPPORTS_SAVESTATES);
        public static readonly string SUPPORTS_REFERENCES = nameof(SUPPORTS_REFERENCES);
        public static readonly string SUPPORTS_GAMEPROTECTION = nameof(SUPPORTS_GAMEPROTECTION);
        public static readonly string SUPPORTS_REALTIME = nameof(SUPPORTS_REALTIME);
        public static readonly string SUPPORTS_MULTITHREAD = nameof(SUPPORTS_MULTITHREAD);
        public static readonly string SUPPORTS_MIXED_STOCKPILE = nameof(SUPPORTS_MIXED_STOCKPILE);
        public static readonly string SUPPORTS_KILLSWITCH = nameof(SUPPORTS_KILLSWITCH);
        public static readonly string SUPPORTS_CONFIG_HANDOFF = nameof(SUPPORTS_CONFIG_HANDOFF);
        public static readonly string USE_INTEGRATED_HEXEDITOR = nameof(USE_INTEGRATED_HEXEDITOR);
        public static readonly string RENAME_SAVESTATE = nameof(RENAME_SAVESTATE);
        public static readonly string OVERRIDE_DEFAULTMAXINTENSITY = nameof(OVERRIDE_DEFAULTMAXINTENSITY);
        public static readonly string CONFIG_PATHS = nameof(CONFIG_PATHS);
        public static readonly string REPLACE_MANUALBLAST_WITH_GHCORRUPT = nameof(REPLACE_MANUALBLAST_WITH_GHCORRUPT);
        public static readonly string LOADSTATE_USES_CALLBACKS = nameof(LOADSTATE_USES_CALLBACKS);
    }

    [SuppressMessage("Microsoft.Design", "CA1707", Justification = "UISPEC names may have underscores since changing this would break serialization.")]
    public static class UISPEC
    {
        public static readonly string SELECTEDDOMAINS = nameof(SELECTEDDOMAINS);
        public static readonly string SELECTEDDOMAINS_FORCAVESEARCH = nameof(SELECTEDDOMAINS_FORCAVESEARCH);
    }
}
