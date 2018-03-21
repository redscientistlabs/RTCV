using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.NetCore
{
    public static class LocalNetCoreRouter
    {
        private static Dictionary<string, IRoutable> endpoints = new Dictionary<string, IRoutable>();
        public static bool HasEndpoints { get { return endpoints.Count > 0; } }

        public static T registerEndpoint<T>(T endpoint, string name)
        {
            if (endpoint is IRoutable)
                endpoints[name] = (IRoutable)endpoint;
            else
                ConsoleEx.WriteLine($"Error while registering object {endpoint} in Netcore Local Router, does not implement IRoutable");

            return endpoint;
        }
        public static IRoutable getEndpoint(string name)
        {
            try
            {
                IRoutable chosen = endpoints[name];
                return chosen;
            }
            catch { return null; }
        }

        public static object Route(string endpointName, object sender, NetCoreEventArgs e)
        {
            var endpoint = getEndpoint(endpointName);
            if (endpoint == null)
            {
                ConsoleEx.WriteLine($"Error in NetCore Local Router, could not route message to endpoint {endpointName}");
                return null;
            }

            return endpoint.OnMessageReceived(sender, e);
        }
    }

    public interface IRoutable
    {
        object OnMessageReceived(object sender, NetCoreEventArgs e);
    }
}
