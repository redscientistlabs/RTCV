namespace RTCV.NetCore
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Ceras;

    public enum NetworkSide
    {
        NONE,
        CLIENT,
        SERVER
    }

    public enum NetworkStatus
    {
        DISCONNECTED,
        CONNECTIONLOST,
        CONNECTING,
        CONNECTED,
        LISTENING
    }

    public class NetCoreReceiver
    {
        public bool Attached = false;
        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e)
        {
            if (MessageReceived == null)
            {
                throw new Exception("No registered handler for MessageReceived!");
            }

            MessageReceived.Invoke(this, e);
        }
    }

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class NetCoreMessage
    {
        public string Type;
    }

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public class NetCoreSimpleMessage : NetCoreMessage
    {
        public NetCoreSimpleMessage()
        {
        }
        public NetCoreSimpleMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }
    }

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public class NetCoreAdvancedMessage : NetCoreMessage
    {
        public string ReturnedFrom;
        public bool Priority = false;
        public Guid? requestGuid = null;
        public object objectValue = null;

        public NetCoreAdvancedMessage()
        {
        }
        public NetCoreAdvancedMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }

        public NetCoreAdvancedMessage(string _Type, object _Obj)
        {
            Type = _Type.Trim().ToUpper();
            objectValue = _Obj;
        }
    }

    public class NetCoreSpec
    {
        // This is a parameters object that must be passed to netcore in order to establish a link
        // The values here will determine the behavior of the Link

        public NetCoreConnector Connector = null;
        public NetworkSide Side = NetworkSide.NONE;
        public bool AutoReconnect = true;
        public int ClientReconnectDelay = 1500;
        public int DefaultBoopMonitoringCounter = System.Diagnostics.Debugger.IsAttached ? 1500 : 20;

        public bool Attached = true;
        public bool Loopback = true;
        public string IP = "127.0.0.1";
        public int Port = 42069;

        public int messageReadTimerDelay = 5; //represents how often the messages are read (ms) (15ms = ~66fps)
        private Mutex StatusEventLockout = new Mutex();
        private protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ISynchronizeInvoke syncObject
        {
            get => _syncObject;
            set => _syncObject = value;
        }

        private ISynchronizeInvoke _syncObject = null;

        #region Events

        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e) => MessageReceived?.Invoke(this, e);

        public event EventHandler ClientConnecting;
        internal virtual void OnClientConnecting(EventArgs e)
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

                ClientConnecting?.Invoke(this, e);
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

        public event EventHandler ClientConnectingFailed;
        internal virtual void OnClientConnectingFailed(EventArgs e)
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

                ClientConnectingFailed?.Invoke(this, e);
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

        public event EventHandler ClientConnected;
        internal virtual void OnClientConnected(EventArgs e)
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

                ClientConnected?.Invoke(this, e);
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

        public event EventHandler ClientDisconnected;
        internal virtual void OnClientDisconnected(EventArgs e)
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

                ClientDisconnected?.Invoke(this, e);
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

        public event EventHandler ClientConnectionLost;
        internal virtual void OnClientConnectionLost(EventArgs e)
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

                ClientConnectionLost?.Invoke(this, e);
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

        public event EventHandler ServerListening;
        internal virtual void OnServerListening(EventArgs e)
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

                ServerListening?.Invoke(this, e);
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

        public event EventHandler ServerConnected;
        internal virtual void OnServerConnected(EventArgs e)
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

                ServerConnected?.Invoke(this, e);
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

        public event EventHandler ServerDisconnected;
        internal virtual void OnServerDisconnected(EventArgs e)
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

                ServerDisconnected?.Invoke(this, e);
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

        public event EventHandler ServerConnectionLost;
        internal virtual void OnServerConnectionLost(EventArgs e)
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

                ServerConnectionLost?.Invoke(this, e);
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

        public event EventHandler SyncedMessageStart;
        internal virtual void OnSyncedMessageStart(EventArgs e) => SyncedMessageStart?.Invoke(this, e);

        public event EventHandler SyncedMessageEnd;
        internal virtual void OnSyncedMessageEnd(EventArgs e) => SyncedMessageEnd?.Invoke(this, e);

        #endregion

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
    }

    public class ConsoleEx
    {
        private protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static volatile bool ShowDebug = false; // for debugging purposes, put this to true in order to see BOOP and EVENT commands in the console

        public static ConsoleEx singularity
        {
            get
            {
                if (_singularity == null)
                {
                    _singularity = new ConsoleEx();
                }

                return _singularity;
            }
        }

        private static ConsoleEx _singularity = null;
        public ISynchronizeInvoke syncObject = null; //This can contain the form or anything that implements it.

        public void Register(Action<object, NetCoreEventArgs> registrant, ISynchronizeInvoke _syncObject = null)
        {
            syncObject = _syncObject;

            Unregister();
            ConsoleWritten += registrant.Invoke; //We trick the eventhandler in executing the registrant instead
        }

        public void Unregister()
        {
            //finds any delegate referencing ConsoleWritten and dereferences it

            FieldInfo eventFieldInfo = typeof(ConsoleEx).GetField("ConsoleWritten", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(ConsoleEx.singularity);
            Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
            MethodInfo eventRemoveMethodInfo = typeof(ConsoleEx).GetEvent("ConsoleWritten").GetRemoveMethod(true);
            foreach (Delegate eventHandler in invocationList)
            {
                eventRemoveMethodInfo.Invoke(ConsoleEx.singularity, new object[] { eventHandler });
            }
        }

        public event EventHandler<NetCoreEventArgs> ConsoleWritten;
        public virtual void OnConsoleWritten(NetCoreEventArgs e)
        {
            if (syncObject != null)
            {
                if (syncObject.InvokeRequired)
                {
                    syncObject.Invoke(new MethodInvoker(() => { OnConsoleWritten(e); }), null);
                    return;
                }
            }

            ConsoleWritten?.Invoke(this, e);
        }

        public bool HasConsoleEventHandler => ConsoleWritten != null;

        public static void WriteLine(string message)
        {
            if (!ShowDebug && (message.Contains("{BOOP}") || message.StartsWith("{EVENT_")))
            {
                return;
            }

            string consoleLine = "[" + DateTime.Now.ToString("hh:mm:ss.ffff") + "] " + message;

            ConsoleEx.singularity.OnConsoleWritten(new NetCoreEventArgs() { message = new NetCoreSimpleMessage(consoleLine) });

            logger.Info(consoleLine);
            Console.WriteLine(consoleLine);
        }
    }
}
