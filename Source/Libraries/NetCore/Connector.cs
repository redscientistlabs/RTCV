using System;
using System.Linq;
using System.Threading;

namespace RTCV.NetCore
{
    public class NetCoreConnector : IRoutable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NetCoreSpec spec = null;

        internal UDPLink udp = null;
        internal volatile TCPLink tcp = null;
        internal MessageHub hub = null;
        internal ReturnWatch watch = null;

        public NetworkStatus status => tcp?.status ?? NetworkStatus.DISCONNECTED;

        public bool Disposed { get; set; } = false;

        public NetCoreConnector(NetCoreSpec _spec)
        {
            logger.Debug($"NetCore Initialization");

            spec = _spec;
            spec.Connector = this;
            Initialize();

            logger.Debug($"NetCore Started");
        }

        private void Initialize()
        {
            if (spec.Side == NetworkSide.NONE)
            {
                logger.Debug("Could not initialize connector : Side was not set");
                return;
            }

            try
            {
                hub = new MessageHub(spec);
                udp = new UDPLink(spec);
                tcp = new TCPLink(spec);
                watch = new ReturnWatch(spec);
            }
            catch (Exception ex)
            {
                Kill();
                throw ex;
            }
        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
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
                tcp?.StopNetworking(!force);
                DateTime startDT = DateTime.Now;

                while (tcp?.client != null && ((DateTime.Now - startDT).TotalMilliseconds) < 1500) // wait timeout
                {
                    Thread.Sleep(50);
                }
            }

            tcp?.Kill();
            udp?.Kill();
            hub?.Kill();
            watch?.Kill();

            logger.Debug($"NetCore {(force ? "Killed" : "Stopped")}");
        }

        public void Kill() => Stop(true);
    }
}
