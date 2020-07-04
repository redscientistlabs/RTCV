namespace RTCV.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class LocalNetCoreRouter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary<string, IRoutable> endpoints = new Dictionary<string, IRoutable>();
        public static bool HasEndpoints => endpoints.Count > 0;

        public static T registerEndpoint<T>(T endpoint, string name)
        {
            if (endpoint is IRoutable)
            {
                endpoints[name] = (IRoutable)endpoint;
            }
            else
            {
                logger.Fatal($"Error while registering object {endpoint} in Netcore Local Router, does not implement IRoutable");
            }

            return endpoint;
        }

        public static IRoutable getEndpoint(string name)
        {
            try
            {
                endpoints.TryGetValue(name, out IRoutable chosen);
                return chosen;
            }
            catch { return null; }
        }

        public static bool hasEndpoint(string name) => endpoints.TryGetValue(name, out IRoutable chosen);

        public static object Route(string endpointName, string messageType, object objectValue) => Route(endpointName, messageType, objectValue, false);
        public static object Route(string endpointName, string messageType, object objectValue, bool synced)
        {
            NetCoreEventArgs ncea = new NetCoreEventArgs(messageType, objectValue);
            (ncea.message as NetCoreAdvancedMessage).requestGuid = (synced ? (Guid?)Guid.NewGuid() : null);
            return Route(endpointName, ncea);
        }

        public static T QueryRoute<T>(string endpointName, string messageType, object objectValue, bool synced = true)
        {
            NetCoreEventArgs ncea = new NetCoreEventArgs(messageType, objectValue);
            (ncea.message as NetCoreAdvancedMessage).requestGuid = (synced ? (Guid?)Guid.NewGuid() : null);
            var returnValue = Route(endpointName, ncea);
            if (returnValue is NetCoreAdvancedMessage ncam)
            {
                return (T)ncam.objectValue;
            }
            if (returnValue == null)
            {
                return default(T);
            }

            return (T)returnValue;
        }

        public static object Route(string endpointName, string messageType) => Route(endpointName, messageType, false);
        public static object Route(string endpointName, string messageType, bool synced = false)
        {
            var ncea = (synced ? new NetCoreEventArgs() { message = new NetCoreAdvancedMessage(messageType) { requestGuid = Guid.NewGuid() } } : new NetCoreEventArgs(messageType));
            return Route(endpointName, ncea);
        }

        public static T QueryRoute<T>(string endpointName, string messageType, bool synced = true)
        {
            var ncea = (synced ? new NetCoreEventArgs() { message = new NetCoreAdvancedMessage(messageType) { requestGuid = Guid.NewGuid() } } : new NetCoreEventArgs(messageType));
            var returnValue = Route(endpointName, ncea);
            if (returnValue is NetCoreAdvancedMessage ncam)
            {
                return (T)ncam.objectValue;
            }
            return (T)returnValue;
        }

        public static object Route(string endpointName, NetCoreEventArgs e)
        {
            try
            {
                var endpoint = getEndpoint(endpointName);
                if (endpoint == null)
                {
                    var defaultEndpoint = getEndpoint("DEFAULT");
                    if (defaultEndpoint != null)
                    {
                        e.message.Type = endpointName + "|" + e.message.Type;
                        return defaultEndpoint.OnMessageReceived(null, e);
                    }
                }
                return endpoint.OnMessageReceived(null, e);
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex) == DialogResult.Abort)
                {
                    throw new RTCV.NetCore.AbortEverythingException();
                }

                return null;
            }
        }
    }

    public interface IRoutable
    {
        object OnMessageReceived(object sender, NetCoreEventArgs e);
    }
}
