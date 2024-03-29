namespace RTCV.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Runtime.ExceptionServices;
    using System.Windows.Forms;

    public static class SyncObjectSingleton
    {
        public static Form SyncObject { get; set; }
        public delegate void ActionDelegate(Action a);
        public delegate void ActionDelegateT<T>(Action<T> a, T b);
        public delegate void GenericDelegate();
        public static ActionDelegate EmuInvokeDelegate { get; set; }
        public static bool UseQueue { get; set; } = false;
        public static bool EmuThreadIsMainThread { get; set; } = false;

        public static void FormExecute(Action a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

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
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

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
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

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
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (sync == null)
            {
                throw new ArgumentNullException(nameof(sync));
            }

            if (sync.InvokeRequired)
            {
                sync.InvokeCorrectly(new MethodInvoker(() => { a.Invoke(null, null); }));
            }
            else
            {
                a.Invoke(null, null);
            }
        }

        //https://stackoverflow.com/a/56931457
        private static object InvokeCorrectly(this Control control, Delegate method, params object[] args)
        {
            Exception failure = null;
            var result = control.Invoke(new Func<object>(() =>
            {
                try
                {
                    return method.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    failure = ex.InnerException;
                    return failure;
                }
            }));
            if (failure != null)
            {
                ExceptionDispatchInfo.Capture(failure).Throw();
            }
            return result;
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
