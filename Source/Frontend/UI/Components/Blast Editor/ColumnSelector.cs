namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.Common;

    public partial class ColumnSelector : Form, IAutoColorize
    {
        public ColumnSelector()
        {
            InitializeComponent();
            Colors.SetRTCColor(Colors.GeneralColor, this);
            this.FormClosing += this.OnFormClosing;
        }

        public void LoadColumnSelector(DataGridViewColumnCollection columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (DataGridViewColumn column in columns)
            {
                CheckBox cb = new CheckBox
                {
                    AutoSize = true,
                    Text = column.HeaderText,
                    Name = column.Name,
                    Checked = column.Visible
                };
                tablePanel.Controls.Add(cb);
            }
            this.Show();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!tablePanel.Controls.Cast<CheckBox>().Any(item => item.Checked))
            {
                e.Cancel = true;
                MessageBox.Show("Select at least one column");
                return;
            }
            List<string> temp = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (CheckBox cb in tablePanel.Controls.Cast<CheckBox>().Where(item => item.Checked))
            {
                temp.Add(cb.Name);

                sb.Append(cb.Name);
                sb.Append(",");
            }
            if (S.GET<BlastEditorForm>() != null)
            {
                S.GET<BlastEditorForm>().VisibleColumns = temp;
                S.GET<BlastEditorForm>().RefreshVisibleColumns();
            }
            NetCore.Params.SetParam("BLASTEDITOR_VISIBLECOLUMNS", sb.ToString());
        }
    }
}
