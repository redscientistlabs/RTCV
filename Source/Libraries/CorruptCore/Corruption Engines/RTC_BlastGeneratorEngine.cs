
namespace RTCV.CorruptCore
{
	public static class RTC_BlastGeneratorEngine
	{
		public static BlastUnit GetUnit()
		{
			return null;
		}

		public static BlastLayer GetBlastLayer()
		{
			return NetCore.LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.NetcoreCommands.UI, NetCore.NetcoreCommands.REMOTE_GETBLASTGENERATOR_LAYER, true);
		}
	}
}
