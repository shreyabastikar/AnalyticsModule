using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Controls;

namespace Analytics
{
    public class InactivityTimer
    {
        public event EventHandler Inactivity;
        public TimeSpan Timeout { get; private set; }

        private bool _enabled = false;

        public TimeSpan InactivityTime
        {
            get
            {
                
                return TimeSpan.FromMilliseconds(Environment.TickCount - _lastActivityTime); 
            }
        }

        private int _lastActivityTime;
        private DispatcherTimer _timer;

        public InactivityTimer(TimeSpan Timeout)
        {
           
            if (Timeout <= TimeSpan.Zero || Timeout.TotalMilliseconds > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Timeout", Timeout, string.Format("Must be a positive number less than {0}.", int.MaxValue));
            }

            this.Timeout = Timeout;
            _lastActivityTime = Environment.TickCount;
            
            InputManager.Current.PreNotifyInput += new NotifyInputEventHandler(Current_PreNotifyInput);

            _timer = new DispatcherTimer();
            _timer.Interval = Timeout;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
           
        }

        public void Dispose()
        {
           
            _timer.Dispatcher.VerifyAccess();
            _timer.Tick -= new EventHandler(_timer_Tick);
            _timer.Stop();

            InputManager.Current.PreNotifyInput -= new NotifyInputEventHandler(Current_PreNotifyInput);
        }

        public bool Enabled
        {

            get { return _enabled; }
            set
            {
                
                _enabled = value;
                Reset();
            }
        }

        public void Reset()
        {
            
            _timer.Dispatcher.VerifyAccess();
            _lastActivityTime = Environment.TickCount;
        }

        private void Current_PreNotifyInput(object sender, NotifyInputEventArgs e)
        {
            
            if (e.StagingItem.Input is MouseEventArgs || 
                e.StagingItem.Input is KeyboardEventArgs || 
                e.StagingItem.Input is TextCompositionEventArgs || 
                e.StagingItem.Input is StylusEventArgs)
            {
                _lastActivityTime = Environment.TickCount;
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            
            if (!Enabled) return;

            int time = Environment.TickCount;
            int inactivityTime = _lastActivityTime + (int)Timeout.TotalMilliseconds;

            if (inactivityTime - time <= 0)
            {
                if (Inactivity != null)
                {
                    Inactivity(this, EventArgs.Empty);
                }
                _lastActivityTime = time;
                _timer.Interval = Timeout;
                
            }
            else { 
                _timer.Interval = TimeSpan.FromMilliseconds(inactivityTime - time);
                
             }
        }

    }
}
