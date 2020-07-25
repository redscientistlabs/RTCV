namespace RTCV.UI
{
    using System.IO;
    using System.Windows.Forms;

    internal static class Common
    {
        internal static bool CopyFile(string sourcePath, string targetDirectory, bool confirmOverwrite)
        {
            var shortPath = sourcePath.Substring(sourcePath.LastIndexOf('\\') + 1);
            var targetPath = Path.Combine(targetDirectory, shortPath);
            return ReplaceFile(sourcePath, targetPath, confirmOverwrite);
        }

        internal static bool ReplaceFile(string sourcePath, string targetPath, bool confirmOverwrite = false)
        {
            if (File.Exists(targetPath))
            {
                if (confirmOverwrite)
                {
                    var result = MessageBox.Show("This file already exist in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                        return true;
                }

                File.Delete(targetPath);
            }

            File.Copy(sourcePath, targetPath);

            return false;
        }
    }
}
