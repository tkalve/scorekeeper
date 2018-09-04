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
            Sheets = new ObservableCollection<SheetObject>();
        }

        public bool Whiteout { get; set; }
        public bool Blackout { get; set; }

        private Game _currentGame;
        private ObservableCollection<Game> _games;
        private ObservableCollection<SheetObject> _sheets;

        private bool _webServerEnabled;
        private string _webServerUrl;
        private string _webServerStatus;

        private bool _networkBroadcastEnabled;
        private string _networkBroadcastLog;

        public Game CurrentGame
        {
            get => _currentGame;
            set
            {
                _currentGame = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<SheetObject> Sheets
        {
            get => _sheets;
            set
            {
                _sheets = value;
                NotifyPropertyChanged();
            }
        }

        public bool WebServerEnabled
        {
            get => _webServerEnabled;
            set
            {
                _webServerEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public string WebServerUrl
        {
            get => _webServerUrl;
            set
            {
                _webServerUrl = value;
                NotifyPropertyChanged();
            }
        }

        public string WebServerStatus
        {
            get => _webServerStatus;
            set
            {
                _webServerStatus = value;
                NotifyPropertyChanged();
            }
        }

        public bool NetworkBroadcastEnabled
        {
            get => _networkBroadcastEnabled;
            set
            {
                _networkBroadcastEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public string NetworkBroadcastLog
        {
            get => _networkBroadcastLog;
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