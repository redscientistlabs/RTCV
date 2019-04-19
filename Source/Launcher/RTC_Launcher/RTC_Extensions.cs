using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace RTC_Launcher
{
    public static class RTC_Extensions
    {
        public static DialogResult getInputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }


        #region ARRAY EXTENSIONS
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            T[] result = new T[length];

            if (data == null)
                return null;

            Array.Copy(data, index, result, 0, length);
            return result;
        }
        #endregion

        #region STRING EXTENSIONS
        public static string ToBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        #endregion

        #region COLOR EXTENSIONS

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
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        #endregion

        #region CONTROL EXTENSIONS

        public static List<Control> getControlsWithTag(this Control.ControlCollection controls)
        {
            List<Control> allControls = new List<Control>();

            foreach (Control c in controls)
            {
                if (c.Tag != null)
                    allControls.Add(c);

                if (c.HasChildren)
                    allControls.AddRange(c.Controls.getControlsWithTag()); //Recursively check all children controls as well; ie groupboxes or tabpages
            }

            return allControls;
        }

        #endregion
    }

    // Used code from this https://github.com/wasabii/Cogito/blob/master/Cogito.Core/RandomExtensions.cs
    // MIT Licensed. thank you very much.
    static class RandomExtensions
    {
        public static long RandomLong(this Random rnd)
        {
            byte[] buffer = new byte[8];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long RandomLong(this Random rnd, long min, long max)
        {
            EnsureMinLEQMax(ref min, ref max);
            long numbersInRange = unchecked(max - min + 1);
            if (numbersInRange < 0)
                throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

            long randomOffset = RandomLong(rnd);
            if (IsModuloBiased(randomOffset, numbersInRange))
                return RandomLong(rnd, min, max); // Try again
            else
                return min + PositiveModuloOrZero(randomOffset, numbersInRange);
        }

        public static long RandomLong(this Random rnd, long max)
        {
            return rnd.RandomLong(0, max);
        }

        static bool IsModuloBiased(long randomOffset, long numbersInRange)
        {
            long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
            return randomOffset > greatestCompleteRange;
        }

        static long PositiveModuloOrZero(long dividend, long divisor)
        {
            long mod;
            Math.DivRem(dividend, divisor, out mod);
            if (mod < 0)
                mod += divisor;
            return mod;
        }

        static void EnsureMinLEQMax(ref long min, ref long max)
        {
            if (min <= max)
                return;
            long temp = min;
            min = max;
            max = temp;
        }
    }

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }




    }
}
