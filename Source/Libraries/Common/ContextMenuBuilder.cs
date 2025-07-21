// <copyright file="ContextMenuBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RTCV.Common
{
    public class ContextMenuBuilder
    {
        private readonly ContextMenuStrip _contextMenu = new ContextMenuStrip();
        private readonly Stack<ToolStripMenuItem> _subMenus = new Stack<ToolStripMenuItem>();
        private ControlFlow _controlFlow = ControlFlow.None;
        
        public ContextMenuBuilder()
        {
        }
        
        public ContextMenuBuilder If(bool condition)
        {
            _controlFlow = condition ? ControlFlow.If : ControlFlow.Else;
            return this;
        }
        public ContextMenuBuilder EndIf()
        {
            if (_controlFlow == ControlFlow.None)
            {
                throw new InvalidOperationException("EndIf() must be called after If() and Else()");
            }
            _controlFlow = ControlFlow.None;
            return this;
        }
        public ContextMenuBuilder Else()
        {
            if (_controlFlow == ControlFlow.None)
            {
                throw new InvalidOperationException("Else() must be called between If() and EndIf()");
            }
            _controlFlow = _controlFlow == ControlFlow.Else ? ControlFlow.If : ControlFlow.Else;
            return this;
        }
        
        public ContextMenuBuilder AddItem(ToolStripItem item)
        {
            if (_controlFlow == ControlFlow.Else)
            {
                return this;
            }

            if (_subMenus.Count > 0)
            {
                _subMenus.Peek().DropDownItems.Add(item);
            }
            else
            {
                _contextMenu.Items.Add(item);
            }
            return this;
        }
        public ContextMenuBuilder AddItem(string text, EventHandler onClick, bool enabled = true, bool isChecked = false)
        {
            var item = new ToolStripMenuItem(text);
            item.Enabled = enabled;
            item.Checked = isChecked;
            item.Click += onClick;
            return AddItem(item);
        }

        public ContextMenuBuilder AddText(string text, FontStyle style) =>
            AddText(text, true, style);
        public ContextMenuBuilder AddText(string text, bool enabled = true, FontStyle style = FontStyle.Regular)
        {
            var item = new ToolStripLabel(text);
            item.Enabled = enabled;
            item.Font = new Font(item.Font, style);
            return AddItem(item);
        }
        
        public ContextMenuBuilder AddHeader(string text)
        {
            var item = new ToolStripLabel(text);
            item.Font = new Font("Segoe UI", 12);
            return AddItem(item);
        }
        
        public ContextMenuBuilder AddSeparator()
        {
            return AddItem(new ToolStripSeparator());
        }
        
        public ContextMenuBuilder BeginSubMenu(string text, bool enabled = true)
        {
            if (_controlFlow == ControlFlow.Else)
            {
                return this;
            }
            
            var item = new ToolStripMenuItem(text);
            item.Enabled = enabled;
            _subMenus.Push(item);
            return this;
        }
        
        public ContextMenuBuilder EndSubMenu()
        {
            if (_subMenus.Count == 0)
            {
                throw new InvalidOperationException("EndSubMenu() called without a matching BeginSubMenu()");
            }
            if (_controlFlow == ControlFlow.Else)
            {
                return this;
            }
            _contextMenu.Items.Add(_subMenus.Pop());
            return this;
        }
        
        public ContextMenuBuilder DontCloseOnClick()
        {
            _contextMenu.Closing += (sender, e) =>
                e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
            return this;
        }
        
        public ContextMenuStrip Build()
        {
            if (_controlFlow != ControlFlow.None)
            {
                throw new InvalidOperationException("All If() calls must be closed with EndIf()");
            }
            if (_subMenus.Count > 0)
            {
                throw new InvalidOperationException("All BeginSubMenu() calls must be closed with EndSubMenu()");
            }
            return _contextMenu;
        }
        
        private enum ControlFlow
        {
            None,
            If,
            Else
        }
    }
}