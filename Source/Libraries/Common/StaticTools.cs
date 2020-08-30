namespace RTCV.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using NLog;

    #pragma warning disable CA1040 // Allow this interface to be empty, since it's used to signal auto-coloriation for a class
    // Implementing this interface causes auto-coloration.
    public interface IAutoColorize { }

    public class FormRegisteredEventArgs : EventArgs
    {
        public Form Form;
        public FormRegisteredEventArgs(Form form)
        {
            Form = form;
        }
    }
    public class FormRegister
    {
        public event EventHandler<FormRegisteredEventArgs> FormRegistered;
        public virtual void OnFormRegistered(FormRegisteredEventArgs e) => FormRegistered?.Invoke(this, e);
    }

    //Static singleton manager
    //Call or create a singleton using class type
    public static class S
    {
        private static readonly ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();
        public static FormRegister formRegister = new FormRegister();
        private static object lockObject = new object();

        [ThreadStatic]
        public static volatile Dictionary<int, List<string>> InvokeStackTraces = new Dictionary<int, List<string>>();

        private static readonly object dicoLock = new object();

        public static void InvokeLog(this ISynchronizeInvoke si, Delegate method, object[] args)
        {
            int pid = Thread.CurrentThread.ManagedThreadId;

            List<string> IST = null;
            bool listExists;

            lock (dicoLock)
            {
                listExists = InvokeStackTraces.TryGetValue(32, out IST);
            }

            if (listExists)
            {
                IST = new List<string>();

                lock (dicoLock)
                {
                    InvokeStackTraces.Add(pid, IST);
                }
            }
            IST.Add(Environment.StackTrace);

            var iar = si.BeginInvoke(method, args);
            si.EndInvoke(iar);

            lock (dicoLock)
            {
                InvokeStackTraces.Remove(InvokeStackTraces.Count - 1);
                if (InvokeStackTraces.Count == 0)
                {
                    InvokeStackTraces.Remove(pid);
                }
            }
        }

        public static bool ISNULL<T>()
        {
            Type typ = typeof(T);
            return !instances.ContainsKey(typ);
        }

        private static Type FINDTYPE(string name, bool any = false)
        {
            //thx https://stackoverflow.com/questions/4692340/find-types-in-all-assemblies

            return
            AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => (any ? t.FullName.Contains(name) : t.FullName.Equals(name)));
        }

        public static object BLINDMAKE(string name)
        {
            //Returns a distinct new instance of an object using a string instead of type
            //Will scan all loaded asemblies to fetch the needed type for creating the instance

            //The instance is not registered in the singleton dictionary

            try
            {
                Type typ = FINDTYPE(name, true);
                var o = Activator.CreateInstance(typ);
                return o;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return null;
            }
        }

        public static T GET<T>()
        {
            Type typ = typeof(T);

            if (!instances.TryGetValue(typ, out object o))
            {
                lock (lockObject)
                {
                    //Check again in case we had stacked threads
                    if (!instances.TryGetValue(typ, out o))
                    {
                        o = Activator.CreateInstance(typ);
                        instances[typ] = o;

                        if (typ.IsSubclassOf(typeof(Form)))
                        {
                            formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                        }
                    }
                }
            }
            return (T)o;
        }

        //returns all singletons that implements a certain type
        public static T[] GETINTERFACES<T>()
        {
            lock (lockObject)
            {
                return instances.Values
                    .OfType<T>()
                    .ToArray();
            }
        }

        public static object GET(Type typ)
        {
            if (!instances.TryGetValue(typ, out object o))
            {
                lock (lockObject)
                {
                    //Check again in case we had stacked threads
                    if (!instances.TryGetValue(typ, out o))
                    {
                        o = Activator.CreateInstance(typ);
                        instances[typ] = o;

                        if (typ.IsSubclassOf(typeof(Form)))
                        {
                            formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                        }
                    }
                }
            }
            return o;
        }

        public static void SET<T>(T newTyp)
        {
            lock (lockObject)
            {
                Type typ = typeof(T);
                if (newTyp == null)
                {
                    instances.TryRemove(typ, out _);
                }
                else
                {
                    instances[typ] = newTyp;
                }

                if (typ.IsSubclassOf(typeof(Form)))
                {
                    formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                }
            }
        }
    }
}
