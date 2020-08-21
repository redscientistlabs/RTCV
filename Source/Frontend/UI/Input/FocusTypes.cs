namespace RTCV.UI.Input
{
    using System;

    [Flags]
    public enum FocusTypes
    {
        None = 0,
        Mouse = 1,
        Keyboard = 2,
        Pad = 4
    }
}
