using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.Common.Controls
{
    public class ConsoleTextBox : TextBox
    {
        private Queue<string> contents = new Queue<string>();
        private const int maxLines = 50;
        private bool useTimer = false;
        public bool UseTimer
        {
            get => useTimer;
            set
            {
                useTimer = value;
                if (value)
                {
                    createTimer();
                }
                else
                {
                    updateTimer?.Stop();
                    updateTimer = null;
                }
            }
        }

        private int timerInterval = 500;

        public int TimerInterval
        {
            get => timerInterval;
            set
            {
                timerInterval = value;
                if (useTimer)
                    createTimer();
            }
        }

        private System.Windows.Forms.Timer updateTimer;

        public ConsoleTextBox()
        {
            if (UseTimer)
            {
                createTimer();
            }
        }

        private void createTimer()
        {
            updateTimer?.Stop();

            updateTimer = new Timer
            {
                Interval = TimerInterval
            };
            updateTimer.Tick += (o, args) => Rewrite();
        }

        public void WriteLine(string input)
        {
            if (contents.Count == maxLines)
                contents.Dequeue();
            contents.Enqueue(input);

            if (!UseTimer)
                Rewrite();
        }

        private void Rewrite()
        {
            var sb = new StringBuilder();
            foreach (var s in contents)
            {
                sb.Append(s);
                sb.Append(Environment.NewLine);
            }
            this.Text = sb.ToString();
        }
    }
}
