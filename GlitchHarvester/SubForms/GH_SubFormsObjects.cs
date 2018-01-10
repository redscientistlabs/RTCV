using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.GlitchHarvester.SubForms
{

    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool HasCancelButton { get; set; }
        void SubForm_Cancel();
        void SubForm_Ok();
    }
}
