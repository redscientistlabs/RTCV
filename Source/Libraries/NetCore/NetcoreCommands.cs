namespace RTCV.NetCore.Commands
{
    public static class Basic
    {
        public const string KillswitchPulse = nameof(Basic) + "_" + nameof(KillswitchPulse);
        public const string ResetGameProtectionIfRunning = nameof(Basic) + "_" + nameof(ResetGameProtectionIfRunning);
        public const string ManualBlast = nameof(Basic) + "_" + nameof(ManualBlast);
        public const string ApplyBlastLayer = nameof(Basic) + "_" + nameof(ApplyBlastLayer);
        public const string ApplyCachedBlastLayer = nameof(Basic) + "_" + nameof(ApplyCachedBlastLayer);
        public const string Blast = nameof(Basic) + "_" + nameof(Blast);
        public const string StashKey = nameof(Basic) + "_" + nameof(StashKey);
        public const string ErrorDisableAutoCorrupt = nameof(Basic) + "_" + nameof(ErrorDisableAutoCorrupt);
        public const string GenerateBlastLayer = nameof(Basic) + "_" + nameof(GenerateBlastLayer);
        public const string SaveSavestate = nameof(Basic) + "_" + nameof(SaveSavestate);
        public const string LoadSavestate = nameof(Basic) + "_" + nameof(LoadSavestate);
        public const string RTCInFocus = nameof(Basic) + "_" + nameof(RTCInFocus);
        public const string BlastGeneratorBlast = nameof(Basic) + "_" + nameof(BlastGeneratorBlast);
    }

    public static class Remote {
        public const string PushVanguardSpec = nameof(Remote) + "_" + nameof(PushVanguardSpec);
        public const string PushVanguardSpecUpdate = nameof(Remote) + "_" + nameof(PushVanguardSpecUpdate);
        public const string PushCorruptCoreSpec = nameof(Remote) + "_" + nameof(PushCorruptCoreSpec);
        public const string PushCorruptCoreSpecUpdate = nameof(Remote) + "_" + nameof(PushCorruptCoreSpecUpdate);
        public const string PushUISpec = nameof(Remote) + "_" + nameof(PushUISpec);
        public const string PushUISpecUpdate = nameof(Remote) + "_" + nameof(PushUISpecUpdate);
        public const string AllSpecSent = nameof(Remote) + "_" + nameof(AllSpecSent);
        public const string RenderStop = nameof(Remote) + "_" + nameof(RenderStop);
        public const string RenderStart = nameof(Remote) + "_" + nameof(RenderStart);
        public const string RenderDisplay = nameof(Remote) + "_" + nameof(RenderDisplay);
        public const string EventDomainsUpdated = nameof(Remote) + "_" + nameof(EventDomainsUpdated);
        public const string EventRestrictFeatures = nameof(Remote) + "_" + nameof(EventRestrictFeatures);
        public const string GetBlastGeneratorLayer = nameof(Remote) + "_" + nameof(GetBlastGeneratorLayer);
        public const string PushRTCSpec = nameof(Remote) + "_" + nameof(PushRTCSpec);
        public const string PushRTCSpecUpdate = nameof(Remote) + "_" + nameof(PushRTCSpecUpdate);
        public const string PushVMDProtos = nameof(Remote) + "_" + nameof(PushVMDProtos);
        public const string GenerateVMDText = nameof(Remote) + "_" + nameof(GenerateVMDText);
        public const string LoadPlugins = nameof(Remote) + "_" + nameof(LoadPlugins);
        public const string LoadState = nameof(Remote) + "_" + nameof(LoadState);
        public const string SaveState = nameof(Remote) + "_" + nameof(SaveState);
        public const string SaveStateless = nameof(Remote) + "_" + nameof(SaveStateless);
        public const string ResumeEmulation = nameof(Remote) + "_" + nameof(ResumeEmulation);
        public const string DisableSavestateSupport = nameof(Remote) + "_" + nameof(DisableSavestateSupport);
        public const string DisableRealtimeSupport = nameof(Remote) + "_" + nameof(DisableRealtimeSupport);
        public const string DisableKillSwitchSupport = nameof(Remote) + "_" + nameof(DisableKillSwitchSupport);
        public const string DisableGameProtectionSupport = nameof(Remote) + "_" + nameof(DisableGameProtectionSupport);
        public const string BlastEditorStartSanitizeTool = nameof(Remote) + "_" + nameof(BlastEditorStartSanitizeTool);
        public const string BlastEditorLoadCorrupt = nameof(Remote) + "_" + nameof(BlastEditorLoadCorrupt);
        public const string BlastEditorLoadOriginal = nameof(Remote) + "_" + nameof(BlastEditorLoadOriginal);
        public const string BlastEditorGetLayerSizeUnlockedUnits = nameof(Remote) + "_" + nameof(BlastEditorGetLayerSizeUnlockedUnits);
        public const string BlastEditorGetLayerSize = nameof(Remote) + "_" + nameof(BlastEditorGetLayerSize);
        public const string SanitizeToolStartSanitizing = nameof(Remote) + "_" + nameof(SanitizeToolStartSanitizing);
        public const string SanitizeToolLeaveWithChanges = nameof(Remote) + "_" + nameof(SanitizeToolLeaveWithChanges);
        public const string SanitizeToolLeaveSubtractChanges = nameof(Remote) + "_" + nameof(SanitizeToolLeaveSubtractChanges);
        public const string SanitizeToolYesEffect = nameof(Remote) + "_" + nameof(SanitizeToolYesEffect);
        public const string SanitizeToolNoEffect = nameof(Remote) + "_" + nameof(SanitizeToolNoEffect);
        public const string SanitizeToolReroll = nameof(Remote) + "_" + nameof(SanitizeToolReroll);
        public const string TriggerHotkey = nameof(Remote) + "_" + nameof(TriggerHotkey);
        public const string BackupKeyRequest = nameof(Remote) + "_" + nameof(BackupKeyRequest);
        public const string BackupKeyStash = nameof(Remote) + "_" + nameof(BackupKeyStash);
        public const string IsNormalAdvance = nameof(Remote) + "_" + nameof(IsNormalAdvance);
        public const string DomainPeekByte = nameof(Remote) + "_" + nameof(DomainPeekByte);
        public const string DomainPokeByte = nameof(Remote) + "_" + nameof(DomainPokeByte);
        public const string DomainRefreshDomains = nameof(Remote) + "_" + nameof(DomainRefreshDomains);
        public const string DomainGetDomains = nameof(Remote) + "_" + nameof(DomainGetDomains);
        public const string DomainVMDAdd = nameof(Remote) + "_" + nameof(DomainVMDAdd);
        public const string DomainVMDRemove = nameof(Remote) + "_" + nameof(DomainVMDRemove);
        public const string DomainActiveTableMakeDump = nameof(Remote) + "_" + nameof(DomainActiveTableMakeDump);
        public const string KeyPushSaveStateDICO = nameof(Remote) + "_" + nameof(KeyPushSaveStateDICO);
        public const string KeyGetRawBlastLayer = nameof(Remote) + "_" + nameof(KeyGetRawBlastLayer);
        public const string BLGetDiffBlastLayer = nameof(Remote) + "_" + nameof(BLGetDiffBlastLayer);
        public const string LongArrayFilterDomain = nameof(Remote) + "_" + nameof(LongArrayFilterDomain);
        public const string ClearBlastlayerCache = nameof(Remote) + "_" + nameof(ClearBlastlayerCache);
        public const string SetApplyUncorruptBL = nameof(Remote) + "_" + nameof(SetApplyUncorruptBL);
        public const string SetApplyCorruptBL = nameof(Remote) + "_" + nameof(SetApplyCorruptBL);
        public const string ClearStepBlastUnits = nameof(Remote) + "_" + nameof(ClearStepBlastUnits);
        public const string RemoveExcessInfiniteStepUnits = nameof(Remote) + "_" + nameof(RemoveExcessInfiniteStepUnits);
        public const string EventLoadGameDoneNewGame = nameof(Remote) + "_" + nameof(EventLoadGameDoneNewGame);
        public const string EventLoadgameDoneSameGame = nameof(Remote) + "_" + nameof(EventLoadgameDoneSameGame);
        public const string EventCloseEmulator = nameof(Remote) + "_" + nameof(EventCloseEmulator);
        public const string EventShutdown = nameof(Remote) + "_" + nameof(EventShutdown);
        public const string OpenHexEditor = nameof(Remote) + "_" + nameof(OpenHexEditor);
        public const string LoadROM = nameof(Remote) + "_" + nameof(LoadROM);
        public const string CloseGame = nameof(Remote) + "_" + nameof(CloseGame);
        public const string PreCorruptAction = nameof(Remote) + "_" + nameof(PreCorruptAction);
        public const string PostCorruptAction = nameof(Remote) + "_" + nameof(PostCorruptAction);
        public const string BlastToolsGetAppliedBackupLayer = nameof(Remote) + "_" + nameof(BlastToolsGetAppliedBackupLayer);
        public const string KeySetSyncSettings = nameof(Remote) + "_" + nameof(KeySetSyncSettings);
        public const string KeySetSystemCore = nameof(Remote) + "_" + nameof(KeySetSystemCore);
        public const string HotkeyManualBlast = nameof(Remote) + "_" + nameof(HotkeyManualBlast);
        public const string HotkeyAutoCorruptToggle = nameof(Remote) + "_" + nameof(HotkeyAutoCorruptToggle);
        public const string HotkeyErrorDelayDecrease = nameof(Remote) + "_" + nameof(HotkeyErrorDelayDecrease);
        public const string HotkeyErrorDelayIncrease = nameof(Remote) + "_" + nameof(HotkeyErrorDelayIncrease);
        public const string HotkeyIntensityDecrease = nameof(Remote) + "_" + nameof(HotkeyIntensityDecrease);
        public const string HotkeyIntensityIncrease = nameof(Remote) + "_" + nameof(HotkeyIntensityIncrease);
        public const string HotkeyGHLoadCorrupt = nameof(Remote) + "_" + nameof(HotkeyGHLoadCorrupt);
        public const string HotkeyGHCorrupt = nameof(Remote) + "_" + nameof(HotkeyGHCorrupt);
        public const string HotkeyGHReroll = nameof(Remote) + "_" + nameof(HotkeyGHReroll);
        public const string HotkeyGHLoad = nameof(Remote) + "_" + nameof(HotkeyGHLoad);
        public const string HotkeyGHSave = nameof(Remote) + "_" + nameof(HotkeyGHSave);
        public const string HotkeyGHStashToStockpile = nameof(Remote) + "_" + nameof(HotkeyGHStashToStockpile);
        public const string HotkeyBlastRawStash = nameof(Remote) + "_" + nameof(HotkeyBlastRawStash);
        public const string HotkeySendRawStash = nameof(Remote) + "_" + nameof(HotkeySendRawStash);
        public const string HotkeyBlastLayerToggle = nameof(Remote) + "_" + nameof(HotkeyBlastLayerToggle);
        public const string HotkeyBlastLayerReBlast = nameof(Remote) + "_" + nameof(HotkeyBlastLayerReBlast);
        public const string HotkeyGameProtectionBack = nameof(Remote) + "_" + nameof(HotkeyGameProtectionBack);
        public const string HotkeyGameProtectionNow = nameof(Remote) + "_" + nameof(HotkeyGameProtectionNow);
        public const string HotkeyBeInvertDisabled = nameof(Remote) + "_" + nameof(HotkeyBeInvertDisabled);
        public const string HotkeyBeRemoveDisabled = nameof(Remote) + "_" + nameof(HotkeyBeRemoveDisabled);
        public const string HotkeyBeDisable50 = nameof(Remote) + "_" + nameof(HotkeyBeDisable50);
        public const string HotkeyBeShiftUp = nameof(Remote) + "_" + nameof(HotkeyBeShiftUp);
        public const string HotkeyBeShiftDown = nameof(Remote) + "_" + nameof(HotkeyBeShiftDown);
        public const string HotkeyBeLoadCorrupt = nameof(Remote) + "_" + nameof(HotkeyBeLoadCorrupt);
        public const string HotkeyBeApply = nameof(Remote) + "_" + nameof(HotkeyBeApply);
        public const string HotkeyBeSendStash = nameof(Remote) + "_" + nameof(HotkeyBeSendStash);
        public const string EventEmuMainFormClose = nameof(Remote) + "_" + nameof(EventEmuMainFormClose);
        public const string EventEmuStarted = nameof(Remote) + "_" + nameof(EventEmuStarted);
    }

    public static class Emulator
    {
        public const string OpenHexEditorAddress = nameof(Emulator) + "_" + nameof(OpenHexEditorAddress);
        public const string GetRealtimeAPI = nameof(Emulator) + "_" + nameof(GetRealtimeAPI);
        public const string GetScreenshot = nameof(Emulator) + "_" + nameof(GetScreenshot);
        public const string InFocus = nameof(Emulator) + "_" + nameof(InFocus);
    }
}
