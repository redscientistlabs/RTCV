namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.UI.Modular;

    public partial class SelectBoxForm : ComponentForm, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private ComponentForm[] childForms;

        public SelectBoxForm(ComponentForm[] _childForms)
        {
            InitializeComponent();

            Colors.SetRTCColor(Colors.GeneralColor, this);

            childForms = _childForms;

            cbSelectBox.DisplayMember = "text";
            cbSelectBox.ValueMember = "value";

            foreach (var item in childForms)
            {
                cbSelectBox.Items.Add(new { text = item.Text, value = item });
            }
        }

        private void AnchorSelectedItemToPanel(object sender, EventArgs e)
        {
            ((cbSelectBox.SelectedItem as dynamic)?.value as ComponentForm)?.AnchorToPanel(pnComponentForm);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            cbSelectBox.SelectedIndex = 0;
        }
    }
}
