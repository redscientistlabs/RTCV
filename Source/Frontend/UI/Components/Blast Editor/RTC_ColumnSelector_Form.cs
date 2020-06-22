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
            UICore.SetRTCColor(UICore.GeneralColor, this);
            this.FormClosing += this.ColumnSelector_Closing;
        }

        public void LoadColumnSelector(DataGridViewColumnCollection columns)
        {
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

        private void ColumnSelector_Closing(object sender, FormClosingEventArgs e)
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
            if (S.GET<RTC_NewBlastEditor_Form>() != null)
            {
                S.GET<RTC_NewBlastEditor_Form>().VisibleColumns = temp;
                S.GET<RTC_NewBlastEditor_Form>().RefreshVisibleColumns();
            }
            RTCV.NetCore.Params.SetParam("BLASTEDITOR_VISIBLECOLUMNS", sb.ToString());
        }
    }
}
