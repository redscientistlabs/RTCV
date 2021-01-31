namespace RTCV.UI
{
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.UI.Modular;

    //From Bizhawk
    public static class NumberExtensions
    {
        public static Point GetMouseLocation(this MouseEventArgs e, object sender)
        {
            if (!(sender is Control ctr))
            {
                return new Point(e.Location.X, e.Location.Y);
            }

            var x = e.Location.X;
            var y = e.Location.Y;

            do
            {
                if (ctr.Parent != null
                    && !(ctr is ComponentFormTile)
                    && !(ctr is CanvasForm)
                    && !(ctr is ComponentPanel)
                    && !(ctr is ComponentForm)
                    )
                {
                    x += ctr.Location.X;
                    y += ctr.Location.Y;
                }

                ctr = ctr.Parent;
            }
            while (ctr != null);

            return new Point(x, y);
        }

        public static string ToHexString(this long n)
        {
            return $"{n:X}";
        }
    }
}
