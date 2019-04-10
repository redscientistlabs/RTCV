using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.NetCore
{
	public static class SyncObjectSingleton
	{
		public static Form SyncObject;
        public static volatile bool executing;
        public static volatile Queue<Action<object, EventArgs>> ActionQueue = new Queue<Action<object, EventArgs>>();

        public static void FormExecute(Action<object, EventArgs> a, object[] args = null, bool useQueue = false)
        {
            if (useQueue)
            {
                ActionDistributor.Enqueue("ACTION",a);
                ActionDistributor.WaitForAction("ACTION", a);
            }
            else
            {
                if (SyncObject.InvokeRequired)
                    SyncObject.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
                else
                    a.Invoke(null, null);
            }
        }

        public static void SyncObjectExecute(Form sync, Action<object, EventArgs> a, object[] args = null)
		{
			if (sync.InvokeRequired)
				sync.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
			else
				a.Invoke(null, null);
		}
	}
    public static class ActionDistributor
    {
        static volatile Dictionary<string, LinkedList<Action<object, EventArgs>>> ActionDico = new Dictionary<string, LinkedList<Action<object, EventArgs>>>();
        static object ActionPoolLock = new object();

        public static void Enqueue(string key, Action<object, EventArgs> act)
        {
            lock (ActionPoolLock)
            {
                if (ActionDico.TryGetValue(key, out LinkedList<Action<object, EventArgs>> actions))
                    actions.AddLast(act);
                else
                {
                    ActionDico[key] = new LinkedList<Action<object, EventArgs>>();
                    actions = ActionDico[key];
                    actions.AddLast(act);
                }
            }
        }

        public static void WaitForAction(string key, Action<object, EventArgs> act)
        {
            LinkedList<Action<object, EventArgs>> actions;

            lock (ActionPoolLock)
            {
                if (!ActionDico.TryGetValue(key, out actions))
                    return;
            }

            while (actions.Contains(act)) { Thread.Sleep(10); } //Lock until action has been executed

        }

        public static void Execute(string key)
        {
            lock (ActionPoolLock)
            {
                LinkedList<Action<object, EventArgs>> actions;
                if (!ActionDico.TryGetValue(key, out actions))
                    return;

                while (true)
                {
                    if (actions.Count == 0)
                        return;

                    var act = actions.First.Value;
                    act.Invoke(null, null);
                    actions.RemoveFirst();

                }
            }
        }
    }
}
