using ScoreKeeper.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScoreKeeper
{
    public class GameHub : INotifyPropertyChanged
    {
        private static GameHub _instance;
        public static GameHub Instance => _instance ?? (_instance = new GameHub());

        private GameHub()
        {
            CurrentGame = new Game();
            Games = new ObservableCollection<Game>();
        }

        public bool Whiteout { get; set; }
        public bool Blackout { get; set; }

        private Game _currentGame;
        private ObservableCollection<Game> _games;

        private bool _webServerEnabled;
        private string _webServerUrl;
        private string _webServerStatus;

        private bool _networkBroadcastEnabled;
        private string _networkBroadcastLog;

        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                NotifyPropertyChanged();
            }
        }

        public bool WebServerEnabled
        {
            get { return _webServerEnabled; }
            set
            {
                _webServerEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public string WebServerUrl
        {
            get { return _webServerUrl; }
            set
            {
                _webServerUrl = value;
                NotifyPropertyChanged();
            }
        }

        public string WebServerStatus
        {
            get { return _webServerStatus; }
            set
            {
                _webServerStatus = value;
                NotifyPropertyChanged();
            }
        }

        public bool NetworkBroadcastEnabled
        {
            get { return _networkBroadcastEnabled; }
            set
            {
                _networkBroadcastEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public string NetworkBroadcastLog
        {
            get { return _networkBroadcastLog; }
            set
            {
                _networkBroadcastLog = value;
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