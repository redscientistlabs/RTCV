using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RTCV.CorruptCore;

namespace RTCV.UI
{
    public partial class UI_ComponentFormSubForm : Form, ISubForm
    {
        public UI_ComponentFormSubForm()
        {
            InitializeComponent();

            UICore.SetRTCColor(UICore.GeneralColor, this);
        }


        public bool HasCancelButton
        {
            get { return false; }
            set { }
        }

        public void SubForm_Cancel()
        {

        }

        public void SubForm_Ok()
        {

        }

        private void UI_ComponentFormSubForm_Load(object sender, EventArgs e)
        {
            CreateList();
        }

        private BindingSource a;
        public void CreateList()
        {
            BindingList<Tuple<StashKey, string>> s = new BindingList<Tuple<StashKey, string>>();
            for (int i = 0; i < 4; i++)
                s.Add(new Tuple<StashKey, string>(new StashKey(i.ToString(), (i + 100).ToString(), null), i.ToString()));
            a = new BindingSource(s, null);
            savestateList1.DataSource = a;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(a.Position + savestateList1.NumPerPage < a.Count - 1)
                a.Position = a.Position + savestateList1.NumPerPage;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            a.Position = a.Position - savestateList1.NumPerPage;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
           a.Add(new Tuple<StashKey, string>(new StashKey(a.Count.ToString(), (a.Count + 100).ToString(), null), a.Count.ToString()));
        }
    }
}
