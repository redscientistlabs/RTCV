namespace RTCV.UI.Components.Controls
{
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ListBoxExtended : ListBox
    {
        [DllImport("user32")]
        public static extern int GetMessagePos();

        private const int WM_LBUTTONDOWN = 0x201;
        private int lastClicked = -1;

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

            lastClicked = this.IndexFromPoint(this.PointToClient(new Point(x, y)));

            if (ModifierKeys.HasFlag(Keys.Shift) && this.SelectedIndices.Count != 0)
            {
                int lastSelected = this.SelectedIndices[this.SelectedIndices.Count - 1];

                if (lastSelected < lastClicked)
                {
                    for (int i = lastSelected; i < lastClicked; i++)
                    {
                        this.SetSelected(i, true);
                    }
                }
                else
                {
                    for (int i = lastSelected; i > lastClicked; i--)
                    {
                        this.SetSelected(i, true);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0) //Attempt to prevent being triggered from slight accidental mouse movement when deselecting by having a buffer
            {
                if (SelectionMode == SelectionMode.None)
                {
                    return;
                }

                int toSelect = this.IndexFromPoint(e.X, e.Y);
                if (toSelect != -1 && toSelect != lastClicked)
                {
                    this.SetSelected(toSelect, true);
                }
            }
        }
    }
}
