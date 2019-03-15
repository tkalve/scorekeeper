using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using ScoreKeeper.Helpers;
using ScoreKeeper.Hubs;
using ScoreKeeper.Models;
using ScoreKeeper.Properties;
using System.Windows.Input;
using System.Windows.Media;
using Google.Apis.Sheets.v4;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Timer = System.Threading.Timer;

namespace ScoreKeeper.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Timer BroadcastTimer { get; set; }
        public UdpClient BroadcastClient { get; set; }

        public SheetReader SheetReader { get; set; }

        public GoalWindow GoalWindow;

        private readonly DispatcherTimer _timer;
        private Stopwatch _stopwatch;

        public MainWindow()
        {
            InitializeComponent();

            Round1Button.Background = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
            
            DataContext = GameHub.Instance;

            GameHub.Instance.PropertyChanged += MainViewModel_PropertyChanged;

            _stopwatch = new Stopwatch();
            // Initialize the timer
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Normal, delegate
            {
                if (GameHub.Instance.CurrentGame == null) return;

                var roundTime = new TimeSpan(0, GameHub.Instance.CurrentGame.RoundMinutes, 0);
                GameHub.Instance.CurrentGame.TimeLeft = roundTime.Subtract(_stopwatch.Elapsed);

                // Stop timer if time is out
                // TODO: Show something graphically?
                if (GameHub.Instance.CurrentGame.TimeLeft <= TimeSpan.Zero) { 
                    _timer.Stop();
                    _stopwatch.Stop();
                    GameHub.Instance.CurrentGame.TimeLeft = TimeSpan.Zero;
                    TimerClock.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
                }

            }, System.Windows.Application.Current.Dispatcher) { IsEnabled =  false};
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void BroadcastTimerCallback()
        {

            var gd = JsonConvert.SerializeObject(GameHub.Instance.CurrentGame);
            var data = Encoding.ASCII.GetBytes(gd);
            var endpoint = new IPEndPoint(IPAddress.Broadcast, 8888);
            BroadcastClient?.SendAsync(data, data.Length, endpoint);
        }

        private void EnableBroadcast()
        {
            BroadcastClient = new UdpClient();
            BroadcastClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            BroadcastTimer = new Timer(_ => BroadcastTimerCallback(), null, 0, 1000);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GameHub.Instance.NetworkBroadcastLog += ("Broadcast started.\n");
            }));
            GameHub.Instance.NetworkBroadcastEnabled = true;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            EnableBroadcast();

            // Add screens to dropdown
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                DisplaysItemsControl.Items.Add(screen);

            // Show version
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            VersionLabel.Content = $"{version.Major}.{version.Minor}";
            VersionMinorLabel.Content = $".{version.Build}.{version.Revision}";
        }

        private void StartTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.TimeLeft > TimeSpan.Zero) {
                _timer.Start();
                _stopwatch.Start();
            
                GameHub.Instance.CurrentGame.HalfTime = false;
                TimerClock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
        }

        private void StopTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _stopwatch.Stop();
            TimerClock.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        }

        private void ResetTimerbutton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _stopwatch.Stop();
            _stopwatch.Reset();
            GameHub.Instance.CurrentGame.TimeLeft = new TimeSpan(0, GameHub.Instance.CurrentGame.RoundMinutes, 0);
            TimerClock.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
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

        private void GamesListButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonExtensions.SetActive(GamesListButton, true);
            ButtonExtensions.SetActive(PreviewButton, false);
            ButtonExtensions.SetActive(ExternalDisplayButton, false);

            GamesTab.Visibility = Visibility.Visible;
            PreviewTab.Visibility = Visibility.Collapsed;
            ExternalDisplayTab.Visibility = Visibility.Collapsed;
        }

        private void PreviewButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonExtensions.SetActive(GamesListButton, false);
            ButtonExtensions.SetActive(PreviewButton, true);
            ButtonExtensions.SetActive(ExternalDisplayButton, false);

            GamesTab.Visibility = Visibility.Collapsed;
            PreviewTab.Visibility = Visibility.Visible;
            ExternalDisplayTab.Visibility = Visibility.Collapsed;
        }
        private void ExternalDisplayButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonExtensions.SetActive(GamesListButton, false);
            ButtonExtensions.SetActive(PreviewButton, false);
            ButtonExtensions.SetActive(ExternalDisplayButton, true);

            GamesTab.Visibility = Visibility.Collapsed;
            PreviewTab.Visibility = Visibility.Collapsed;
            ExternalDisplayTab.Visibility = Visibility.Visible;
        }

        private void DisplayItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Screen screen)
            {
                GoalWindow?.Close();
                GoalWindow = new GoalWindow();
                GoalWindow.Show();
                GoalWindow.MaximizeTo(screen);
            }
        }

        private void BlackoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonExtensions.GetActive(BlackoutButton) == false)
            {
                GoalWindow?.Blackout(true);
                ButtonExtensions.SetActive(BlackoutButton, true);
                ButtonExtensions.SetActive(WhiteoutButton, false);
            }
            else
            {
                GoalWindow?.Blackout(false);
                ButtonExtensions.SetActive(BlackoutButton, false);
                ButtonExtensions.SetActive(WhiteoutButton, false);
            }
            if (GoalWindow == null)
            {
                ButtonExtensions.SetActive(BlackoutButton, false);
                ButtonExtensions.SetActive(WhiteoutButton, false);
            }
        }

        private void WhiteoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonExtensions.GetActive(WhiteoutButton) == false)
            { 
                GoalWindow?.Whiteout(true);
                ButtonExtensions.SetActive(WhiteoutButton, true);
                ButtonExtensions.SetActive(BlackoutButton, false);
            }
            else
            {
                GoalWindow?.Whiteout(false);
                ButtonExtensions.SetActive(BlackoutButton, false);
                ButtonExtensions.SetActive(WhiteoutButton, false);
            }
            if (GoalWindow == null)
            {
                ButtonExtensions.SetActive(BlackoutButton, false);
                ButtonExtensions.SetActive(WhiteoutButton, false);
            }
        }

        private void BlueUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.BlueTeamGoals >= 0)
                GameHub.Instance.CurrentGame.BlueTeamGoals++;
            else if (GameHub.Instance.CurrentGame.BlueTeamGoals < 0)
                GameHub.Instance.CurrentGame.BlueTeamGoals = 0;
        }

        private void BlueDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.BlueTeamGoals > 0)
                GameHub.Instance.CurrentGame.BlueTeamGoals--;
            else if (GameHub.Instance.CurrentGame.BlueTeamGoals < 0)
                GameHub.Instance.CurrentGame.BlueTeamGoals = 0;
        }

        private void WhiteUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.WhiteTeamGoals >= 0)
                GameHub.Instance.CurrentGame.WhiteTeamGoals++;
            else if (GameHub.Instance.CurrentGame.WhiteTeamGoals < 0)
                GameHub.Instance.CurrentGame.WhiteTeamGoals = 0;
        }

        private void WhiteDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.WhiteTeamGoals > 0)
                GameHub.Instance.CurrentGame.WhiteTeamGoals--;
            else if (GameHub.Instance.CurrentGame.WhiteTeamGoals < 0)
                GameHub.Instance.CurrentGame.WhiteTeamGoals = 0;
        }

        private void Round1Button_Click(object sender, RoutedEventArgs e)
        {
            GameHub.Instance.CurrentGame.CurrentRound = 1;
            GameHub.Instance.CurrentGame.Extra = "";
            Round1Button.Background = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
            Round2Button.Background = new SolidColorBrush(Color.FromArgb(255, 128,128,128));
            SuddenDeathButton.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        }

        private void Round2Button_Click(object sender, RoutedEventArgs e)
        {
            GameHub.Instance.CurrentGame.CurrentRound = 2;
            GameHub.Instance.CurrentGame.Extra = "";
            Round1Button.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            Round2Button.Background = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
            SuddenDeathButton.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        }
        private void SuddenDeathButton_Click(object sender, RoutedEventArgs e)
        {
            GameHub.Instance.CurrentGame.CurrentRound = null;
            GameHub.Instance.CurrentGame.Extra = "SD";
            Round1Button.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            Round2Button.Background = new SolidColorBrush(Color.FromArgb(255, 128, 127, 128));
            SuddenDeathButton.Background = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
        }

        private void ResetScoreButton_Click(object sender, RoutedEventArgs e)
        {
            GameHub.Instance.CurrentGame.BlueTeamGoals = 0;
            GameHub.Instance.CurrentGame.WhiteTeamGoals = 0;
        }

        private void GamesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GameHub.Instance.CurrentGame = GamesListView.SelectedItem as Game;
            _stopwatch.Reset();
            if (GameHub.Instance.CurrentGame != null) { 
                _stopwatch.SetOffset(new TimeSpan(0, GameHub.Instance.CurrentGame.RoundMinutes, 0).Subtract(GameHub.Instance.CurrentGame.TimeLeft));
            }
        }

        private void TimerClock_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_timer.IsEnabled && !_stopwatch.IsRunning)
            {
                TimerClock.Visibility = Visibility.Collapsed;
                TimerClockEdit.Visibility = Visibility.Visible;

                MinuteTextBox.Text = GameHub.Instance.CurrentGame.TimeLeft.Minutes.ToString();
                SecondsTextBox.Text = GameHub.Instance.CurrentGame.TimeLeft.Seconds.ToString();
            }
        }

        private void SetTimeButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var minutes = int.Parse(MinuteTextBox.Text);
                var seconds = int.Parse(SecondsTextBox.Text);

                var timeSpan = new TimeSpan(0, minutes, seconds);
                var newElapsed = new TimeSpan(0, GameHub.Instance.CurrentGame.RoundMinutes, 0) - timeSpan;
                var diff = newElapsed - _stopwatch.Elapsed;
                _stopwatch.SetOffset(diff);
                GameHub.Instance.CurrentGame.TimeLeft = timeSpan;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            TimerClock.Visibility = Visibility.Visible;
            TimerClockEdit.Visibility = Visibility.Collapsed;
        }

        private void CancelTimeButton_OnClick(object sender, RoutedEventArgs e)
        {
            TimerClock.Visibility = Visibility.Visible;
            TimerClockEdit.Visibility = Visibility.Collapsed;
        }

        private void HalftimeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameHub.Instance.CurrentGame.HalfTime)
                GameHub.Instance.CurrentGame.HalfTime = false;
            else
            {
                GameHub.Instance.CurrentGame.CurrentRound = 2;
                GameHub.Instance.CurrentGame.Extra = "";
                GameHub.Instance.CurrentGame.HalfTime = true;
                Round1Button.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
                Round2Button.Background = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
                SuddenDeathButton.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
                _timer.Stop();
                _stopwatch.Stop();
                _stopwatch.Reset();
                GameHub.Instance.CurrentGame.TimeLeft = new TimeSpan(0, GameHub.Instance.CurrentGame.RoundMinutes, 0);
                TimerClock.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            }
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SheetReader == null)
                    SheetReader = new SheetReader();

                SheetReader.Login();
                SheetReader.GetSheets(GameHub.Instance.Sheets);
                SheetsList.SelectedIndex = 0;

                LoginPanel.Visibility = Visibility.Collapsed;
                LoadGamesPanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                LoginPanel.Visibility = Visibility.Visible;
                LoadGamesPanel.Visibility = Visibility.Collapsed;

                MessageBox.Show(ex.Message, "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_stopwatch.IsRunning)
            {
                MessageBox.Show("Only load games while the clock is not running", "Unable to load games", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            try
            {
                SheetReader.Load(SheetsList.SelectedValue as string, GameHub.Instance.Games);
                GamesListView.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured loading sheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class ButtonExtensions
    {
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.RegisterAttached("Active", typeof(bool), typeof(ButtonExtensions), new PropertyMetadata(default(bool)));

        public static void SetActive(UIElement element, bool value)
        {
            element.SetValue(ActiveProperty, value);
        }

        public static bool GetActive(UIElement element)
        {
            return (bool)element.GetValue(ActiveProperty);
        }
    }
}
