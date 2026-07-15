namespace RTCV.UI
{
    using RTCV.CorruptCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    internal static class Common
    {
        internal static void CopyFile(string sourcePath, string targetDirectory, string folderName = null, bool confirmOverwrite = true)
        {
            string shortPath = sourcePath.Substring(sourcePath.LastIndexOf('\\') + 1);
            string targetPath = Path.Combine(targetDirectory, shortPath);
            string destinationName = new DirectoryInfo(targetDirectory).Name;
            ReplaceFile(sourcePath, targetPath, folderName ?? destinationName, confirmOverwrite);
        }

        internal static void ReplaceFile(string sourcePath, string targetPath, string folderName = null, bool confirmOverwrite = false)
        {
            string destinationName = new DirectoryInfo(Path.GetDirectoryName(targetPath)!).Name;
            folderName ??= destinationName;
            if (File.Exists(targetPath))
            {
                if (confirmOverwrite)
                {
                    var result = MessageBox.Show($"This file already exists in your {folderName} folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        throw new OverwriteCancelledException();
                    }
                }

                File.Delete(targetPath);
            }

            File.Copy(sourcePath, targetPath);
        }

        // Check if any emulators in the savestates file are not installed, and prompt the user
        // to select an emu version if it's a legacy savestates file.
        internal static bool CheckForEmulators(List<StashKey> sks)
        {
            List<string> missingEmulators = new List<string> { };
            bool missingEmuVer = false;
            foreach (StashKey key in sks)
            {
                if (key.EmuVer != "")
                {
                    string emulatorPath = Path.Combine(RtcCore.RtcDir, "..\\..\\", key.EmuVer);
                    if (!Directory.Exists(emulatorPath) && !missingEmulators.Contains(key.EmuVer))
                        missingEmulators.Add(key.EmuVer);
                }
                // Update stashkey emulator version if it's empty
                else
                {
                    missingEmuVer = true;
                }
            }

            if (missingEmulators.Count > 0)
            {
                string missingEmulatorsString = "";
                foreach (string emulator in missingEmulators)
                {
                    missingEmulatorsString += emulator + "\n";
                }
                string missingEmulatorsMessage = "You are missing the following emulators used in this savestates file: \n\n" +
                                                  String.Join(Environment.NewLine, missingEmulatorsString + "\n" +
                                                  "Please install these emulators and then load the savestates file again.");
                MessageBox.Show(missingEmulatorsMessage, "Operation cancelled", MessageBoxButtons.OK);
                return false;
            }
            else if (missingEmuVer)
            {
                var form = new UpdateEmuVersionForm();

                // start/show the control
                form.ShowDialog();

                if (form.SelectedVersion != null)
                {
                    foreach (StashKey key in sks)
                    {
                        key.EmuVer = form.SelectedVersion;
                    }
                }
                else
                {
                    MessageBox.Show("Emulator system and version selection was cancelled, the savestates file will not be loaded.", "Operation cancelled", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
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
