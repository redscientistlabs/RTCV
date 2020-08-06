namespace RTCV.CorruptCore
{
    using System.Windows.Forms;
    using RTCV.NetCore;

    public static class Render
    {
        public static bool RenderAtLoad
        {
            get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_AT_LOAD];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_AT_LOAD, value);
        }

        public static bool IsRendering
        {
            get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_ISRENDERING];
            set
            {
                RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_ISRENDERING, value);
                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_RENDER_DISPLAY);
            }
        }

        public static RENDERTYPE RenderType
        {
            get => (RENDERTYPE)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_RENDERTYPE];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_RENDERTYPE, value);
        }

        public static IRenderer Renderer = null;

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial[RTCSPEC.RENDER_ISRENDERING] = false;
            partial[RTCSPEC.RENDER_RENDERTYPE] = RENDERTYPE.WAV;
            return partial;
        }


        public static bool StartRender()
        {
            bool vanguardSupportsRendering = ((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_RENDERING] ?? false);

            if (!vanguardSupportsRendering && Renderer == null)
            {
                MessageBox.Show("Rendering isn't supported by this Emulator");
                return false;
            }


            if (IsRendering)
            {
                if (vanguardSupportsRendering)
                    StopRender();
                else if (Renderer != null)
                    Renderer.StopRender();
            }

            IsRendering = true;

            if (vanguardSupportsRendering)
                LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RENDER_START, true);
            else
                Renderer.StopRender();


            return true;
        }

        public static void StopRender()
        {
            bool vanguardSupportsRendering = ((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_RENDERING] ?? false);

            IsRendering = false;

            if (vanguardSupportsRendering)
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
