using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.UI.Components.Controls
{
    public class ListBoxExtended : ListBox
    {
        [DllImport("user32")]
        public static extern int GetMessagePos();

        private const int WM_LBUTTONDOWN = 0x201;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                    OnPreSelect();
                    break;
                }
                base.WndProc(ref m);
        }
        protected void OnPreSelect()
        {
            int pos = GetMessagePos();

            var x = (short)(pos & 0xffff);
            var y = (short)((pos >> 16) & 0xffff);

            int clicked = this.IndexFromPoint(this.PointToClient(new System.Drawing.Point(x, y)));

            if (ModifierKeys.HasFlag(Keys.Shift) && this.SelectedIndices.Count != 0)
            {
                int lastSelected = this.SelectedIndices[this.SelectedIndices.Count - 1];

                if (lastSelected < clicked)
                {
                    for (int i = lastSelected; i < clicked; i++)
                    {
                        this.SetSelected(i, true);
                    }
                }
                else
                {
                    for (int i = lastSelected; i > clicked; i--)
                    {
                        this.SetSelected(i, true);
                    }

                }
            }
        }

    }
}
