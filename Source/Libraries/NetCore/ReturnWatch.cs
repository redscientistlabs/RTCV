using System;
using System.Collections.Concurrent;
using System.Threading;

namespace RTCV.NetCore
{
    public class ReturnWatch
    {
        //This is a component that allows to freeze the thread that asked for a value from a Synced Message
        //This makes inter-process calls able to block and wait for return values to keep code linearity

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private volatile NetCoreSpec spec;
        private volatile ConcurrentDictionary<Guid, object> SyncReturns = new ConcurrentDictionary<Guid, object>();
        private volatile int attemptsAtReading = 0;
        private volatile bool KillReturnWatch = false;

        public bool IsWaitingForReturn => attemptsAtReading > 0;

        internal ReturnWatch(NetCoreSpec _spec)
        {
            spec = _spec;
        }

        public void Kill()
        {
            SyncReturns.Clear();
            KillReturnWatch = true;
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
            //Jams the current thread until the value is returned or the KillReturnWatch flag is set to true

            using (new TimePeriod(1))
            {
                logger.Info("GetValue:Awaiting -> " + type);
                //spec.OnSyncedMessageStart(null);
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGESTART}"));

                attemptsAtReading = 0;

                //If we're this deep, something went really wrong so we just emergency abort
                if (StackFrameHelper.GetCallStackDepth() > 2000)
                {
                    KillReturnWatch = true;
                    throw new CustomException("A fatal error has occurred. Please send this to the devs. You should save your Stockpile then restart the RTC.", Environment.StackTrace);
                }

                while (!SyncReturns.ContainsKey(WatchedGuid))
                {
                    if (KillReturnWatch)
                    {
                        //Stops waiting and returns null
                        KillReturnWatch = false;
                        attemptsAtReading = 0;

                        logger.Warn("GetValue:Killed -> " + type);
                        //spec.OnSyncedMessageEnd(null);
                        spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGEEND}"));
                        return null;
                    }

                    attemptsAtReading++;
                    if (attemptsAtReading % 5 == 0)
                    {
                        System.Windows.Forms.Application.DoEvents(); //This is a horrible hack we need due to the fact we have synchronous calls that invoke the main thread
                    }

                    Thread.Sleep(spec.messageReadTimerDelay);
                }

                attemptsAtReading = 0;
                SyncReturns.TryRemove(WatchedGuid, out object ret);

                logger.Info("GetValue:Returned -> " + type);
                //spec.OnSyncedMessageEnd(null);
                spec.Connector.hub.QueueMessage(new NetCoreAdvancedMessage("{EVENT_SYNCEDMESSAGEEND}"));
                return ret;
            }
        }

    }


}
