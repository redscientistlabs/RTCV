using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Media;

namespace RTC
{
    public partial class AutoKillSwitch : Form
    {
        Timer t;
        SoundPlayer simpleSound = new SoundPlayer("RTC\\ASSETS\\crash.wav");

        public AutoKillSwitch()
        {
            InitializeComponent();
        }

        private void AutoKillSwitch_Load(object sender, EventArgs e)
        {
            cbDetection.SelectedIndex = 0;

            t = new Timer();
            t.Interval = 300;
            t.Tick += new EventHandler(CheckHeartbeat);
            t.Start();

            RTC_RPC.Start();
        }

        private void CheckHeartbeat(object sender, EventArgs e)
        {
            if ((pbTimeout.Value == pbTimeout.Maximum && !RTC_RPC.Heartbeat) || RTC_RPC.Freeze || !cbEnabled.Checked)
                return;

            if (!RTC_RPC.Heartbeat)
            {
                pbTimeout.PerformStep();

                if (pbTimeout.Value == pbTimeout.Maximum)
                {
                    //this.Focused = false;
                    btnKillAndRestart_Click(sender, e);
                    //this.Focused = false;
                }


            }
            else
            {
                pbTimeout.Value = 0;
                RTC_RPC.Heartbeat = false;
            }

            

        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            simpleSound.Play();
            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitch.bat");
            RTC_RPC.Freeze = true;

        }

        private void btnKillAndRestart_Click(object sender, EventArgs e)
        {
            simpleSound.Play();
            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitchRestart.bat");
            RTC_RPC.Freeze = true;
        }

        private void btnKillResetAndRestart_Click(object sender, EventArgs e)
        {
            if (File.Exists("RTC\\SESSION\\Restore.dat"))
                File.Delete("RTC\\SESSION\\Restore.dat");

            if (File.Exists("RTC\\SESSION\\WindowRestore.dat"))
                File.Delete("RTC\\SESSION\\WindowRestore.dat");

            if(sender != null)
                simpleSound.Play();

            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitchRestart.bat");
            RTC_RPC.Freeze = true;

        }

        private void cbDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbDetection.SelectedItem.ToString())
            {
                case "VIOLENT":
                    pbTimeout.Maximum = 4;
                    break;
                case "HEAVY":
                    pbTimeout.Maximum = 6;
                    break;
                case "MILD":
                    pbTimeout.Maximum = 10;
                    break;
                case "SLOPPY":
                    pbTimeout.Maximum = 15;
                    break;
                case "COMATOSE":
                    pbTimeout.Maximum = 30;
                    break;
            }
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbEnabled.Checked)
                RTC_RPC.Freeze = true;
            else
                RTC_RPC.Freeze = false;
        }


    }
}
