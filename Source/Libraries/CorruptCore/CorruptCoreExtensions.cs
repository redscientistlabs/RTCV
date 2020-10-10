namespace RTCV.CorruptCore
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using Ceras;

    public static class CorruptCoreExtensions
    {
        public static void DirectoryRequired(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        internal static void DirectoryRequired(string[] paths)
        {
            foreach (var path in paths)
            {
                DirectoryRequired(path);
            }
        }


        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1.
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(this Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = ((255 - red) * correctionFactor) + red;
                green = ((255 - green) * correctionFactor) + green;
                blue = ((255 - blue) * correctionFactor) + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        public static string GetRelativePath(string rootPath, string fullPath)
        {
            var rootPathAsUri = new Uri(rootPath + "\\");
            var fullPathAsUri = new Uri(fullPath);
            Uri diff = rootPathAsUri.MakeRelativeUri(fullPathAsUri);
            return Uri.UnescapeDataString(diff.OriginalString).Replace('/', '\\');
        }
        //https://stackoverflow.com/a/23354773
        public static bool IsOrIsSubDirectoryOf(string candidate, string other)
        {
            var isChild = false;
            try
            {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (true)
                {
                    if (Path.GetFullPath(candidateInfo.FullName + Path.DirectorySeparatorChar) == Path.GetFullPath(otherInfo.FullName + Path.DirectorySeparatorChar))
                    {
                        isChild = true;
                        break;
                    }
                    if (candidateInfo.Parent == null)
                    {
                        break;
                    }

                    candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception error)
            {
                var message = string.Format("Unable to check directories {0} and {1}: {2}", candidate, other, error);
                Trace.WriteLine(message);
            }

            return isChild;
        }
    }

    public static class ObjectCopierCeras
    {
        public static T Clone<T>(T source)
        {
            var ser = new CerasSerializer(new SerializerConfig()
            {
                DefaultTargets = TargetMember.All
            });
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (source == null)
            {
                return default;
            }

            return ser.Deserialize<T>(ser.Serialize(source));
        }
    }

    //Lifted from Bizhawk https://github.com/TASVideos/BizHawk
    public static class NativeWin32APIs
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        internal const int SW_HIDE = 0;
        internal const int SW_SHOW = 5;

        internal const int SC_CLOSE = 0xF060;           //close button's code in Windows API
        internal const int MF_ENABLED = 0x00000000;     //enabled button status
        internal const int MF_GRAYED = 0x1;             //disabled button status (enabled = false)
        internal const int MF_DISABLED = 0x00000002;    //disabled button status

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
