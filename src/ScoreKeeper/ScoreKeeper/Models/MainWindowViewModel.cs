using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private GameModel _game;
        private bool _webServerEnabled;
        private string _webServerUrl;
        private string _webServerStatus;

        private bool _networkBroadcastEnabled;
        private string _networkBroadcastLog;

        public GameModel Game
        {
            get { return _game; }
            set
            {
                _game = value;
                NotifyPropertyChanged();
            }
        }

        public bool WebServerEnabled
        {
            get {  return _webServerEnabled; }
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