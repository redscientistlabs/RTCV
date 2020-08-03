namespace RTCV.NetCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using RTCV.NetCore.NetCore_Extensions;

    public class ReturnWatch : IDisposable
    {
        //This is a component that allows to freeze the thread that asked for a value from a Synced Message
        //This makes inter-process calls able to block and wait for return values to keep code linearity

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private volatile NetCoreSpec spec;
        private volatile ConcurrentDictionary<Guid, object> SyncReturns = new ConcurrentDictionary<Guid, object>();
        private volatile int activeWatches = 0;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Guid guid = Guid.NewGuid();

        public bool IsWaitingForReturn
        {
            get
            {
                return activeWatches > 0;
            }
        }

        internal ReturnWatch(NetCoreSpec _spec)
        {
            spec = _spec;
        }

        public void Kill()
        {
            logger.Info("KillReturnWatch called on {guid}", guid);
            SyncReturns.Clear();
            cts.Cancel();
            cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            cts.Dispose();
        }

        public void AddReturn(NetCoreAdvancedMessage message)
        {
            if (!message.requestGuid.HasValue)
            {
                return;
            }

            SyncReturns.TryAdd(message.requestGuid.Value, message.objectValue);
        }

        internal object GetValue(Guid WatchedGuid, string type)
        {
            object result = null;
            Interlocked.Increment(ref activeWatches);
            try
            {
                result = GetValueTask(WatchedGuid, type, cts.Token).Result;
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException
                    || (e is AggregateException && (e.InnerException is OperationCanceledException || e.InnerException is TaskCanceledException)))
                {
                    logger.Info("WatchedGuid {guid} was cancelled, returning null.", WatchedGuid);
                    Interlocked.Decrement(ref activeWatches);
                    return null;
                }
                //Let it up the chain if it's not a cancellation
                throw;
            }
            Interlocked.Decrement(ref activeWatches);
            return result;
        }

        #pragma warning disable CS1998
        internal async Task<object> GetValueTask(Guid WatchedGuid, string type, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            logger.Trace("GetValue called on {guid}", guid);
            //Jams the current thread until the value is returned or the KillReturnWatch flag is set to true

            logger.Trace("GetValue:Awaiting -> " + type);
            //spec.OnSyncedMessageStart(null);
            spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGESTART}"));

            var attemptsAtReading = 0;

            //If we're this deep, something went really wrong so we just emergency abort
            if (StackFrameHelper.GetCallStackDepth() > 2000)
            {
                cts.Cancel();
                throw new Exception("A fatal error has occurred. Please send this to the devs. You should save your Stockpile then restart the RTC.");
            }

            while (!SyncReturns.ContainsKey(WatchedGuid))
            {
                if (token.IsCancellationRequested)
                {
                    logger.Warn("GetValue:Killed -> " + type);
                    //spec.OnSyncedMessageEnd(null);
                    spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGEEND}"));
                    throw new OperationCanceledException();
                }

                attemptsAtReading++;
                if (attemptsAtReading % 5 == 0)
                {
                    System.Windows.Forms.Application.DoEvents(); //This is a horrible hack we need due to the fact we have synchronous calls that invoke the main thread
                }

                Thread.Sleep(spec.messageReadTimerDelay);
            }

            SyncReturns.TryRemove(WatchedGuid, out object ret);

            logger.Info("GetValue:Returned -> " + type);
            //spec.OnSyncedMessageEnd(null);
            spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGEEND}"));
            return ret;
        }
    }
}
