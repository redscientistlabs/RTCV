using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.GlitchHarvester.TileForms
{
    public interface ITileForm
    {
        //Interface used for added contrals in TileForms

        int TilesX { get; set; }
        int TilesY { get; set; }
        bool CanPopout { get; set; }

    }

}
