namespace RTCV.UI.Extensions
{
    using RTCV.CorruptCore;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public static class ToStringExtensions
    {
        public static string ToString(this string[] obj)
        {
            string quoteme(string v) => $"\"{v}\"";
            return string.Join(", ", obj.Select(it => quoteme(it)));
        }

        public static string ToString(this MemoryDomainProxy[] obj)
        {
            string quoteme(string v) => $"\"{v}\"";
            return string.Join(", ", obj.Select(it => quoteme(it.Name)));
        }
    }
}
