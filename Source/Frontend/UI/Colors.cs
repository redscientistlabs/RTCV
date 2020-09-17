namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.UI.Extensions;

    public static class Colors
    {
        internal static Color GeneralColor = Color.LightSteelBlue;

        public static Color Light1Color { get; private set; }
        public static Color Light2Color { get; private set; }
        public static Color NormalColor { get; private set; }
        public static Color Dark1Color { get; private set; }
        public static Color Dark2Color { get; private set; }
        public static Color Dark3Color { get; private set; }
        public static Color Dark4Color { get; private set; }

        public static void SetRTCColor(Color color, Control ctr = null)
        {
            HashSet<Control> allControls = new HashSet<Control>();

            if (ctr == null)
            {
                foreach (Form targetForm in UICore.AllColorizedSingletons())
                {
                    if (targetForm != null)
                    {
                        foreach (var c in targetForm.Controls.getControlsWithTag())
                            allControls.Add(c);
                        allControls.Add(targetForm);
                    }
                }

                //Get the extraforms
                foreach (CanvasForm targetForm in CanvasForm.extraForms)
                {
                    foreach (var c in targetForm.Controls.getControlsWithTag())
                        allControls.Add(c);
                    allControls.Add(targetForm);
                }

                //We have to manually add the mtform because it's not singleton, not an extraForm, and not owned by any specific form
                //Todo - Refactor this so we don't need to add it separately
                if (UICore.mtForm != null)
                {
                    foreach (var c in UICore.mtForm.Controls.getControlsWithTag())
                        allControls.Add(c);
                    allControls.Add(UICore.mtForm);
                }
            }
            else if (ctr is Form || ctr is UserControl)
            {
                foreach (var c in ctr.Controls.getControlsWithTag())
                    allControls.Add(c);
                allControls.Add(ctr);
            }
            else if (ctr is Form)
            {
                allControls.Add(ctr);
            }

            float generalDarken = -0.50f;
            float light1 = 0.10f;
            float light2 = 0.45f;
            float dark1 = -0.20f;
            float dark2 = -0.35f;
            float dark3 = -0.50f;
            float dark4 = -0.85f;

            color = color.ChangeColorBrightness(generalDarken);

            Light1Color = color.ChangeColorBrightness(light1);
            Light2Color = color.ChangeColorBrightness(light2);
            NormalColor = color;
            Dark1Color = color.ChangeColorBrightness(dark1);
            Dark2Color = color.ChangeColorBrightness(dark2);
            Dark3Color = color.ChangeColorBrightness(dark3);
            Dark4Color = color.ChangeColorBrightness(dark4);

            var tag2ColorDico = new Dictionary<string, Color>
            {
                { "color:light2", Light2Color },
                { "color:light1", Light1Color },
                { "color:normal", NormalColor },
                { "color:dark1", Dark1Color },
                { "color:dark2", Dark2Color },
                { "color:dark3", Dark3Color },
                { "color:dark4", Dark4Color }
            };

            foreach (var c in allControls)
            {
                var tag = c.Tag?.ToString().Split(' ');

                if (tag == null || tag.Length == 0)
                {
                    continue;
                }

                //Snag the tag and look for the color.
                var ctag = tag.FirstOrDefault(x => x.Contains("color:"));

                //We didn't find a valid color
                if (ctag == null || !tag2ColorDico.TryGetValue(ctag, out Color _color))
                {
                    continue;
                }

                if (c is Label l && l.BackColor != Color.FromArgb(30, 31, 32))
                {
                    c.ForeColor = _color;
                }
                else
                {
                    c.BackColor = _color;
                }

                if (c is Button btn)
                {
                    btn.FlatAppearance.BorderColor = _color;
                }

                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = _color;
                }

                c.Invalidate();
            }
        }

        public static void SelectRTCColor()
        {
            // Show the color dialog.
            Color color;
            ColorDialog cd = new ColorDialog();
            DialogResult result = cd.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                color = cd.Color;
            }
            else
            {
                return;
            }

            GeneralColor = color;
            SetRTCColor(color);

            SaveRTCColor(color);
        }

        public static void LoadRTCColor()
        {
            if (NetCore.Params.IsParamSet("COLOR"))
            {
                string[] bytes = NetCore.Params.ReadParam("COLOR").Split(',');
                GeneralColor = Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2]));
            }
            else
            {
                GeneralColor = Color.FromArgb(110, 150, 193);
            }

            SetRTCColor(GeneralColor);
        }

        public static void SaveRTCColor(Color color)
        {
            NetCore.Params.SetParam("COLOR", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
        }
    }
}
