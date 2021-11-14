namespace RTCV
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RTCV.CorruptCore;

    public static class UISideHooks
    {
        public static Action KillSwitchFired;
        public static void OnKillSwitchFired() => KillSwitchFired?.Invoke();

        public static Action<bool> AutoCorruptToggle;
        public static void OnAutoCorruptToggled(bool obj) => AutoCorruptToggle?.Invoke(obj);


        public static Action<long> IntensityChanged;
        public static void OnIntensityChanged(long obj) => IntensityChanged?.Invoke(obj);


        public static Action<long> ErrorDelayChanged;
        public static void OnErrorDelayChanged(long obj) => ErrorDelayChanged?.Invoke(obj);


        public static Action<string[]> SelectedDomainsChanged;
        public static void OnSelectedDomainsChanged(string[] obj) => SelectedDomainsChanged?.Invoke(obj);


        public static Action<StashKey> StashkeyLoaded;
        public static void OnStashkeyLoaded(StashKey obj) => StashkeyLoaded?.Invoke(obj);


        public static Action<Stockpile> StockpileLoaded;
        public static void OnStockpileLoaded(Stockpile obj) => StockpileLoaded?.Invoke(obj);
    }
}
