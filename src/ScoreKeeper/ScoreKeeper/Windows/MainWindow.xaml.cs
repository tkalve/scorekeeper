using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using ScoreKeeper.Helpers;
using ScoreKeeper.Hubs;
using ScoreKeeper.Models;
using ScoreKeeper.Properties;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace ScoreKeeper.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add a game
            GameHub.Instance.CurrentGame = new GameModel()
            {
                WhiteTeamName = "NOR",
                BlueTeamName = "SWE",
                Rounds = 2,
                CurrentRound = 1,
                WhiteTeamGoals = 25,
                BlueTeamGoals = 2,
                TimeLeft = new TimeSpan(0, 8, 0)
            };

            DataContext = new MainViewModel();
            ((MainViewModel)DataContext).Game = GameHub.Instance.CurrentGame;

            // Initialize the timer
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                // Stop timer if time is out
                // TODO: Show something graphically?
                if (GameHub.Instance.CurrentGame.TimeLeft == TimeSpan.Zero) _timer.Stop();

                // Remove a second from the timer
                GameHub.Instance.CurrentGame.TimeLeft = GameHub.Instance.CurrentGame.TimeLeft.Add(TimeSpan.FromSeconds(-1));

                // Update hub if enabled
                if (Settings.Default.EnableWebServer)
                    UpdateHubClock();

            }, System.Windows.Application.Current.Dispatcher) { IsEnabled =  false};
        }

        public Windows.GoalWindow GoalWindow;
        
        private readonly DispatcherTimer _timer;
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.EnableWebServer)
            {
                var ip = string.IsNullOrWhiteSpace(Settings.Default.WebServerIp)
                    ? Network.GetLocalIpAddress()
                    : Settings.Default.WebServerIp;

                var url = $"http://{ip}:{Settings.Default.WebServerPort}";

                WebApp.Start<Startup>(url: url);

                ((MainViewModel) DataContext).WebServerEnabled = true;
                ((MainViewModel) DataContext).WebServerUrl = url;

                UpdateHub();
            }

            // Add screens to dropdown
            foreach (var screen in Screen.AllScreens)
            {
                //DisplaysComboBox.Items.Add(screen);
            }

            // Show version
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            VersionLabel.Content = $"{version.Major}.{version.Minor}";
            VersionMinorLabel.Content = $".{version.Build}.{version.Revision}";
        }

        private void ShowScoresButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var screen = DisplaysComboBox.SelectedItem as Screen;
            //if (screen != null)
            //{
            //    GoalWindow?.Close();

            //    GoalWindow = new GoalWindow();
            //    GoalWindow.Show();
            //    GoalWindow.MaximizeTo(screen);
            //}
        }

        private void StartTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void StopTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void VersionLabel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            VersionMinorLabel.Visibility = Visibility.Visible;
        }

        private void VersionLabel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            VersionMinorLabel.Visibility = Visibility.Hidden;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void UpdateHubClock()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ScoreHub>();

            context.Clients.All.UpdateClock(
                $"{GameHub.Instance.CurrentGame.TimeLeft.Minutes}:{GameHub.Instance.CurrentGame.TimeLeft.Seconds:00}",
                GameHub.Instance.CurrentGame.CurrentRound);
        }
        private void UpdateHubScore()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ScoreHub>();

            context.Clients.All.UpdateScore(GameHub.Instance.CurrentGame.BlueTeamGoals, GameHub.Instance.CurrentGame.WhiteTeamGoals);
        }

        private void UpdateHub()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ScoreHub>();
            context.Clients.All.Update(
                GameHub.Instance.CurrentGame.BlueTeamName,
                GameHub.Instance.CurrentGame.WhiteTeamName,
                GameHub.Instance.CurrentGame.BlueTeamGoals,
                GameHub.Instance.CurrentGame.WhiteTeamGoals,
                $"{GameHub.Instance.CurrentGame.TimeLeft.Minutes}:{GameHub.Instance.CurrentGame.TimeLeft.Seconds:00}",
                GameHub.Instance.CurrentGame.CurrentRound
            );
        }

    }
}
