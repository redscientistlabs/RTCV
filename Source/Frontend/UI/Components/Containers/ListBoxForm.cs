namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.UI.Modular;

    public partial class ListBoxForm : ComponentForm, IBlockable
    {
        private ComponentForm[] childForms;

        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public ListBoxForm(ComponentForm[] _childForms)
        {
            InitializeComponent();

            this.undockedSizable = false;

            childForms = _childForms ?? throw new ArgumentNullException(nameof(_childForms));

            //Populate the filter ComboBox
            lbComponentForms.DisplayMember = "Name";
            lbComponentForms.ValueMember = "Value";

            foreach (var item in childForms)
            {
                lbComponentForms.Items.Add(new ComboBoxItem<Form>(item.Text, item));
            }
        }

        private void OnComponentFormsSelectedIndexChanged(object sender, EventArgs e)
        {
            ((lbComponentForms.SelectedItem as ComboBoxItem<Form>)?.Value as ComponentForm)?.AnchorToPanel(pnTargetComponentForm);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            lbComponentForms.SelectedIndex = 0;
        }

        public void SetFocusedForm(ComponentForm form)
        {
            lbComponentForms.SelectedItem = lbComponentForms.Items.Cast<ComboBoxItem<Form>>().FirstOrDefault(x => x.Value == form);
        }
    }
}
