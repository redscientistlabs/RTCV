namespace RTCV.NetCore
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

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
        private ISynchronizeInvoke syncObject = null; //This can contain the form or anything that implements it.

        public void Register(Action<object, NetCoreEventArgs> registrant, ISynchronizeInvoke _syncObject = null)
        {
            if (registrant == null)
            {
                throw new ArgumentNullException(nameof(registrant));
            }

            syncObject = _syncObject;

            Unregister();
            ConsoleWritten += registrant.Invoke; //We trick the eventhandler in executing the registrant instead
        }

        public static void Unregister()
        {
            //finds any delegate referencing ConsoleWritten and dereferences it

            FieldInfo eventFieldInfo = typeof(ConsoleEx).GetField("ConsoleWritten", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(singularity);
            Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
            MethodInfo eventRemoveMethodInfo = typeof(ConsoleEx).GetEvent("ConsoleWritten").GetRemoveMethod(true);
            foreach (Delegate eventHandler in invocationList)
            {
                eventRemoveMethodInfo.Invoke(singularity, new object[] { eventHandler });
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
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!ShowDebug && (message.Contains("{BOOP}") || message.StartsWith("{EVENT_")))
            {
                return;
            }

            string consoleLine = "[" + DateTime.Now.ToString("hh:mm:ss.ffff") + "] " + message;

            singularity.OnConsoleWritten(new NetCoreEventArgs() { message = new NetCoreSimpleMessage(consoleLine) });

            logger.Info(consoleLine);
            Console.WriteLine(consoleLine);
        }
    }
}
