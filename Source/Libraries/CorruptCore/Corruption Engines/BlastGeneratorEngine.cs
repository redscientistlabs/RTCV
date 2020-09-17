namespace RTCV.CorruptCore
{
    public static class BlastGeneratorEngine
    {
        public static BlastUnit GetUnit()
        {
            return null;
        }

        public static BlastLayer GetBlastLayer()
        {
            return NetCore.LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.Endpoints.UI, NetCore.Commands.Remote.GetBlastGeneratorLayer, true);
        }
    }
}
