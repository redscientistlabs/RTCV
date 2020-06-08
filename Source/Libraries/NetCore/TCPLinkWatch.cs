namespace RTCV.NetCore
{
    using System.Threading;

    public class TCPLinkWatch
    {
        private volatile System.Timers.Timer watchdog = null;
        private object watchLock = new object();
        private TCPLink tcp;

        internal TCPLinkWatch(TCPLink _tcp, NetCoreSpec spec)
        {
            watchdog = new System.Timers.Timer
            {
                Interval = spec.ClientReconnectDelay
            };
            watchdog.Elapsed += Watchdog_Elapsed;
            tcp = _tcp;
            tcp.StartNetworking();
            watchdog.Start();
        }

        private void Watchdog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (watchLock)
            {
                if ((tcp.status == NetworkStatus.DISCONNECTED || tcp.status == NetworkStatus.CONNECTIONLOST))
                {
                    tcp.StopNetworking(false);
                    Thread.Sleep(800);
                    tcp.StartNetworking();
                }
            }
        }

        internal void Kill()
        {
            watchdog?.Stop();
            watchdog = null;
        }
    }
}
