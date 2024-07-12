using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VanguardHook
{
    public partial class AnchorForm : Form
    {
        public AnchorForm()
        {
            InitializeComponent();
        }

        private void btnSaveLoad_Click(object sender, EventArgs e)
        {
            VanguardImplementation.ReloadState();
        }
    }
}
