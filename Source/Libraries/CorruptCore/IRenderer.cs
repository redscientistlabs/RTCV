namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRenderer
    {
        public bool SupportsFormat(string format);
        public void StartRendering();
        public void StopRender();
    }
}
