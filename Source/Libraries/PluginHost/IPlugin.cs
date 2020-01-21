using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.PluginHost
{
    public enum RTCSide
    {
        Server,
        Client,
        Both
    }
    public interface IPlugin : IDisposable
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        Version Version { get; }
        RTCSide SupportedSide { get; }


        bool Start(RTCSide side);
        bool Stop();
    }
}
