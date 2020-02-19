namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.CorruptCore;

    public partial class SavestateHolder : UserControl
    {
        private StashKey _sk = null;
        public StashKey sk => _sk;

        private SaveStateKey ssk = null;

        public SavestateHolder(int num)
        {
            InitializeComponent();
            btnSavestate.Text = num.ToString();
            tbSavestate.Visible = false;
        }

        public void SetStashKey(SaveStateKey key, int number)
        {
            ssk = key;
            _sk = key?.StashKey;
            if (key?.Text == null)
            {
                tbSavestate.Visible = false;
            }
            else
            {
                tbSavestate.Text = key.Text;
                tbSavestate.Visible = true;
            }
            btnSavestate.Text = number.ToString();
            tbSavestate.TextChanged += TbSavestate_TextChanged;
        }

        private void TbSavestate_TextChanged(object sender, EventArgs e)
        {
            ssk.Text = tbSavestate.Text;
        }

        public void SetSelected(bool selected)
        {
            btnSavestate.ForeColor = selected ? Color.OrangeRed : Color.PaleGreen;
        }
    }
}
