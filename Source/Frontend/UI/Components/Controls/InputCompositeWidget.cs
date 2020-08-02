//https://github.com/TASVideos/BizHawk/blob/master/BizHawk.Client.EmuHawk/config/InputCompositeWidget.cs
namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Windows.Forms;

    public partial class InputCompositeWidget : UserControl
    {
        public InputCompositeWidget()
        {
            InitializeComponent();

            _dropdownMenu = new ContextMenuStrip();

            _dropdownMenu.ItemClicked += DropdownMenu_ItemClicked;
            _dropdownMenu.PreviewKeyDown += DropdownMenu_PreviewKeyDown;
            foreach (var spec in InputWidget.SpecialBindings)
            {
                var tsi = new ToolStripMenuItem(spec.BindingName) { ToolTipText = spec.TooltipText };
                _dropdownMenu.Items.Add(tsi);
            }

            btnSpecial.ContextMenuStrip = _dropdownMenu;

            widget.CompositeWidget = this;
        }

        private const string WidgetTooltipText = "* Escape clears a key mapping\r\n* Disable Auto Tab to multiply bind";
        private ToolTip _tooltip;
        private string _bindingTooltipText;

        public void SetupTooltip(ToolTip tip, string bindingText)
        {
            _tooltip = tip;
            _tooltip.SetToolTip(btnSpecial, "Click here for special tricky bindings");
            _bindingTooltipText = bindingText;
            RefreshTooltip();
        }

        public void RefreshTooltip()
        {
            string widgetText = $"Current Binding: {widget.Text}";
            if (_bindingTooltipText != null)
            {
                widgetText = $"{widgetText}\r\n---\r\n{_bindingTooltipText}";
            }

            widgetText = $"{widgetText}\r\n---\r\n{WidgetTooltipText}";
            _tooltip.SetToolTip(widget, widgetText);
        }

        private void DropdownMenu_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // suppress handling of ALT keys, so that we can receive them as binding modifiers
            if (e.KeyCode == Keys.Menu)
            {
                e.IsInputKey = true;
            }
        }

        public void TabNext()
        {
            Parent.SelectNextControl(btnSpecial, true, true, true, true);
        }

        private readonly ContextMenuStrip _dropdownMenu;

        public bool AutoTab { get => widget.AutoTab; set => widget.AutoTab = value; }
        public string WidgetName { get => widget.WidgetName; set => widget.WidgetName = value; }

        public string Bindings { get => widget.Bindings; set => widget.Bindings = value; }

        public void Clear()
        {
            widget.ClearAll();
        }

        private void BtnSpecial_Click(object sender, EventArgs e)
        {
            _dropdownMenu.Show(MousePosition);
        }

        private void DropdownMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Input.ModifierKeys mods = new Input.ModifierKeys();

            if ((ModifierKeys & Keys.Shift) != 0)
            {
                mods |= Input.ModifierKeys.Shift;
            }

            if ((ModifierKeys & Keys.Control) != 0)
            {
                mods |= Input.ModifierKeys.Control;
            }

            if ((ModifierKeys & Keys.Alt) != 0)
            {
                mods |= Input.ModifierKeys.Alt;
            }

            var lb = new Input.LogicalButton(e.ClickedItem.Text, mods);

            widget.SetBinding(lb.ToString());
        }
    }
}
