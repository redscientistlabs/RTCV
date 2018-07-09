using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV;
using RTCV.NetCore;

namespace RTCV.DolphinCorrupt
{
    public class DolphinCorruptConnector : IRoutable
    {
        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            //Use setReturnValue to handle returns

            var message = e.message;
            var simpleMessage = message as NetCore.NetCoreSimpleMessage;
            var advancedMessage = message as NetCore.NetCoreAdvancedMessage;

            switch (message.Type)
            {
                //HANDLE MESSAGES HERE

                case "LOADSTATE":
                    string path = advancedMessage.objectValue as string;
                    NetCoreEventArgs args = new NetCoreEventArgs();
                    args.message = new NetCoreAdvancedMessage("LOADSTATE", path);
                    NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);
                    break;


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
