﻿//Based on https://github.com/TASVideos/BizHawk/blob/master/BizHawk.Client.EmuHawk/config/InputWidget.cs
namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SpecialBindingInfo
    {
        public string BindingName { get; set; }
        public string TooltipText { get; set; }
    }

    public sealed class InputWidget : TextBox
    {
        // TODO: when binding, make sure that the new key combo is not in one of the other bindings
        private readonly Timer _timer = new Timer();
        private readonly List<string> _bindings = new List<string>();

        private string _wasPressed = "";

        public InputCompositeWidget CompositeWidget { get; set; }

        /// <summary>
        /// These bindings get ignored by the widget and can only be entered by SetBinding() via the context menu from the InputCompositeWidget
        /// </summary>
        public static readonly SpecialBindingInfo[] SpecialBindings =
        {
            new SpecialBindingInfo { BindingName = "Escape", TooltipText = "Binds the Escape key" },
            new SpecialBindingInfo { BindingName = "WMouse L", TooltipText = "Binds the left mouse button" },
            new SpecialBindingInfo { BindingName = "WMouse M", TooltipText = "Binds the middle mouse button" },
            new SpecialBindingInfo { BindingName = "WMouse R", TooltipText = "Binds the right mouse button" },
            new SpecialBindingInfo { BindingName = "WMouse 1", TooltipText = "Binds the mouse auxiliary button 1" },
            new SpecialBindingInfo { BindingName = "WMouse 2", TooltipText = "Binds the mouse auxiliary button 2" }
        };

        public InputWidget()
        {
            ContextMenu = new ContextMenu();
            _timer.Tick += Timer_Tick;
            ClearBindings();
            AutoTab = true;
            Cursor = Cursors.Arrow;
        }

        public bool AutoTab { get; set; }
        public string WidgetName { get; set; }

        public string Bindings
        {
            get => Text;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                ClearBindings();
                var newBindings = value.Trim().Split(',');
                _bindings.AddRange(newBindings);
                UpdateLabel();
            }
        }

        [DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        protected override void OnMouseClick(MouseEventArgs e)
        {
            HideCaret(Handle);
            base.OnMouseClick(e);
        }

        public void ClearAll()
        {
            ClearBindings();
            Clear();
        }

        private void ClearBindings()
        {
            _bindings.Clear();
        }

        private Color oldColor = Color.White;

        protected override void OnEnter(EventArgs e)
        {
            _timer.Start();

            UICore.SetHotkeyTimer(false);
            _wasPressed = Input.Input.Instance.GetNextBindEvent();
            oldColor = BackColor;
            BackColor = Color.DarkSlateBlue;
        }

        protected override void OnLeave(EventArgs e)
        {
            _timer.Stop();
            UICore.SetHotkeyTimer(true);
            UpdateLabel();
            BackColor = oldColor;
            base.OnLeave(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReadKeys();
        }

        private void EraseMappings()
        {
            ClearBindings();
            Text = "";
        }

        /// <summary>
        /// sets a binding manually. This may not be implemented quite right.
        /// </summary>
        public void SetBinding(string bindingStr)
        {
            _bindings.Add(bindingStr);
            UpdateLabel();
            Increment();
        }

        /// <summary>
        /// Poll input events and apply processing related to accepting that as a binding
        /// </summary>
        private void ReadKeys()
        {
            Input.Input.Update();
            var bindingStr = Input.Input.Instance.GetNextBindEvent();
            if (!string.IsNullOrEmpty(_wasPressed) && bindingStr == _wasPressed)
            {
                return;
            }

            if (bindingStr != null)
            {
                // has special meaning for the binding UI system (clear it).
                // you can set it through the special bindings dropdown menu
                if (bindingStr == "Escape")
                {
                    EraseMappings();
                    Increment();
                    return;
                }

                // seriously, we refuse to allow you to bind this to anything else.
                if (bindingStr == "Alt+F4")
                {
                    return;
                }

                // ignore special bindings
                foreach (var spec in SpecialBindings)
                {
                    if (spec.BindingName == bindingStr)
                    {
                        return;
                    }
                }

                if (!IsDuplicate(bindingStr))
                {
                    if (AutoTab)
                    {
                        ClearBindings();
                    }

                    _bindings.Add(bindingStr);
                }

                _wasPressed = bindingStr;
                UpdateLabel();
                Increment();
            }
        }

        private bool IsDuplicate(string binding)
        {
            return _bindings.FirstOrDefault(x => x == binding) != null;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Alt)
            {
                base.OnKeyUp(e);
            }

            _wasPressed = "";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Alt)
            {
                base.OnKeyDown(e);
                return;
            }

            e.Handled = true;
        }

        // Advances to the next widget depending on the autotab setting
        public void Increment()
        {
            if (AutoTab)
            {
                CompositeWidget.TabNext();
            }
        }

        public void Decrement()
        {
            if (AutoTab)
            {
                Parent.SelectNextControl(this, false, true, true, true);
            }
        }

        public void UpdateLabel()
        {
            Text = string.Join(",", _bindings.Where(str => !string.IsNullOrWhiteSpace(str)));
            CompositeWidget.RefreshTooltip();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Handled = true;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0201: // WM_LBUTTONDOWN
                    Focus();
                    return;
                case 0x0203: // WM_LBUTTONDBLCLK
                case 0x0204: // WM_RBUTTONDOWN
                case 0x0205: // WM_RBUTTONUP
                case 0x0206: // WM_RBUTTONDBLCLK
                    return;
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Delta > 0)
            {
                Decrement();
            }
            else
            {
                Increment();
            }

            base.OnMouseWheel(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            HideCaret(Handle);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return !(keyData.ToString() == "F4" || keyData.ToString().Contains("Alt"));
        }
    }
}
