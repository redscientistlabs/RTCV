namespace RTCV.Plugins.HexEditor
{
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;

    public class HexEditorConnector : IRoutable
    {
        public HexEditorConnector()
        {
            LocalNetCoreRouter.registerEndpoint(this, "HEXEDITOR");
        }
        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            //Use setReturnValue to handle returns
            var message = e.message;
            var advancedMessage = message as NetCoreAdvancedMessage;

            switch (e.message.Type)
            {
                case NetcoreCommands.REMOTE_OPENHEXEDITOR:
                    {
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            if (S.GET<HexEditor>().IsDisposed)
                            {
                                S.SET(new HexEditor());
                            }
                            S.GET<HexEditor>().Restart();
                            S.GET<HexEditor>().Show();
                        });
                    }
                    break;

                case NetcoreCommands.EMU_OPEN_HEXEDITOR_ADDRESS:
                    {
                        var temp = advancedMessage.objectValue as object[];
                        string domain = (string)temp[0];
                        long address = (long)temp[1];

                        MemoryInterface mi = MemoryDomains.GetInterface(domain);
                        if (mi == null)
                            break;

                        SyncObjectSingleton.FormExecute(() =>
                        {
                            if (S.GET<HexEditor>().IsDisposed)
                            {
                                S.SET(new HexEditor());
                            }
                            S.GET<HexEditor>().Restart();
                            S.GET<HexEditor>().Show();
                            S.GET<HexEditor>().SetDomain(mi);
                            S.GET<HexEditor>().GoToAddress(address);
                        });
                    }
                    break;
            }
            return e.returnMessage;
        }
    }
}
