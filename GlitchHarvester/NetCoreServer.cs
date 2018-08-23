using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.UI
{
    public static class NetCoreServer
    {
        static NetCore.NetCoreConnector loopbackConnector = null;
        static NetCore.NetCoreConnector multiplayerConnector = null;
        
        public static void StartLoopback()
        {
            var spec = new NetCore.NetCoreSpec();
            //spec.Side = NetCore.NetworkSide.SERVER;
            //spec.Loopback = true;
            //spec.IP = "";
            //spec.Port = 42069;
            spec.MessageReceived += OnMessageReceived;
            loopbackConnector = new NetCore.NetCoreConnector(spec);
        }

        public static void RestartLoopback()
        {
            loopbackConnector.Kill();
            loopbackConnector = null;
            StartLoopback();
        }

        public static void StartMultiplayer(int _Port)
        {
            var spec = new NetCore.NetCoreSpec();
            //spec.Side = NetCore.NetworkSide.SERVER;
            spec.Loopback = false;
            //spec.IP = "";
            spec.Port = _Port;

            multiplayerConnector = new NetCore.NetCoreConnector(spec);
        }

        private static void OnMessageReceived(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
