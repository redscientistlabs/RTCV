namespace RTCV.NetCore
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using RTCV.NetCore.Enums;

    public class NetCoreSpec : IDisposable
    {
        // This is a parameters object that must be passed to netcore in order to establish a link
        // The values here will determine the behavior of the Link

        public NetCoreConnector Connector { get; set; } = null;
        public NetworkSide Side { get; set; } = NetworkSide.NONE;
        public int ClientReconnectDelay { get; set; } = 1500;
        public int DefaultBoopMonitoringCounter { get; } = System.Diagnostics.Debugger.IsAttached ? 1500 : 20;

        public bool Attached { get; set; } = true;
        public bool Loopback { get; set; } = true;
        public string IP { get; } = "127.0.0.1";
        public int Port { get; } = 42069;

        public int messageReadTimerDelay { get; } = 5; //represents how often the messages are read (ms) (15ms = ~66fps)
        private Mutex StatusEventLockout = new Mutex();
        private protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ISynchronizeInvoke syncObject
        {
            get => _syncObject;
            set => _syncObject = value;
        }

        private ISynchronizeInvoke _syncObject = null;

        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e) => MessageReceived?.Invoke(this, e);

        private void RunUnderLock(Action action)
        {
            logger.Trace("Thread id {0} requesting StatusEventLockout Mutex.", Thread.CurrentThread.ManagedThreadId);
            try
            {
                try
                {
                    StatusEventLockout.WaitOne();
                    logger.Trace("Thread id {0} obtained StatusEventLockout Mutex.",
                        Thread.CurrentThread.ManagedThreadId);
                }
                catch (AbandonedMutexException ame)
                {
                    logger.Trace("Thread id {0} obtained StatusEventLockout Mutex (AbandonedMutexException) {1}.",
                        Thread.CurrentThread.ManagedThreadId, ame);
                }
                catch (Exception ex)
                {
                    logger.Trace(ex, "Exception occurred");
                    throw;
                }

                action();
            }
            finally
            {
                logger.Trace("Thread id {0} releasing StatusEventLockout Mutex.",
                    Thread.CurrentThread.ManagedThreadId);
                StatusEventLockout.ReleaseMutex();
                logger.Trace("Thread id {0} released StatusEventLockout Mutex.",
                    Thread.CurrentThread.ManagedThreadId);
            }
        }

        public event EventHandler ClientConnecting;
        internal virtual void OnClientConnecting(EventArgs e)
        {
            RunUnderLock(() => ClientConnecting?.Invoke(this, e));
        }

        public event EventHandler ClientConnectingFailed;
        internal virtual void OnClientConnectingFailed(EventArgs e)
        {
            RunUnderLock(() => ClientConnectingFailed?.Invoke(this, e));
        }

        public event EventHandler ClientConnected;
        internal virtual void OnClientConnected(EventArgs e)
        {
            RunUnderLock(() => ClientConnected?.Invoke(this, e));
        }

        public event EventHandler ClientDisconnected;
        internal virtual void OnClientDisconnected(EventArgs e)
        {
            RunUnderLock(() =>  ClientDisconnected?.Invoke(this, e));
        }

        public event EventHandler ClientConnectionLost;
        internal virtual void OnClientConnectionLost(EventArgs e)
        {
            RunUnderLock(() =>  ClientConnectionLost?.Invoke(this, e));
        }

        public event EventHandler ServerListening;
        internal virtual void OnServerListening(EventArgs e)
        {
            RunUnderLock(() => ServerListening?.Invoke(this, e));
        }

        public event EventHandler ServerConnected;
        internal virtual void OnServerConnected(EventArgs e)
        {
            RunUnderLock(() =>  ServerConnected?.Invoke(this, e));
        }

        public event EventHandler ServerDisconnected;
        internal virtual void OnServerDisconnected(EventArgs e)
        {
            RunUnderLock(() => ServerDisconnected?.Invoke(this, e));
        }

        public event EventHandler ServerConnectionLost;
        internal virtual void OnServerConnectionLost(EventArgs e)
        {
            RunUnderLock(() => ServerConnectionLost?.Invoke(this, e));
        }

        public event EventHandler SyncedMessageStart;
        internal virtual void OnSyncedMessageStart(EventArgs e) => SyncedMessageStart?.Invoke(this, e);

        public event EventHandler SyncedMessageEnd;
        internal virtual void OnSyncedMessageEnd(EventArgs e) => SyncedMessageEnd?.Invoke(this, e);

        public bool LockStatusEventLockout(int timeout = int.MaxValue)
        {
            bool s = false;
            logger.Trace("Thread id {0} requested Lock StatusEventLockout Mutex.", Thread.CurrentThread.ManagedThreadId);
            try
            {
                s = StatusEventLockout.WaitOne(timeout);
                logger.Trace("Thread id {0} got StatusEventLockout Mutex.", Thread.CurrentThread.ManagedThreadId);
            }
            catch (AbandonedMutexException)
            {
                logger.Trace("Thread id {0} got StatusEventLockout Mutex (AbandonedMutexException).",
                    Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "Exception!");
            }

            return s;
        }

        public void UnlockLockStatusEventLockout()
        {
            logger.Trace("Thread id {0} requested Unlock StatusEventLockout Mutex.", Thread.CurrentThread.ManagedThreadId);
            try
            {
                StatusEventLockout.ReleaseMutex();
                logger.Trace("Thread id {0} Unlocked StatusEventLockout Mutex.", Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "Exception!");
            }
        }

        public void Dispose()
        {
            Connector?.Dispose();
            StatusEventLockout?.Dispose();
        }
    }
}
