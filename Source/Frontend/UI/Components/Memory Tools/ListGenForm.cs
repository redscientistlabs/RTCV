namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.CorruptCore.Extensions;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class ListGenForm : ComponentForm, IAutoColorize, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public ListGenForm()
        {
            InitializeComponent();
        }

        private static ulong SafeStringToULongHex(string input)
        {
            if (input.IndexOf("0X", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ulong.Parse(input.Substring(2), NumberStyles.HexNumber);
            }
            else
            {
                return ulong.Parse(input, NumberStyles.HexNumber);
            }
        }

        private static bool IsHex(string str)
        {
            //Hex characters
            //Trim the 0x off
            string regex = "^((0[Xx])|([xX]))[0-9A-Fa-f]+$";
            return Regex.IsMatch(str, regex);
        }

        private static bool IsWholeNumber(string str)
        {
            string regex = "^[0-9]+$";
            return Regex.IsMatch(str, regex);
        }

        private static bool IsDecimalNumber(string str)
        {
            string regex = "^(\\d*\\.){1}\\d+$";
            return Regex.IsMatch(str, regex);
        }

        public static T Convert<T>(string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                return (T)converter.ConvertFromString(input);
            }
            return default(T);
        }

        private void GenerateList(object sender, EventArgs e)
        {
            if (tbListValues.Lines.Length == 0)
            {
                return;
            }

            List<string> newList = new List<string>();
            foreach (string line in tbListValues.Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string trimmedLine = line.Trim();

                string[] lineParts = trimmedLine.Split('-');

                //We can't do a range on anything besides plain old numbers
                if (lineParts.Length > 1)
                {
                    //Hex
                    if (IsHex(lineParts[0]) && IsHex(lineParts[1]))
                    {
                        ulong start = SafeStringToULongHex(lineParts[0]);
                        ulong end = SafeStringToULongHex(lineParts[1]);

                        for (ulong i = start; i < end; i++)
                        {
                            newList.Add(i.ToString("X"));
                        }
                    }
                    //Decimal
                    else if (IsWholeNumber(lineParts[0]) && IsWholeNumber(lineParts[1]))
                    {
                        ulong start = ulong.Parse(lineParts[0]);
                        ulong end = ulong.Parse(lineParts[1]);

                        for (ulong i = start; i < end; i++)
                        {
                            newList.Add(i.ToString("X"));
                        }
                    }
                }
                else
                {
                    //If it's not a range we parse for both prefixes and suffixes then see the type
                    if (IsHex(trimmedLine)) //Hex with 0x prefix
                    {
                        newList.Add(lineParts[0].Substring(2));
                    }
                    else if (Regex.IsMatch(trimmedLine, "^[0-9]+[fF]$")) //123f float
                    {
                        float f = Convert<float>(trimmedLine.Substring(0, trimmedLine.Length - 1));
                        byte[] t = BitConverter.GetBytes(f);
                        newList.Add(ByteArrayExtensions.BytesToHexString(t));
                    }
                    else if (Regex.IsMatch(trimmedLine, "^[0-9]+[dD]$")) //123d double
                    {
                        double d = Convert<double>(trimmedLine.Substring(0, trimmedLine.Length - 1));
                        byte[] t = BitConverter.GetBytes(d);
                        newList.Add(ByteArrayExtensions.BytesToHexString(t));
                    }
                    else if (IsDecimalNumber(trimmedLine)) //double no suffix
                    {
                        double d = Convert<double>(trimmedLine);
                        byte[] t = BitConverter.GetBytes(d);
                        newList.Add(ByteArrayExtensions.BytesToHexString(t));
                    }
                    else if (IsWholeNumber(trimmedLine)) //plain old number
                    {
                        newList.Add(ulong.Parse(trimmedLine).ToString("X"));
                    }
                }
            }

            string filename = StringExtensions.MakeSafeFilename(tbListName.Text, '-');
            //Handle saving the list to a file
            if (cbSaveFile.Checked)
            {
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    File.WriteAllLines(Path.Combine(RtcCore.RtcDir, "LISTS", filename + ".txt"), newList);
                }
                else
                {
                    MessageBox.Show("Filename is empty. Unable to save your list to a file");
                }
            }

            //If there's no name just generate one
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = RtcCore.GetRandomKey();
            }

            //TODO fix this before i forget

            IListFilter list;

            if (newList.Contains("?"))
            {
                list = new NullableByteArrayList();
            }
            else
            {
                list = new ValueByteArrayList();
            }

            list.Initialize(filename + ".txt", newList.ToArray(), false, false);
            Filtering.RegisterList(list, filename, true);
            var hash = list.GetHash();

            //Register the list in the ui
            Filtering.RegisterListInUI(filename, hash);

            tbListValues.Clear();
        }

        private void RefreshListsFromFile(object sender, EventArgs e)
        {
            UICore.LoadLists(RtcCore.ListsDir);
            UICore.LoadLists(Path.Combine(RtcCore.EmuDir, "LISTS"));
        }

        private void ShowHelpMessage(object sender, EventArgs e)
        {
            MessageBox.Show(
@"List Generator instructions help and examples
-----------------------------------------------
A whole number will be treated as decimal.
A number prefixed with '0x' will be treated as hex.
You can use a range of these two types.

	A number with a decimal point will be treated as a double.
A number with the suffix 'd' will be treated as a double.
A number with the suffix 'f' will be treated as a float.


Examples:
8-11 -----> 8,9,A
0x8-0x11 -> 8,9,A,B,C,D,E,F,10
10 -------> A
0x10 -----> 10
1.0	------> 000000000000F03F
1d -------> 000000000000F03F
1f -------> 0000803F

> Ranges are exclusive, meaning that the last
	address is excluded from the range.");
        }
    }
}
