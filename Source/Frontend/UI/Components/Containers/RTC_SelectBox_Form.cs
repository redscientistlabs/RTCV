namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_SelectBox_Form : ComponentForm, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private ComponentForm[] childForms;

        public RTC_SelectBox_Form(ComponentForm[] _childForms)
        {
            InitializeComponent();

            UICore.SetRTCColor(UICore.GeneralColor, this);

            childForms = _childForms;

            cbSelectBox.DisplayMember = "text";
            cbSelectBox.ValueMember = "value";

            foreach (var item in childForms)
            {
                cbSelectBox.Items.Add(new { text = item.Text, value = item });
            }
        }

        private void cbSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((cbSelectBox.SelectedItem as dynamic)?.value as ComponentForm)?.AnchorToPanel(pnComponentForm);
        }

        private void RTC_SelectBox_Form_Load(object sender, EventArgs e)
        {
            cbSelectBox.SelectedIndex = 0;
        }

        private void RTC_SelectBox_Form_Resize(object sender, EventArgs e)
        {
            cbSelectBox_SelectedIndexChanged(sender, e);
            /*
            var cf = pnComponentForm.Controls.OfType<ComponentForm>().FirstOrDefault();

            if(cf != null)
            {
                cf.
            }
            */
            /*
            var controls = new ArrayList().AddRange(pnComponentForm.Controls);
            var query = from element in pnComponentForm.Controls
                        where element is ComponentForm
                        select element;

            var result = query.FirstOrDefault();
            */
        }
    }
}
