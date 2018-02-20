using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.TestForm
{
    public partial class TestMainForm : Form
    {
        NetCoreConnector connector = null;
        bool IsClient;

        private bool UseLoopback = true;

        public TestMainForm(bool _IsClient = true)
        {
            IsClient = _IsClient;
            InitializeComponent();

            Text += " " + (IsClient ? "CLIENT" : "SERVER");
        }

        private void btnKillNetCore_Click(object sender, EventArgs e)
        {
            KillClient();
        }

        public void KillClient()
        {
            if (connector == null)
            {
                MessageBox.Show("connector is null");
                return;
            }

            connector?.Kill();
            connector = null;
        }

        private void rbConnectLoopback_CheckedChanged(object sender, EventArgs e)
        {
            UseLoopback = rbConnectLoopback.Checked;
        }

        private void rbConnectTextbox_CheckedChanged(object sender, EventArgs e)
        {
            UseLoopback = rbConnectLoopback.Checked;
        }

        private void btnStartNetCore_Click(object sender, EventArgs e)
        {
            if(connector != null)
            {
                MessageBox.Show("Connector not null");
                return;
            }

            string target = tbCustomTarget.Text;
            string port = tbPort.Text;

            NetCoreSpec spec = new NetCoreSpec();

            spec.syncObject = this;

            //ConsoleEx.singularity.ConsoleWritten += Singularity_ConsoleWritten;
            ConsoleEx.singularity.Register((ob, ea) => Singularity_ConsoleWritten(ob,ea), this);

            spec.Side = (IsClient ? NetworkSide.CLIENT : NetworkSide.SERVER);
            spec.Port = Convert.ToInt32(port);
            spec.Loopback = UseLoopback;

            if(!UseLoopback)
                spec.IP = target;

            spec.MessageReceived += OnMessageReceived;

            connector = new NetCoreConnector(spec);


           
        }

        private void Singularity_ConsoleWritten(object sender, NetCoreEventArgs e)
        {
            if(cbDisplayConsole.Checked)
                tbNetCoreOutput.AppendText(e.message.Type + "\n");
        }

        public void StartClient()
        {

            var spec = new NetCoreSpec();
            spec.Side = NetworkSide.CLIENT;
            spec.MessageReceived += OnMessageReceived;
            connector = new NetCoreConnector(spec);
        }

        public void RestartClient()
        {
            if (connector == null)
            {
                MessageBox.Show("connector is null");
                return;
            }

            connector.Kill();
            connector = null;
            StartClient();
        }

        private void OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            // This is where you implement interaction.
            // Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

            var message = e.message;
            var simpleMessage = message as NetCoreSimpleMessage;
            var advancedMessage = message as NetCoreAdvancedMessage;

            switch (message.Type) //Handle received messages here
            {

                case "{EVENT_NETWORKSTATUS}": //status update broadcast (string)
                    lbNetCoreStatus.Text = $"Status : {(advancedMessage.objectValue as string)?.ToString()}";
                    break;



                default:
                    ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
                    break;
            }

        }

        private void btnStopNetCore_Click(object sender, EventArgs e)
        {
            if (connector == null)
            {
                MessageBox.Show("connector is null");
                return;
            }

            connector?.Stop();
            connector = null;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartClient();
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            tbNetCoreOutput.Clear();
        }
    }
}
