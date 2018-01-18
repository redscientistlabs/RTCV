using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.Vanguard
{
    public class VanguardConnector
    {
        TargetSpec spec;

        public VanguardConnector(TargetSpec _spec)
        {
            spec = _spec;

        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {



            return null;
        }

        public void Kill()
        {

        }
    }
}
