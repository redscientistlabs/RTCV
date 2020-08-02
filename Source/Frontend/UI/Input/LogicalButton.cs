namespace RTCV.UI.Input
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using RTCV.NetCore;

    public struct LogicalButton : IEquatable<LogicalButton>
    {
        public LogicalButton(string button, Input.ModifierKeys modifiers)
        {
            Button = button;
            Modifiers = modifiers;
        }

        public readonly string Button;
        public readonly Input.ModifierKeys Modifiers;

        public bool Alt { get { return ((Modifiers & Input.ModifierKeys.Alt) != 0); } }
        public bool Control { get { return ((Modifiers & Input.ModifierKeys.Control) != 0); } }
        public bool Shift { get { return ((Modifiers & Input.ModifierKeys.Shift) != 0); } }

        public override string ToString()
        {
            string ret = "";
            if (Control) ret += "Ctrl+";
            if (Alt) ret += "Alt+";
            if (Shift) ret += "Shift+";
            ret += Button;
            return ret;
        }

        public override bool Equals(object obj)
        {
            var other = (LogicalButton)obj;
            return Equals(other);
        }

        public bool Equals(LogicalButton other)
        {
            return other == this;
        }

        public override int GetHashCode()
        {
            return Button.GetHashCode() ^ Modifiers.GetHashCode();
        }

        public static bool operator ==(LogicalButton lhs, LogicalButton rhs)
        {
            return lhs.Button == rhs.Button && lhs.Modifiers == rhs.Modifiers;
        }

        public static bool operator !=(LogicalButton lhs, LogicalButton rhs)
        {
            return !(lhs == rhs);
        }
    }

}
