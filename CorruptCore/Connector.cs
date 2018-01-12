using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
    public class CorruptCoreConnector : IRoutable
    {
        public IRoutable vanguard;

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {





            return null;
        }

        public void Kill()
        {

        }
    }
}
