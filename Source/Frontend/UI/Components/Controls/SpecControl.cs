using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.UI.Components.Controls
{

    public abstract class SpecControl<T> : UserControl where T : new()
    {
        internal bool GeneralUpdateFlag = false; //makes other events ignore firing
        internal Timer updater;
        internal int updateThreshold = 50;
        internal bool FirstLoadDone = false;

		internal List<SpecControl<T>> slaveComps = new List<SpecControl<T>>();
        internal SpecControl<T> _parent = null;

        public event EventHandler<ValueUpdateEventArgs<T>> ValueChanged;
        public virtual void OnValueChanged(ValueUpdateEventArgs<T> e) => ValueChanged?.Invoke(this, e);

        private T _Value;

		private bool initialized = false;

        [Description("Net value of the control"), Category("Data")]
        public T Value
        {
            get { return _Value; }
            set
            {
				if (!initialized)
				{
					UpdateAllControls(value, null);
                    initialized = true;
                    return;
				}
				_Value = value;
            }
        }

        public SpecControl()
        {
            updater = new Timer();
            updater.Interval = updateThreshold;
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
            UpdateAllControls(value, setter, true);

            updater.Stop();
            updater.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpecControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "SpecControl";
            this.Size = new System.Drawing.Size(123, 30);
            this.ResumeLayout(false);

        }
    }

    public class ValueUpdateEventArgs<T> : EventArgs
    {
        public T Value;

        public ValueUpdateEventArgs(T value)
        {
            Value = value;
        }
    }
}
