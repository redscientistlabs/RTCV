namespace RTCV.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using RTCV.NetCore.NetCore_Extensions;

    public static class SyncObjectSingleton
    {
        public static Form SyncObject;
        public static volatile bool executing;
        public static volatile Queue<Action> ActionQueue = new Queue<Action>();
        public delegate void ActionDelegate(Action a);
        public delegate void ActionDelegateT<T>(Action<T> a, T b);
        public delegate void GenericDelegate();
        public static ActionDelegate EmuInvokeDelegate;
        public static bool UseQueue = false;
        public static bool EmuThreadIsMainThread = false;

        public static void FormExecute(Action a)
        {
            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(new MethodInvoker(a.Invoke));
            }
            else
            {
                a.Invoke();
            }
        }

        public static void FormExecute<T>(Action<T> a, T b)
        {
            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(new MethodInvoker(() => { a.Invoke(b); }));
            }
            else
            {
                a.Invoke(b);
            }
        }

        public static void FormExecute(Delegate a)
        {
            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(a);
            }
            else
            {
                a.DynamicInvoke();
            }
        }

        public static void EmuThreadExecute(Action a, bool fallBackToMainThread)
        {
            if (UseQueue)
            {
                ActionDistributor.Enqueue("ACTION", a);
                ActionDistributor.WaitForAction("ACTION", a);
                return;
            }

            //We invoke the main thread before invoking the thread because
            //various emulators need this (Dolphin) and chaining delegates wasn't worth it
            if (EmuInvokeDelegate != null)
            {
                FormExecute(() => { EmuInvokeDelegate.Invoke(a); });
            }
            //If there's no emuthread, fall back to the main thread if told to
            else if (fallBackToMainThread || EmuThreadIsMainThread)
            {
                FormExecute(() => { a.Invoke(); });
            }
        }

        public static void SyncObjectExecute(Form sync, Action<object, EventArgs> a)
        {
            if (sync.InvokeRequired)
            {
                sync.InvokeCorrectly(new MethodInvoker(() => { a.Invoke(null, null); }));
            }
            else
            {
                a.Invoke(null, null);
            }
        }
    }

    public static class ActionDistributor
    {
        private static Dictionary<string, LinkedList<Action>> ActionDico = new Dictionary<string, LinkedList<Action>>();
        private static object ActionPoolLock = new object();

        public static void Enqueue(string key, Action act)
        {
            lock (ActionPoolLock)
            {
                if (ActionDico.TryGetValue(key, out LinkedList<Action> actions))
                {
                    actions.AddLast(act);
                }
                else
                {
                    ActionDico[key] = new LinkedList<Action>();
                    actions = ActionDico[key];
                    actions.AddLast(act);
                }
            }
        }

        public static void WaitForAction(string key, Action act)
        {
            LinkedList<Action> actions;

            lock (ActionPoolLock)
            {
                if (!ActionDico.TryGetValue(key, out actions))
                {
                    return;
                }
            }

            while (actions.Contains(act)) { Thread.Sleep(10); } //Lock until action has been executed
        }

        public static void Execute(string key)
        {
            lock (ActionPoolLock)
            {
                if (!ActionDico.TryGetValue(key, out LinkedList<Action> actions))
                {
                    return;
                }

                while (true)
                {
                    if (actions.Count == 0)
                    {
                        return;
                    }

                    var act = actions.First.Value;
                    act.Invoke();
                    actions.RemoveFirst();
                }
            }
        }
    }
}
