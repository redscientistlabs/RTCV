namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public abstract class SpecControl<T> : UserControl
        where T : new()
    {
        internal bool GeneralUpdateFlag = false; //makes other events ignore firing
        internal Timer updater;
        internal int updateThreshold = 50;
        internal bool FirstLoadDone = false;

        internal List<SpecControl<T>> slaveComps = new List<SpecControl<T>>();
        internal SpecControl<T> _parent = null;

        public event EventHandler<ValueUpdateEventArgs<T>> ValueChanged;
        public virtual void OnValueChanged(ValueUpdateEventArgs<T> e)
        {
            ValueChanged?.Invoke(this, e);
        }

        internal T _Value;

        [Description("Net value of the control"), Category("Data")]
        public T Value
        {
            get => _Value;
            set
            {
                _Value = value;
                if (!GeneralUpdateFlag)
                {
                    UpdateAllControls(value, null);
                }
            }
        }

        public SpecControl()
        {
            updater = new Timer
            {
                Interval = updateThreshold
            };
            updater.Tick += Updater_Tick;
            this.Load += SpecControl_Load;
        }

        internal abstract void UpdateAllControls(T value, Control setter, bool ignore = false);

        private void SpecControl_Load(object sender, EventArgs e)
        {
            FirstLoadDone = true;
        }

        internal virtual void Updater_Tick(object sender, EventArgs e)
        {
            updater.Stop();
            OnValueChanged(new ValueUpdateEventArgs<T>(Value));
        }

        public virtual void registerSlave(SpecControl<T> comp)
        {
            slaveComps.Add(comp);
            comp._parent = this;
        }

        internal void PropagateValue(T value, Control setter)
        {
            UpdateAllControls(value, setter);
            Value = value;
            updater.Stop();
            updater.Start();
        }
    }

    public class ValueUpdateEventArgs<T> : EventArgs
    {
        public T _value;

        public ValueUpdateEventArgs(T value)
        {
            _value = value;
        }
    }
}
