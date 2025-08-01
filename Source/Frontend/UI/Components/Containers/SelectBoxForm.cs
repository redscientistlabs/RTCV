namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.UI.Modular;

    public partial class SelectBoxForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private readonly ComponentForm[] _childForms;

        public override bool PopoutAllowed
        {
            get
            {
                if (this.cbSelectBox.SelectedItem is ComponentForm form)
                {
                    return form.PopoutAllowed;
                }
                try
                {
                    return ((ComponentForm)((dynamic)this.cbSelectBox.SelectedItem).value).PopoutAllowed;
                }
                catch
                {
                    return false;
                }
            }
        }

        public SelectBoxForm(ComponentForm[] childForms)
        {
            InitializeComponent();

            this._childForms = childForms ?? throw new ArgumentNullException(nameof(childForms));

            this.cbSelectBox.DisplayMember = "text";
            this.cbSelectBox.ValueMember = "value";

            foreach (var item in this._childForms)
            {
                this.cbSelectBox.Items.Add(new { text = item.Text, value = item });
            }
        }

        private void AnchorSelectedItemToPanel(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            try
            {
                object selected = this.cbSelectBox.SelectedItem;

                if (selected is ComponentForm asCf)
                {
                    asCf.AnchorToPanel(this.pnComponentForm);
                }
                else
                {
                    var cf = ((dynamic)selected)?.value as ComponentForm;
                    cf?.AnchorToPanel(this.pnComponentForm);
                    if (cf is { PopoutAllowed: false })
                    {
                        this.RestoreToPreviousPanel();
                    }
                }
            }
            catch (Exception ex)
            {
                /*try
                {
                    (cbSelectBox.SelectedItem as ComponentForm)?.AnchorToPanel(pnComponentForm);
                }
                catch
                {
                    throw;
                }*/

                throw;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.cbSelectBox.SelectedIndex = 0;
        }
    }
}
