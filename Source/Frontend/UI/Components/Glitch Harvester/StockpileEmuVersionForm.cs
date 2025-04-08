using RTCV.CorruptCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.UI
{
    public partial class StockpileEmuVersionForm : Form
    {
        public string SelectedVersion;
        public StockpileEmuVersionForm(bool detected = true)
        {
            InitializeComponent();

            string detected_text = "RTC has detected this stockpile does not have an associated emulator system and version. Please select the system and version this stockpile was created in.";
            string default_text = "Please select the system and version the selected stockpile items were created in.";


            label1.Text = detected ? detected_text : default_text;

            var directories = Directory.GetDirectories(new DirectoryInfo(RtcCore.RtcDir).Parent.Parent.FullName);
            foreach(var dir in directories)
            {
                var dirName = new DirectoryInfo(dir).Name;
                if (dirName != "Launcher" && dirName != "RTCV")
                { 
                    EmuVersionDropDown.Items.Add(dirName);
                }
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            EmuVersionDropDown.DroppedDown = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedVersion = (string)EmuVersionDropDown.SelectedItem;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
