namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Input;
    using RTCV.UI.Modular;

    public partial class SettingsHotkeyConfigForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public SettingsHotkeyConfigForm()
        {
            InitializeComponent();

            LoadHotkeys();

            this.HotkeyTabControl.SelectedIndexChanged += HotkeyHotkeyTabControlSelectedIndexChanged;

            this.LostFocus += OnFormLostFocus;

            DoTabs();
            DoFocus();
        }

        private void OnFormGotFocus(object sender, EventArgs e)
        {
            UICore.SetHotkeyTimer(false);
            DoFocus();
        }

        private void OnFormLostFocus(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            Bindings.ClearBindings();
            foreach (var w in InputWidgets)
            {
                var b = UICore.HotkeyBindings.FirstOrDefault(x => x.DisplayName == w.WidgetName);
                b.Bindings = w.Bindings;
                //Rebind
                Bindings.BindMulti(b.DisplayName, b.Bindings);
            }

            var binds = JsonConvert.SerializeObject(UICore.HotkeyBindings, Formatting.Indented);
            NetCore.Params.SetParam("HOTKEYS", binds);
        }

        internal void SaveAndReload()
        {
            Save();
            LoadHotkeys();
            DoTabs();
        }

        private static void AddMissingHotKeys()
        {
            var def = BindingCollection.DefaultValues;

            //fetches all default bindings that aren't contained in the current bindings
            var missing = def.Where(it => UICore.HotkeyBindings.FirstOrDefault(it2 => it2.DisplayName == it.DisplayName) == null);

            foreach (var bind in missing)
            {
                UICore.HotkeyBindings.Add(bind);
            }
        }

        private static void LoadHotkeys()
        {
            if (NetCore.Params.IsParamSet("HOTKEYS"))
            {
                try
                {
                    var binds = JsonConvert.DeserializeObject<BindingCollection>(NetCore.Params.ReadParam("HOTKEYS"));

                    UICore.HotkeyBindings = binds;

                    AddMissingHotKeys();

                    foreach (var b in UICore.HotkeyBindings)
                    {
                        Bindings.BindMulti(b.DisplayName, b.Bindings);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Something went wrong when loading your hotkeys. Deleting old hotkeys and contining");
                    logger.Error(e, "Error loading hotkeys");
                    NetCore.Params.RemoveParam("HOTKEYS");
                }
            }
        }

        private void HotkeyHotkeyTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            DoFocus();
        }

        private void DoTabs()
        {
            HotkeyTabControl.TabPages.Clear();

            // Buckets
            var tabs = UICore.HotkeyBindings.Select(x => x.TabGroup).Distinct().ToList();

            foreach (var tab in tabs)
            {
                var _y = 16;
                var _x = 6;

                var tb = new TabPage { Name = tab, Text = tab, Tag = "color:normal" };

                var bindings = UICore.HotkeyBindings.Where(x => x.TabGroup == tab).OrderBy(x => x.Ordinal).ThenBy(x => x.DisplayName).ToList();

                int iwOffsetX = 110;
                int iwOffsetY = -4;
                int iwWidth = 120;
                foreach (var b in bindings)
                {
                    var l = new Label
                    {
                        Text = b.DisplayName,
                        Location = new Point(_x, _y),
                        Size = new Size(iwOffsetX - 2, 15),
                        ForeColor = Color.White,
                    };

                    var w = new InputCompositeWidget
                    {
                        Location = new Point(_x + iwOffsetX, _y + iwOffsetY),
                        AutoTab = false,
                        Width = iwWidth,
                        Height = 22,
                        WidgetName = b.DisplayName,
                    };

                    w.SetupTooltip(toolTip1, b.ToolTip);
                    toolTip1.SetToolTip(l, b.ToolTip);

                    w.Bindings = b.Bindings;

                    tb.Controls.Add(l);
                    tb.Controls.Add(w);

                    _y += 26;
                    if (_y > HotkeyTabControl.Height - 15)
                    {
                        _x += iwOffsetX + iwWidth + 10;
                        _y = 14;
                    }
                }
                HotkeyTabControl.TabPages.Add(tb);
            }
        }

        private void DoFocus()
        {
            if (HotkeyTabControl.SelectedTab != null)
            {
                foreach (var c in HotkeyTabControl.SelectedTab.Controls.OfType<InputWidget>())
                {
                    c.Focus();
                    return;
                }
            }
        }

        private IEnumerable<InputCompositeWidget> InputWidgets
        {
            get
            {
                var widgets = new List<InputCompositeWidget>();
                for (var x = 0; x < HotkeyTabControl.TabPages.Count; x++)
                {
                    for (var y = 0; y < HotkeyTabControl.TabPages[x].Controls.Count; y++)
                    {
                        if (HotkeyTabControl.TabPages[x].Controls[y] is InputCompositeWidget)
                        {
                            widgets.Add(HotkeyTabControl.TabPages[x].Controls[y] as InputCompositeWidget);
                        }
                    }
                }
                return widgets;
            }
        }

        private void Defaults()
        {
            foreach (var w in InputWidgets)
            {
                var b = UICore.HotkeyBindings.FirstOrDefault(x => x.DisplayName == w.WidgetName);
                if (b != null)
                {
                    w.Bindings = b.DefaultBinding;
                }
            }
        }
    }
}
