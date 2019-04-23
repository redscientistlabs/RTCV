using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class Render
	{
        public static bool RenderAtLoad
        {
            get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_AT_LOAD.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_AT_LOAD.ToString(), value);
        }

        public static bool IsRendering
		{
			get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_ISRENDERING.ToString()];
			set
            {
                RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_ISRENDERING.ToString(), value);
                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_RENDER_DISPLAY);
            }
		}
		
		public static RENDERTYPE RenderType
		{
			get => (RENDERTYPE)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_RENDERTYPE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_RENDERTYPE.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");
			partial[RTCSPEC.RENDER_ISRENDERING.ToString()] = false;
			partial[RTCSPEC.RENDER_RENDERTYPE.ToString()] = RENDERTYPE.WAV;
			return partial;
		}


		public static bool StartRender()
		{
			if (!((bool?) AllSpec.VanguardSpec[VSPEC.SUPPORTS_RENDERING] ?? false))
			{
				MessageBox.Show("Rendering isn't supported by this Emulator");
				return false;
            }
                

			if (IsRendering)
				StopRender();

			IsRendering = true;
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RENDER_START, true);
            return true;
		}

		public static void StopRender()
		{
			IsRendering = false;
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RENDER_STOP, true);
		}

		public enum RENDERTYPE
		{
			NONE,
			WAV,
			AVI,
			MPEG,
			LAST,
		}
	}
}
