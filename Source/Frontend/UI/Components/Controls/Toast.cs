using RTCV.CorruptCore;
using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTCV.UI.Components.Controls
{
    public partial class Toast : UserControl
    {
        private static Toast _instance;

        private Dictionary<int, ToastEntry> _entries = new Dictionary<int, ToastEntry>();

        private bool _collapsed;
        public Toast()
        {
            InitializeComponent();

            pnEntriesContainer.Controls.Clear();

            this.lbChevron.Text = "\ue015"; // the designer can't handle funky unicode characters
            RtcCore.ToastProgressBarHandler += UpdateProgress;
            Colors.SetRTCColor(Colors.GeneralColor, this);
        }

        public static Toast GetInstance()
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new Toast();
            }

                return _instance;
        }

        public int AddToastEntry(string text = "")
        {
            int id = 0;
            while (_entries.ContainsKey(id))
            {
                id++;
            }

            ToastEntry entry = new ToastEntry();
            Colors.SetRTCColor(Colors.GeneralColor, entry);
            entry.UpdateEntry(text, 0);
            pnEntriesContainer.Controls.Add(entry);

            _entries.Add(id, entry);

            return id;
        }

        public void RemoveToastEntry(int id)
        {
            _entries[id].Dispose();
            _entries.Remove(id);

            if (_entries.Count == 0)
            {
                Close();
            }
        }

        private void Toast_Load(object sender, EventArgs e)
        {
            this.lbChevron.Cursor = HandCursor.Get();
        }
        
        private void UpdateProgress(object sender, ProgressBarEventArgs e)
        {
            SyncObjectSingleton.FormBeginExecute(() =>
            {
                if (_entries.TryGetValue(e.ToastID, out ToastEntry entry))
                {
                    string text = e.CurrentTask;
                    decimal progress = e.Progress;
                    entry.UpdateEntry(text, progress);
                }
            });
        }

        private void Close()
        {
            this.Dispose();
        }

        private void lbChevron_Click(object sender, EventArgs e)
        {
            this._collapsed = !this._collapsed;
            if (this._collapsed)
            {
                this.AutoSize = false;
                this.Height = 22;
                this.lbChevron.Text = "\ue013"; // 
            }
            else
            {
                this.AutoSize = true;
                this.lbChevron.Text = "\ue015"; // 
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            RtcCore.ToastProgressBarHandler -= UpdateProgress;
            base.Dispose(disposing);
        }
    }
}
