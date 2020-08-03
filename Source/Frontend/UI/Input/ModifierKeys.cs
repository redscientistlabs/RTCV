namespace RTCV.UI.Input
{
    using System;

    [Flags]
    public enum ModifierKeys
    {
        // Summary:
        //     The bitmask to extract modifiers from a key value.
        Modifiers = -65536,

        // Summary:
        //     No key pressed.
        None = 0,

        // Summary:
        //     The SHIFT modifier key.
        Shift = 65536,

        // Summary:
        //     The CTRL modifier key.
        Control = 131072,

        // Summary:
        //     The ALT modifier key.
        Alt = 262144,
    }
}
