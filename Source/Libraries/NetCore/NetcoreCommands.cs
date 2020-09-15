namespace RTCV.NetCore.Commands
{
    public static class Basic
    {
        public const string CorruptCore = nameof(Basic) + "_" + nameof(CorruptCore);
        public const string Vanguard = nameof(Basic) + "_" + nameof(Vanguard);
        public const string Default = nameof(Basic) + "_" + nameof(Default);
        public const string UI = nameof(Basic) + "_" + nameof(UI);
        public const string KillswitchPulse = nameof(Basic) + "_" + nameof(KillswitchPulse);
        public const string ResetgameProtectionIfRunning = nameof(Basic) + "_" + nameof(ResetgameProtectionIfRunning);
        public const string MANUALBLAST = nameof(Basic) + "_" + nameof(MANUALBLAST);
        public const string APPLYBLASTLAYER = nameof(Basic) + "_" + nameof(APPLYBLASTLAYER);
        public const string APPLYCACHEDBLASTLAYER = nameof(Basic) + "_" + nameof(APPLYCACHEDBLASTLAYER);
        public const string BLAST = nameof(Basic) + "_" + nameof(BLAST);
        public const string STASHKEY = nameof(Basic) + "_" + nameof(STASHKEY);
        public const string ErrorDiableAutoCorrupt = nameof(Basic) + "_" + nameof(ErrorDiableAutoCorrupt);
        public const string GENERATEBLASTLAYER = nameof(Basic) + "_" + nameof(GENERATEBLASTLAYER);
        public const string SAVESAVESTATE = nameof(Basic) + "_" + nameof(SAVESAVESTATE);
        public const string LOADSAVESTATE = nameof(Basic) + "_" + nameof(LOADSAVESTATE);
        public const string RTC_INFOCUS = nameof(Basic) + "_" + nameof(RTC_INFOCUS);
        public const string BLASTGENERATOR_BLAST = nameof(Basic) + "_" + nameof(BLASTGENERATOR_BLAST);
    }

    public static class Remote {
        public const string PushVanguardSpec = nameof(Remote) + "_" + nameof(PushVanguardSpec);
        public const string PushVanguardSpecUpdate = nameof(Remote) + "_" + nameof(PushVanguardSpecUpdate);
        public const string PushCorruptCoreSpec = nameof(Remote) + "_" + nameof(PushCorruptCoreSpec);
        public const string RemotePushCorruptCoreSpecUpdate = nameof(Remote) + "_" + nameof(RemotePushCorruptCoreSpecUpdate);
        public const string RemotePushUISpec = nameof(Remote) + "_" + nameof(RemotePushUISpec);
        public const string REMOTE_PUSHUISPECUPDATE = nameof(Remote) + "_" + nameof(REMOTE_PUSHUISPECUPDATE);
        public const string REMOTE_ALLSPECSSENT = nameof(Remote) + "_" + nameof(REMOTE_ALLSPECSSENT);
        public const string REMOTE_RENDER_STOP = nameof(Remote) + "_" + nameof(REMOTE_RENDER_STOP);
        public const string REMOTE_RENDER_START = nameof(Remote) + "_" + nameof(REMOTE_RENDER_START);
        public const string REMOTE_RENDER_DISPLAY = nameof(Remote) + "_" + nameof(REMOTE_RENDER_DISPLAY);
        public const string REMOTE_EVENT_DOMAINSUPDATED = nameof(Remote) + "_" + nameof(REMOTE_EVENT_DOMAINSUPDATED);
        public const string REMOTE_EVENT_RESTRICTFEATURES = nameof(Remote) + "_" + nameof(REMOTE_EVENT_RESTRICTFEATURES);
        public const string REMOTE_GETBLASTGENERATOR_LAYER = nameof(Remote) + "_" + nameof(REMOTE_GETBLASTGENERATOR_LAYER);
        public const string REMOTE_PUSHRTCSPEC = nameof(Remote) + "_" + nameof(REMOTE_PUSHRTCSPEC);
        public const string REMOTE_PUSHRTCSPECUPDATE = nameof(Remote) + "_" + nameof(REMOTE_PUSHRTCSPECUPDATE);
        public const string REMOTE_PUSHVMDPROTOS = nameof(Remote) + "_" + nameof(REMOTE_PUSHVMDPROTOS);
        public const string REMOTE_GENERATEVMDTEXT = nameof(Remote) + "_" + nameof(REMOTE_GENERATEVMDTEXT);
        public const string REMOTE_LOADPLUGINS = nameof(Remote) + "_" + nameof(REMOTE_LOADPLUGINS);
        public const string REMOTE_LOADSTATE = nameof(Remote) + "_" + nameof(REMOTE_LOADSTATE);
        public const string REMOTE_SAVESTATE = nameof(Remote) + "_" + nameof(REMOTE_SAVESTATE);
        public const string REMOTE_SAVESTATELESS = nameof(Remote) + "_" + nameof(REMOTE_SAVESTATELESS);
        public const string REMOTE_RESUMEEMULATION = nameof(Remote) + "_" + nameof(REMOTE_RESUMEEMULATION);
        public const string REMOTE_DISABLESAVESTATESUPPORT = nameof(Remote) + "_" + nameof(REMOTE_DISABLESAVESTATESUPPORT);
        public const string REMOTE_DISABLEREALTIMESUPPORT = nameof(Remote) + "_" + nameof(REMOTE_DISABLEREALTIMESUPPORT);
        public const string REMOTE_DISABLEKILLSWITCHSUPPORT = nameof(Remote) + "_" + nameof(REMOTE_DISABLEKILLSWITCHSUPPORT);
        public const string REMOTE_DISABLEGAMEPROTECTIONSUPPORT = nameof(Remote) + "_" + nameof(REMOTE_DISABLEGAMEPROTECTIONSUPPORT);
        public const string REMOTE_BLASTEDITOR_STARTSANITIZETOOL = nameof(Remote) + "_" + nameof(REMOTE_BLASTEDITOR_STARTSANITIZETOOL);
        public const string REMOTE_BLASTEDITOR_LOADCORRUPT = nameof(Remote) + "_" + nameof(REMOTE_BLASTEDITOR_LOADCORRUPT);
        public const string REMOTE_BLASTEDITOR_LOADORIGINAL = nameof(Remote) + "_" + nameof(REMOTE_BLASTEDITOR_LOADORIGINAL);
        public const string REMOTE_BLASTEDITOR_GETLAYERSIZE_UNLOCKEDUNITS = nameof(Remote) + "_" + nameof(REMOTE_BLASTEDITOR_GETLAYERSIZE_UNLOCKEDUNITS);
        public const string REMOTE_BLASTEDITOR_GETLAYERSIZE = nameof(Remote) + "_" + nameof(REMOTE_BLASTEDITOR_GETLAYERSIZE);
        public const string REMOTE_SANITIZETOOL_STARTSANITIZING = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_STARTSANITIZING);
        public const string REMOTE_SANITIZETOOL_LEAVEWITHCHANGES = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_LEAVEWITHCHANGES);
        public const string REMOTE_SANITIZETOOL_LEAVESUBTRACTCHANGES = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_LEAVESUBTRACTCHANGES);
        public const string REMOTE_SANITIZETOOL_YESEFFECT = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_YESEFFECT);
        public const string REMOTE_SANITIZETOOL_NOEFFECT = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_NOEFFECT);
        public const string REMOTE_SANITIZETOOL_REROLL = nameof(Remote) + "_" + nameof(REMOTE_SANITIZETOOL_REROLL);
        public const string REMOTE_BACKUPKEY_REQUEST = nameof(Remote) + "_" + nameof(REMOTE_BACKUPKEY_REQUEST);
        public const string REMOTE_BACKUPKEY_STASH = nameof(Remote) + "_" + nameof(REMOTE_BACKUPKEY_STASH);
        public const string REMOTE_ISNORMALADVANCE = nameof(Remote) + "_" + nameof(REMOTE_ISNORMALADVANCE);
        public const string REMOTE_DOMAIN_PEEKBYTE = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_PEEKBYTE);
        public const string REMOTE_DOMAIN_POKEBYTE = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_POKEBYTE);
        public const string REMOTE_DOMAIN_REFRESHDOMAINS = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_REFRESHDOMAINS);
        public const string REMOTE_DOMAIN_GETDOMAINS = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_GETDOMAINS);
        public const string REMOTE_DOMAIN_VMD_ADD = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_VMD_ADD);
        public const string REMOTE_DOMAIN_VMD_REMOVE = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_VMD_REMOVE);
        public const string REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP = nameof(Remote) + "_" + nameof(REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP);
        public const string REMOTE_KEY_PUSHSAVESTATEDICO = nameof(Remote) + "_" + nameof(REMOTE_KEY_PUSHSAVESTATEDICO);
        public const string REMOTE_KEY_GETRAWBLASTLAYER = nameof(Remote) + "_" + nameof(REMOTE_KEY_GETRAWBLASTLAYER);
        public const string REMOTE_BL_GETDIFFBLASTLAYER = nameof(Remote) + "_" + nameof(REMOTE_BL_GETDIFFBLASTLAYER);
        public const string REMOTE_LONGARRAY_FILTERDOMAIN = nameof(Remote) + "_" + nameof(REMOTE_LONGARRAY_FILTERDOMAIN);
        public const string REMOTE_SET_APPLYUNCORRUPTBL = nameof(Remote) + "_" + nameof(REMOTE_SET_APPLYUNCORRUPTBL);
        public const string REMOTE_SET_APPLYCORRUPTBL = nameof(Remote) + "_" + nameof(REMOTE_SET_APPLYCORRUPTBL);
        public const string REMOTE_CLEARSTEPBLASTUNITS = nameof(Remote) + "_" + nameof(REMOTE_CLEARSTEPBLASTUNITS);
        public const string REMOTE_REMOVEEXCESSINFINITESTEPUNITS = nameof(Remote) + "_" + nameof(REMOTE_REMOVEEXCESSINFINITESTEPUNITS);
        public const string REMOTE_EVENT_LOADGAMEDONE_NEWGAME = nameof(Remote) + "_" + nameof(REMOTE_EVENT_LOADGAMEDONE_NEWGAME);
        public const string REMOTE_EVENT_LOADGAMEDONE_SAMEGAME = nameof(Remote) + "_" + nameof(REMOTE_EVENT_LOADGAMEDONE_SAMEGAME);
        public const string REMOTE_EVENT_CLOSEEMULATOR = nameof(Remote) + "_" + nameof(REMOTE_EVENT_CLOSEEMULATOR);
        public const string REMOTE_EVENT_SHUTDOWN = nameof(Remote) + "_" + nameof(REMOTE_EVENT_SHUTDOWN);
        public const string REMOTE_OPENHEXEDITOR = nameof(Remote) + "_" + nameof(REMOTE_OPENHEXEDITOR);
        public const string REMOTE_LOADROM = nameof(Remote) + "_" + nameof(REMOTE_LOADROM);
        public const string REMOTE_CLOSEGAME = nameof(Remote) + "_" + nameof(REMOTE_CLOSEGAME);
        public const string REMOTE_PRECORRUPTACTION = nameof(Remote) + "_" + nameof(REMOTE_PRECORRUPTACTION);
        public const string REMOTE_POSTCORRUPTACTION = nameof(Remote) + "_" + nameof(REMOTE_POSTCORRUPTACTION);
        public const string REMOTE_BLASTTOOLS_GETAPPLIEDBACKUPLAYER = nameof(Remote) + "_" + nameof(REMOTE_BLASTTOOLS_GETAPPLIEDBACKUPLAYER);
        public const string REMOTE_KEY_SETSYNCSETTINGS = nameof(Remote) + "_" + nameof(REMOTE_KEY_SETSYNCSETTINGS);
        public const string REMOTE_KEY_SETSYSTEMCORE = nameof(Remote) + "_" + nameof(REMOTE_KEY_SETSYSTEMCORE);
        public const string REMOTE_HOTKEY_MANUALBLAST = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_MANUALBLAST);
        public const string REMOTE_HOTKEY_AUTOCORRUPTTOGGLE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_AUTOCORRUPTTOGGLE);
        public const string REMOTE_HOTKEY_ERRORDELAYDECREASE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_ERRORDELAYDECREASE);
        public const string REMOTE_HOTKEY_ERRORDELAYINCREASE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_ERRORDELAYINCREASE);
        public const string REMOTE_HOTKEY_INTENSITYDECREASE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_INTENSITYDECREASE);
        public const string REMOTE_HOTKEY_INTENSITYINCREASE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_INTENSITYINCREASE);
        public const string REMOTE_HOTKEY_GHLOADCORRUPT = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHLOADCORRUPT);
        public const string REMOTE_HOTKEY_GHCORRUPT = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHCORRUPT);
        public const string REMOTE_HOTKEY_GHREROLL = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHREROLL);
        public const string REMOTE_HOTKEY_GHLOAD = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHLOAD);
        public const string REMOTE_HOTKEY_GHSAVE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHSAVE);
        public const string REMOTE_HOTKEY_GHSTASHTOSTOCKPILE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GHSTASHTOSTOCKPILE);
        public const string REMOTE_HOTKEY_BLASTRAWSTASH = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BLASTRAWSTASH);
        public const string REMOTE_HOTKEY_SENDRAWSTASH = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_SENDRAWSTASH);
        public const string REMOTE_HOTKEY_BLASTLAYERTOGGLE = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BLASTLAYERTOGGLE);
        public const string REMOTE_HOTKEY_BLASTLAYERREBLAST = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BLASTLAYERREBLAST);
        public const string REMOTE_HOTKEY_GAMEPROTECTIONBACK = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GAMEPROTECTIONBACK);
        public const string REMOTE_HOTKEY_GAMEPROTECTIONNOW = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_GAMEPROTECTIONNOW);
        public const string REMOTE_HOTKEY_BEINVERTDISABLED = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BEINVERTDISABLED);
        public const string REMOTE_HOTKEY_BEREMOVEDISABLED = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BEREMOVEDISABLED);
        public const string REMOTE_HOTKEY_BEDISABLE50 = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BEDISABLE50);
        public const string REMOTE_HOTKEY_BESHIFTUP = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BESHIFTUP);
        public const string REMOTE_HOTKEY_BESHIFTDOWN = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BESHIFTDOWN);
        public const string REMOTE_HOTKEY_BELOADCORRUPT = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BELOADCORRUPT);
        public const string REMOTE_HOTKEY_BEAPPLY = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BEAPPLY);
        public const string REMOTE_HOTKEY_BESENDSTASH = nameof(Remote) + "_" + nameof(REMOTE_HOTKEY_BESENDSTASH);
        public const string REMOTE_EVENT_EMU_MAINFORM_CLOSE = nameof(Remote) + "_" + nameof(REMOTE_EVENT_EMU_MAINFORM_CLOSE);
        public const string REMOTE_EVENT_EMUSTARTED = nameof(Remote) + "_" + nameof(REMOTE_EVENT_EMUSTARTED);
    }

    public static class Emulator
    {
        public const string EMU_OPEN_HEXEDITOR_ADDRESS = nameof(Emulator) + "_" + nameof(EMU_OPEN_HEXEDITOR_ADDRESS);
        public const string EMU_GET_REALTIME_API = nameof(Emulator) + "_" + nameof(EMU_GET_REALTIME_API);
        public const string EMU_GET_SCREENSHOT = nameof(Emulator) + "_" + nameof(EMU_GET_SCREENSHOT);
        public const string EMU_INFOCUS = nameof(Emulator) + "_" + nameof(EMU_INFOCUS);
    }
}
