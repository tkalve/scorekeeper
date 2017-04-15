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

        private TimeSpan _timeLeft;
        private bool _halfTime;

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
            get
            {
                return _whiteTeamGoals;
            }
            set
            {
                _whiteTeamGoals = value;
                NotifyPropertyChanged();
            }
        }

        public int Rounds
        {
            get
            {
                return _rounds;
            }
            set
            {
                _rounds = value;
                NotifyPropertyChanged();
            }
        }

        public int? CurrentRound
        {
            get { return _currentRound; }
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
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
