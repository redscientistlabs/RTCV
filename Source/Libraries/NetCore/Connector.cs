namespace RTCV.NetCore
{
    using System;
    using System.Linq;
    using System.Threading;
    using RTCV.NetCore.Enums;

    public class NetCoreConnector : IRoutable, IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NetCoreSpec Spec { get; private set; } = null;
        internal UDPLink udp = null;
        internal volatile TCPLink tcp = null;
        internal MessageHub hub = null;
        internal ReturnWatch watch = null;

        public NetworkStatus status => tcp?.status ?? NetworkStatus.DISCONNECTED;

        public bool Disposed { get; set; } = false;

        public NetCoreConnector(NetCoreSpec spec)
        {
            logger.Debug($"NetCore Initialization");

            Spec = spec ?? throw new ArgumentNullException(nameof(spec));
            Spec.Connector = this;
            Initialize();

            logger.Debug($"NetCore Started");
        }

        private void Initialize()
        {
            if (Spec.Side == NetworkSide.NONE)
            {
                logger.Debug("Could not initialize connector : Side was not set");
                return;
            }

            try
            {
                hub = new MessageHub(Spec);
                udp = new UDPLink(Spec);
                tcp = new TCPLink(Spec);
                watch = new ReturnWatch(Spec);
            }
            catch (Exception)
            {
                Kill();
                throw;
            }
        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if ((e.message as NetCoreAdvancedMessage)?.requestGuid != null)
            {
                return SendMessage(e.message, true, true);
            }
            else
            {
                SendMessage(e.message, false, true);
                return null;
            }
        }

        public void SendMessage(string message) => SendMessage(new NetCoreSimpleMessage(message));
        public void SendMessage(string message, object value) => SendMessage(new NetCoreAdvancedMessage(message) { objectValue = value });
        public object SendSyncedMessage(string message) => SendMessage(new NetCoreAdvancedMessage(message), true);
        public object SendSyncedMessage(string message, object value) => SendMessage(new NetCoreAdvancedMessage(message) { objectValue = value }, true);

        private object SendMessage(NetCoreMessage _message, bool synced = false, bool external = false)
        {
            if (!external && _message.Type.Contains('|'))
            {
                string[] splitType = _message.Type.Split('|');
                string target = splitType[0];
                _message.Type = splitType[1];
                if (LocalNetCoreRouter.hasEndpoint(target))
                {
                    if (synced)
                    {
                        if (((NetCoreAdvancedMessage)_message).requestGuid == null)
                        {
                            ((NetCoreAdvancedMessage)_message).requestGuid = Guid.NewGuid();
                        }
                    }

                    return LocalNetCoreRouter.Route(target, new NetCoreEventArgs() { message = _message });
                }
            }

            return hub?.SendMessage(_message, synced);
        }

        public void Stop(bool force = false)
        {
            Disposed = true;

            if (!force)
            {
                try
                {
                    tcp?.StopNetworking(!force);
                    DateTime startDT = DateTime.Now;

                    while (tcp?.client != null && ((DateTime.Now - startDT).TotalMilliseconds) < 1500) // wait timeout
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Something went terribly wrong when stopping tcp networking");
                }
            }

            try
            {
                tcp?.Kill();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something went terribly wrong when killing tcp");
            }
            try
            {
                udp?.Kill();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something went terribly wrong when killing udp");
            }
            try
            {
                hub?.Kill();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something went terribly wrong when killing hub");
            }
            try
            {
                watch?.Kill();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something went terribly wrong when killing watch");
            }

            logger.Debug($"NetCore {(force ? "Killed" : "Stopped")}");
        }

        public void Kill() => Stop(true);

        public void Restart()
        {
            Kill();
            Initialize();
        }

        public void Dispose()
        {
            Stop(true);
            Spec?.Dispose();
            udp?.Dispose();
            tcp?.Dispose();
            hub?.Dispose();
            watch?.Dispose();
        }
    }
}
