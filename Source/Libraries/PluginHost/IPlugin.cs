namespace RTCV.PluginHost
{
    using System;

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
