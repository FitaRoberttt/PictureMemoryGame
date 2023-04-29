using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryGame.ViewModel
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _playedTimer;
        private TimeSpan _timePlayed;

        private const int _playSeconds = 1;

      
        public TimeSpan Time
        {
            get
            {
                return _timePlayed;
            }
            set
            {
                _timePlayed = value;
                OnPropertyRaised("Time");
            }
        }

        public TimerViewModel(TimeSpan time)
        {
            _playedTimer = new DispatcherTimer();
            _playedTimer.Interval = time;
            _playedTimer.Tick += PlayedTimer_Tick;
            _timePlayed = new TimeSpan();
        }

        public void Start()
        {
            _playedTimer.Start();
        }

        public void Stop()
        {
            _playedTimer.Stop();
        }

        private void PlayedTimer_Tick(object sender, EventArgs e)
        {
            Time = _timePlayed.Add(new TimeSpan(0, 0, 1));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
