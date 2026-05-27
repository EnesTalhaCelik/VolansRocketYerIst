using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VolansCommunication
{
    internal class MissionTimer
    {
        private System.Timers.Timer _timer;  // Fully qualified reference to System.Timers.Timer
        private Action _task;

        public bool IsRunning { get; private set; } = false;

        public MissionTimer(double intervalMilliseconds, Action task)
        {
            _timer = new System.Timers.Timer(intervalMilliseconds); // Explicitly using System.Timers.Timer
            _task = task;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
        }

        // Timer'ı başlat
        public void Start()
        {
            if (!IsRunning)
            {
                _timer.Start();
                IsRunning = true;
            }
        }

        // Timer'ı durdur
        public void Stop()
        {
            if (IsRunning)
            {
                _timer.Stop();
                IsRunning = false;
            }
        }

        // Timer olayı
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _task.Invoke(); // Belirtilen görev fonksiyonunu çağır
        }
    }
}