using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RTCV.NetCore
{

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

    [Serializable()]
    public abstract class NetCoreMessage
    {
        public string Type;
    }

    [Serializable()]
    public class NetCoreSimpleMessage : NetCoreMessage
    {
        public NetCoreSimpleMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }
    }

    [Serializable()]
    public class NetCoreAdvancedMessage : NetCoreMessage
    {
        public string ReturnedFrom;
        public bool Priority = false;
        public Guid? requestGuid = null;
        public object objectValue = null;

        public NetCoreAdvancedMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }

        public NetCoreAdvancedMessage(string _Type, object _Obj)
        {
            Type = _Type.Trim().ToUpper();
            objectValue = _Obj;
        }

        public NetCoreAdvancedMessage()
        {
        }

    }

    public class NetCoreSpec
    {
        // This is a parameters object that must be passed to netcore in order to establish a link
        // The values here will determine the behavior of the Link

        public NetCoreConnector Connector = null;
        public NetworkSide Side = NetworkSide.NONE;
        public bool AutoReconnect = true;
        public int ClientReconnectDelay = 600;
        public int DefaultBoopMonitoringCounter = 15;

        public bool Loopback = true;
        public string IP = "127.0.0.1";
        public int Port = 42069;

        public int messageReadTimerDelay = 10; //represents how often the messages are read (ms) (15ms = ~66fps)

        public ISynchronizeInvoke syncObject
        {
            get { return _syncObject; }
            set {
                _syncObject = value;
            }
        }

        private ISynchronizeInvoke _syncObject = null;

        #region Events

        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e) => MessageReceived?.Invoke(this, e);

        public event EventHandler ClientConnecting;
        internal virtual void OnClientConnecting(EventArgs e) => ClientConnecting?.Invoke(this, e);

        public event EventHandler ClientConnectingFailed;
        internal virtual void OnClientConnectingFailed(EventArgs e) => ClientConnectingFailed?.Invoke(this, e);

        public event EventHandler ClientConnected;
        internal virtual void OnClientConnected(EventArgs e) => ClientConnected?.Invoke(this, e);

        public event EventHandler ClientDisconnected;
        internal virtual void OnClientDisconnected(EventArgs e) => ClientDisconnected?.Invoke(this, e);

        public event EventHandler ClientConnectionLost;
        internal virtual void OnClientConnectionLost(EventArgs e) => ClientConnectionLost?.Invoke(this, e);

        public event EventHandler ServerListening;
        internal virtual void OnServerListening(EventArgs e) => ServerListening?.Invoke(this, e);

        public event EventHandler ServerConnected;
        internal virtual void OnServerConnected(EventArgs e) => ServerConnected?.Invoke(this, e);

        public event EventHandler ServerDisconnected;
        internal virtual void OnServerDisconnected(EventArgs e) => ServerDisconnected?.Invoke(this, e);

        public event EventHandler ServerConnectionLost;
        internal virtual void OnServerConnectionLost(EventArgs e) => ServerConnectionLost?.Invoke(this, e);

        public event EventHandler SyncedMessageStart;
        internal virtual void OnSyncedMessageStart(EventArgs e) => SyncedMessageStart?.Invoke(this, e);

        public event EventHandler SyncedMessageEnd;
        internal virtual void OnSyncedMessageEnd(EventArgs e) => SyncedMessageEnd?.Invoke(this, e);

        #endregion

    }

    public class ConsoleEx
    {
        public static volatile bool ShowDebug = false; // for debugging purposes, put this to true in order to see BOOP and EVENT commands in the console

        public static ConsoleEx singularity
        {
            get
            {
                if (_singularity == null)
                    _singularity = new ConsoleEx();
                return _singularity;
            }
        }
        static ConsoleEx _singularity = null;
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
                eventRemoveMethodInfo.Invoke(ConsoleEx.singularity, new object[] { eventHandler });
        }

        public event EventHandler<NetCoreEventArgs> ConsoleWritten;
        public virtual void OnConsoleWritten(NetCoreEventArgs e)
        {
            if (syncObject != null)
                if (syncObject.InvokeRequired)
                {
                    syncObject.Invoke(new MethodInvoker(() => { OnConsoleWritten(e); }),null);
                    return;
                }

            ConsoleWritten?.Invoke(this, e);
        }

        public bool HasConsoleEventHandler
        {
            get { return ConsoleWritten != null; }
        }

        public static void WriteLine(string message)
        {

            if (!ShowDebug && (message.Contains("{BOOP}") || message.StartsWith("{EVENT_")))
                return;

            string consoleLine = "[" + DateTime.Now.ToString("hh:mm:ss.ffff") + "] " + message;

            ConsoleEx.singularity.OnConsoleWritten(new NetCoreEventArgs() { message = new NetCoreSimpleMessage(consoleLine) });

            Console.WriteLine(consoleLine);
        }
    }
}
