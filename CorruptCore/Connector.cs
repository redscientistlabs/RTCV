using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
    public class CorruptCoreConnector : IRoutable
    {
        public FullSpec vanguardSpec;

        public CorruptCoreConnector(FullSpec _vanguardSpec)
        {
            vanguardSpec = _vanguardSpec;
            vanguardSpec.RegisterUpdateAction((ob, ea) => {
                
                //This will be triggerred if the vanguardSpec gets updated.
                //new memory domains or whatever.

            });

        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            //Use setReturnValue to handle returns

            switch (e.message.Type)
            {
                //HANDLE MESSAGES HERE
                default:
                    new object();
                    break;
            }

            return e.returnMessage;
        }


        public void Kill()
        {

        }
    }
}
