using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTCV;
using RTCV.NetCore;
using RTCV.DolphinCorrupt;
using WindowsGlitchHarvester;

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
                    {
                        string path = advancedMessage.objectValue as string;
                        NetCoreEventArgs args = new NetCoreEventArgs();
                        args.message = new NetCoreAdvancedMessage("LOADSTATE", path);
                        NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);
                        break;
                    }

                case "SAVESTATE":
                    {
                        string path = advancedMessage.objectValue as string;
                        NetCoreEventArgs args = new NetCoreEventArgs();
                        args.message = new NetCoreAdvancedMessage("SAVESTATE", path);
                        NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);
                        break;
                    }

                case "PEEKBYTE":
                    {
                        long address = Convert.ToInt64(advancedMessage.objectValue);
                        NetCoreEventArgs args = new NetCoreEventArgs();
                        args.message = new NetCoreAdvancedMessage("PEEKBYTE", address);
                        NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);
                        e.setReturnValue(((NetCoreAdvancedMessage)(args.returnMessage)).objectValue);
                        break;
                    }

                case "BLASTLAYER":
                    {
                        BlastLayer bl = (BlastLayer)advancedMessage.objectValue;
                        foreach(BlastUnit bu in bl.Layer)
                        {
                            if (bu is BlastByte bb)
                            {
                                NetCoreEventArgs args = new NetCoreEventArgs();
                                Object[] parameters = new Object[2];
                                parameters[0] = bb.Address;
                                parameters[1] = bb.Value;

                                args.message = new NetCoreAdvancedMessage("POKEBYTE", parameters);
                                NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);
                            }
                            else if (bu is BlastVector bv)
                            {

                                NetCoreEventArgs args = new NetCoreEventArgs();
                                Object[] parameters = new Object[3];
                                parameters[0] = bv.Address;
                                parameters[1] = 4;
                                parameters[2] = bv.Values;
                                args.message = new NetCoreAdvancedMessage("POKEBYTES", parameters);
                                NetCore.LocalNetCoreRouter.Route("VANGUARD", this, args);

                            }

                        }
                        break;
                    }

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
