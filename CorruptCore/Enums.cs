	using System;
	using System.Collections.Generic;

	namespace RTCV.CorruptCore
{
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
		BLASTGENERATORENGINE,
		CUSTOM,
		NONE
	}
	public enum BGValueModes
	{
		SET,
		ADD,
		SUBTRACT,
		RANDOM,
		RANDOM_RANGE,
		SHIFT_LEFT,
		SHIFT_RIGHT,
		REPLACE_X_WITH_Y,
		BITWISE_AND,
		BITWISE_OR,
		BITWISE_XOR,
		BITWISE_COMPLEMENT,
		BITWISE_SHIFT_LEFT,
		BITWISE_SHIFT_RIGHT,
		BITWISE_ROTATE_LEFT,
		BITWISE_ROTATE_RIGHT
	}

	public enum BGStoreModes
	{
		CHAINED,
		SOURCE_SET,
		SOURCE_RANDOM,
		DEST_RANDOM,
		FREEZE,
	}

	public enum ProblematicItemTypes
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

	public static class RTCSPEC
	{
		public static readonly string CORE_SELECTEDENGINE = "CORE_SELECTEDENGINE";
		public static readonly string CORE_ALLOWCROSSCORECORRUPTION = "CORE_ALLOWCROSSCORECORRUPTION";
		public static readonly string CORE_CURRENTPRECISION = "CORE_CURRENTPRECISION";
		public static readonly string CORE_INTENSITY = "CORE_INTENSITY";
		public static readonly string CORE_ERRORDELAY = "CORE_ERRORDELAY";
		public static readonly string CORE_RADIUS = "CORE_RADIUS";
		public static readonly string STEP_CLEARSTEPACTIONSONREWIND = "STEP_CLEARSTEPACTIONSONREWIND";
		public static readonly string STEP_MAXINFINITEBLASTUNITS = "STEP_MAXINFINITEBLASTUNITS";
		public static readonly string STEP_LOCKEXECUTION = "STEP_LOCKEXECUTION";
		public static readonly string STEP_RUNBEFORE = "STEP_RUNBEFORE";
		public static readonly string CORE_EXTRACTBLASTLAYER = "CORE_EXTRACTBLASTLAYER";
		public static readonly string CORE_AUTOCORRUPT = "CORE_AUTOCORRUPT";
		public static readonly string CORE_KILLSWITCHINTERVAL = "CORE_KILLSWITCHINTERVAL";
		public static readonly string CORE_BIZHAWKOSDDISABLED = "CORE_BIZHAWKOSDDISABLED";
		public static readonly string CORE_SHOWCONSOLE = "CORE_SHOWCONSOLE";
		public static readonly string CORE_DONTCLEANSAVESTATESONQUIT = "CORE_DONTCLEANSAVESTATESONQUIT";
		public static readonly string CORE_REROLLADDRESS = "CORE_REROLLADDRESS";
		public static readonly string CORE_REROLLSOURCEADDRESS = "CORE_REROLLSOURCEADDRESS";
		public static readonly string CORE_REROLLDOMAIN = "CORE_REROLLDOMAIN";
		public static readonly string CORE_REROLLSOURCEDOMAIN = "CORE_REROLLSOURCEDOMAIN";
		public static readonly string CORE_REROLLIGNOREORIGINALSOURCE = "CORE_REROLLIGNOREORIGINALSOURCE";
		public static readonly string CORE_REROLLFOLLOWENGINESETTINGS = "CORE_REROLLFOLLOWENGINESETTINGS";
		public static readonly string NIGHTMARE_ALGO = "NIGHTMARE_ALGO";
		public static readonly string NIGHTMARE_MINVALUE8BIT = "NIGHTMARE_MINVALUE8BIT";
		public static readonly string NIGHTMARE_MAXVALUE8BIT = "NIGHTMARE_MAXVALUE8BIT";
		public static readonly string NIGHTMARE_MAXVALUE16BIT = "NIGHTMARE_MAXVALUE16BIT";
		public static readonly string NIGHTMARE_MINVALUE16BIT = "NIGHTMARE_MINVALUE16BIT";
		public static readonly string NIGHTMARE_MAXVALUE32BIT = "NIGHTMARE_MAXVALUE32BIT";
		public static readonly string NIGHTMARE_MINVALUE32BIT = "NIGHTMARE_MINVALUE32BIT";
		public static readonly string HELLGENIE_MINVALUE8BIT = "HELLGENIE_MINVALUE8BIT";
		public static readonly string HELLGENIE_MAXVALUE8BIT = "HELLGENIE_MAXVALUE8BIT";
		public static readonly string HELLGENIE_MINVALUE16BIT = "HELLGENIE_MINVALUE16BIT";
		public static readonly string HELLGENIE_MAXVALUE16BIT = "HELLGENIE_MAXVALUE16BIT";
		public static readonly string HELLGENIE_MINVALUE32BIT = "HELLGENIE_MINVALUE32BIT";
		public static readonly string HELLGENIE_MAXVALUE32BIT = "HELLGENIE_MAXVALUE32BIT";
		public static readonly string DISTORTION_DELAY = "DISTORTION_DELAY";
		public static readonly string CUSTOM_NAME = "CUSTOM_NAME";
		public static readonly string CUSTOM_PATH = "CUSTOM_PATH";
		public static readonly string CUSTOM_DELAY = "CUSTOM_DELAY";
		public static readonly string CUSTOM_LIFETIME = "CUSTOM_LIFETIME";
		public static readonly string CUSTOM_LIMITERLISTHASH = "CUSTOM_LIMITERLISTHASH";
		public static readonly string CUSTOM_LIMITERTIME = "CUSTOM_LIMITERTIME";
		public static readonly string CUSTOM_LIMITERINVERTED = "CUSTOM_LIMITERINVERTED";
		public static readonly string CUSTOM_LOOP = "CUSTOM_LOOP";
		public static readonly string CUSTOM_MINVALUE8BIT = "CUSTOM_MINVALUE8BIT";
		public static readonly string CUSTOM_MINVALUE16BIT = "CUSTOM_MINVALUE16BIT";
		public static readonly string CUSTOM_MINVALUE32BIT = "CUSTOM_MINVALUE32BIT";
		public static readonly string CUSTOM_MAXVALUE8BIT = "CUSTOM_MAXVALUE8BIT";
		public static readonly string CUSTOM_MAXVALUE16BIT = "CUSTOM_MAXVALUE16BIT";
		public static readonly string CUSTOM_MAXVALUE32BIT = "CUSTOM_MAXVALUE32BIT";
		public static readonly string CUSTOM_SOURCE = "CUSTOM_SOURCE";
		public static readonly string CUSTOM_STOREADDRESS = "CUSTOM_STOREADDRESS";
		public static readonly string CUSTOM_STORETIME = "CUSTOM_STORETIME";
		public static readonly string CUSTOM_STORETYPE = "CUSTOM_STORETYPE";
		public static readonly string CUSTOM_STORELIMITERMODE = "CUSTOM_STORELIMITERMODE";
		public static readonly string CUSTOM_TILTVALUE = "CUSTOM_TILTVALUE";
		public static readonly string CUSTOM_VALUELISTHASH = "CUSTOM_VALUELISTHASH";
		public static readonly string CUSTOM_VALUESOURCE = "CUSTOM_VALUESOURCE";
		public static readonly string FILTERING_HASH2LIMITERDICO = "FILTERING_HASH2LIMITERDICO";
		public static readonly string FILTERING_HASH2VALUEDICO = "FILTERING_HASH2VALUEDICO";
		public static readonly string FILTERING_HASH2NAMEDICO = "FILTERING_HASH2NAMEDICO";
		public static readonly string VECTOR_LIMITERLISTHASH = "VECTOR_LIMITERLISTHASH";
		public static readonly string VECTOR_VALUELISTHASH = "VECTOR_VALUELISTHASH";
		public static readonly string RENDER_AT_LOAD = "RENDER_AT_LOAD";
		public static readonly string RENDER_ISRENDERING = "RENDER_ISRENDERING";
		public static readonly string RENDER_RENDERTYPE = "RENDER_RENDERTYPE";

		public static readonly string STOCKPILE_CURRENTSAVESTATEKEY = "STOCKPILE_CURRENTSAVESTATEKEY";
		public static readonly string STOCKPILE_BACKUPEDSTATE = "STOCKPILE_BACKUPEDSTATE";
		public static readonly string STOCKPILE_StashAfterOperation = "STOCKPILE_StashAfterOperation";
		public static readonly string STOCKPILE_StashHistory = "STOCKPILE_StashHistory";
		public static readonly string STOCKPILE_SavestateStashkeyDico = "STOCKPILE_SavestateStashkeyDico";
		public static readonly string STOCKPILE_RenderAtLoad = "STOCKPILE_RenderAtLoad";
	}

	public static class VSPEC
	{
		public static readonly string CORE_LASTLOADERROM = "CORE_LASTLOADERROM";
		public static readonly string STEP_RUNBEFORE = "STEP_RUNBEFORE";
		public static readonly string SYSTEM = "SYSTEM";
		public static readonly string GAMENAME = "GAMENAME";
		public static readonly string SYSTEMPREFIX = "SYSTEMPREFIX";
		public static readonly string SYSTEMCORE = "SYSTEMCORE";
		public static readonly string SYNCSETTINGS = "SYNCSETTINGS";
		public static readonly string OPENROMFILENAME = "OPENROMFILENAME";
		public static readonly string SYNCOBJECT = "SYNCOBJECT";
		public static readonly string MEMORYDOMAINS_INTERFACES = "MEMORYDOMAINS_INTERFACES";
		public static readonly string MEMORYDOMAINS_BLACKLISTEDDOMAINS = "MEMORYDOMAINS_BLACKLISTEDDOMAINS";
		public static readonly string CORE_DISKBASED = "CORE_DISKBASED";
		
	}

	public enum UISPEC
	{
	}

}
