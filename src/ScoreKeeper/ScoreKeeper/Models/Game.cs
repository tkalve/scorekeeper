using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ScoreKeeper.Models
{
    public class Game : INotifyPropertyChanged
    {
        public Game ()
        {
            _rounds = 2;
            _currentRound = 1;
            _blueTeamGoals = 0;
            _whiteTeamGoals = 0;
            _timeLeft = new TimeSpan(0, 8, 0);
            _halfTime = false;
            _roundMinutes = 8;
            _halfTimeMinutes = 3;
        }
        public Game(int roundMinutes)
        {
            _rounds = 2;
            _currentRound = 1;
            _blueTeamGoals = 0;
            _whiteTeamGoals = 0;
            _timeLeft = new TimeSpan(0, roundMinutes, 0);
            _halfTime = false;
            _roundMinutes = roundMinutes;
            _halfTimeMinutes = 3;
        }


        private int? _id;

        private string _blueTeamName;
        private string _whiteTeamName;

        private string _type;
        
        private int _blueTeamGoals;
        private int _whiteTeamGoals;

        private int _rounds;
        private int? _currentRound;

        private string _extra;
        private bool _suddenDeath;

        private int _roundMinutes;
        private TimeSpan _timeLeft;

        private bool _halfTime;
        private int _halfTimeMinutes;
        private TimeSpan _halfTimeTimeLeft;

        private bool _timeout;
        private int _timeoutMinutes;
        private TimeSpan _timeoutTimeLeft;

        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string BlueTeamName
        {
            get {  return _blueTeamName; }
            set
            {
                _blueTeamName = value;
                NotifyPropertyChanged();
            }
        }

        public string WhiteTeamName
        {
            get {  return _whiteTeamName; }
            set
            {
                _whiteTeamName = value;
                NotifyPropertyChanged();
            }
        }

        public string WhiteTeamShortName => _whiteTeamName?.ToUpper().Substring(0, 3);
        public string BlueTeamShortName => _blueTeamName?.ToUpper().Substring(0, 3);

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged();
            }
        }

        public int BlueTeamGoals
        {
            get { return _blueTeamGoals; }
            set
            {
                _blueTeamGoals = value;
                NotifyPropertyChanged();
            }
        }

        public int WhiteTeamGoals
        {
            get => _whiteTeamGoals;
            set
            {
                _whiteTeamGoals = value;
                NotifyPropertyChanged();
            }
        }

        public int Rounds
        {
            get => _rounds;
            set
            {
                _rounds = value;
                NotifyPropertyChanged();
            }
        }

        public int? CurrentRound
        {
            get => _currentRound;
            set
            {
                _currentRound = value;
                NotifyPropertyChanged();
            }
        }

        public string Extra
        {
            get { return _extra; }
            set
            {
                _extra = value;
                NotifyPropertyChanged();
            }
        }

        public bool SuddenDeath
        {
            get { return _suddenDeath; }
            set
            {
                _suddenDeath = value;
                NotifyPropertyChanged();
            }
        }

        public int RoundMinutes
        {
            get
            {
                return _roundMinutes;
            }
            set
            {
                _roundMinutes = value;
                NotifyPropertyChanged();
            }
        } 

        public TimeSpan TimeLeft
        {
            get
            {
                return _timeLeft;
            }
            set
            {
                _timeLeft = value;
                NotifyPropertyChanged();
            }
        }

        public bool HalfTime
        {
            get
            {
                return _halfTime;
            }
            set
            {
                _halfTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged($"TimeoutOrHalftime");
            }
        }

        public int HalfTimeMinutes
        {
            get { return _halfTimeMinutes; }
            set
            {
                _halfTimeMinutes = value;
                NotifyPropertyChanged();
            }
        }

        public bool Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged($"TimeoutOrHalftime");
            }
        }

        public TimeSpan HalfTimeTimeLeft
        {
            get
            {
                return _halfTimeTimeLeft;
            }
            set
            {
                _halfTimeTimeLeft = value;
                NotifyPropertyChanged();
            }
        }

        public bool TimeoutOrHalftime
        {
            get
            {
                if (_timeout || _halfTime)
                    return true;
                return false;
            }
        }

        public int TimeoutMinutes
        {
            get
            {
                return _timeoutMinutes;
            }
            set
            {
                _timeoutMinutes = value;
                NotifyPropertyChanged();
            }
        }

        public TimeSpan TimeoutTimeLeft
        {
            get
            {
                return _timeoutTimeLeft;
            }
            set
            {
                _timeoutTimeLeft = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
