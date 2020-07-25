namespace RTCV.UI
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Runtime.Serialization;

    internal static class Common
    {
        internal static void CopyFile(string sourcePath, string targetDirectory, bool confirmOverwrite)
        {
            var shortPath = sourcePath.Substring(sourcePath.LastIndexOf('\\') + 1);
            var targetPath = Path.Combine(targetDirectory, shortPath);
            ReplaceFile(sourcePath, targetPath, confirmOverwrite);
        }

        internal static void ReplaceFile(string sourcePath, string targetPath, bool confirmOverwrite = false)
        {
            if (File.Exists(targetPath))
            {
                if (confirmOverwrite)
                {
                    var result = MessageBox.Show("This file already exist in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                        throw new OverwriteCancelledException();
                }

                File.Delete(targetPath);
            }

            File.Copy(sourcePath, targetPath);
        }

        [Serializable]
        public class OverwriteCancelledException : Exception
        {
            public OverwriteCancelledException() : base("File overwrite operation cancelled by user")
            {
            }

            public OverwriteCancelledException(string message) : base(message)
            {
            }

            public OverwriteCancelledException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected OverwriteCancelledException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
