using System;   
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;

namespace RTCV.UI.Components.Controls
{
    public partial class SavestateHolder : UserControl
    {
        private StashKey _sk = null;
        public StashKey sk { get => _sk; }

        public SavestateHolder(int num)
        {
            InitializeComponent();
            btnSavestate.Text = num.ToString();
            tbSavestate.Visible = false;
        }

        public void SetStashKey(StashKey sk, String name, int number)
        {
            this._sk = sk;
            if (name == null)
                tbSavestate.Visible = false;
            else
            {
                tbSavestate.Text = name;
                tbSavestate.Visible = true;
            }
            btnSavestate.Text = number.ToString();
            this.Invalidate();
        }

        public void SetSelected(bool selected)
        {
            btnSavestate.ForeColor = selected ? Color.OrangeRed : Color.PaleGreen;
        }

    }
}
