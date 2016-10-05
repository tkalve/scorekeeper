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
    public class GameModel : INotifyPropertyChanged
    {
        private string _blueTeamName;
        private string _whiteTeamName;

        private int _blueTeamGoals;
        private int _whiteTeamGoals;

        private int _rounds;
        private int _currentRound;

        private TimeSpan _timeLeft;

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

        public int CurrentRound
        {
            get { return _currentRound; }
            set
            {
                _currentRound = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
