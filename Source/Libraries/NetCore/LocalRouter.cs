namespace RTCV.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public static class LocalNetCoreRouter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary<string, IRoutable> endpoints = new Dictionary<string, IRoutable>();
        public static bool HasEndpoints => endpoints.Count > 0;

        public static T registerEndpoint<T>(T endpoint, string name)
        {
            if (endpoint is IRoutable routable)
            {
                endpoints[name] = routable;
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


        //Command Calls
        public static async Task RouteAsync(string endpointName, string messageType, object objectValue) => await Task.Run(() => Route(endpointName, messageType, objectValue, true));

        public static object Route(string endpointName, string messageType, object objectValue, bool synced = false)
        {
            NetCoreEventArgs ncea = new NetCoreEventArgs(messageType, objectValue);
            (ncea.message as NetCoreAdvancedMessage).requestGuid = (synced ? (Guid?)Guid.NewGuid() : null);
            return Route(endpointName, ncea);
        }

        public static async Task RouteAsync(string endpointName, string messageType) => await Task.Run(() => Route(endpointName, messageType, true));
        public static object Route(string endpointName, string messageType, bool synced = false)
        {
            NetCoreEventArgs ncea = (synced ? new NetCoreEventArgs() { message = new NetCoreAdvancedMessage(messageType) { requestGuid = Guid.NewGuid() } } : new NetCoreEventArgs(messageType));
            return Route(endpointName, ncea);
        }


        //Query Calls
        public static async Task<T> QueryRouteAsync<T>(string endpointName, string messageType, object objectValue = null) => await Task<T>.Run(() => QueryRoute<T>(endpointName, messageType, objectValue));
        public static T QueryRoute<T>(string endpointName, string messageType, object objectValue = null)
        {
            var ncea = new NetCoreEventArgs() {
                message = new NetCoreAdvancedMessage(messageType) {
                    requestGuid = Guid.NewGuid(),
                    objectValue = objectValue
                }
            };
            var returnValue = Route(endpointName, ncea);
            if (returnValue is NetCoreAdvancedMessage ncam)
            {
                return (T)ncam.objectValue;
            }
            return (T)returnValue;
        }

        //Master routing call
        public static object Route(string endpointName, NetCoreEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

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
                    throw new AbortEverythingException();
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
