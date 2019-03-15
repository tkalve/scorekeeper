using System;
using System.Windows;
using System.Windows.Controls;

namespace ScoreKeeper.Controls
{
    /// <summary>
    /// Interaction logic for GoalWindow.xaml
    /// </summary>
    public partial class GoalView : UserControl
    {
        public GoalView()
        {
            InitializeComponent();
        }

        private void GoalView_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = GameHub.Instance.CurrentGame;
            GameHub.Instance.PropertyChanged += Instance_PropertyChanged;

            if (GameHub.Instance.CurrentGame != null)
                GameHub.Instance.CurrentGame.PropertyChanged += CurrentGame_PropertyChanged;
        }

        private void CurrentGame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimeLeft")
            {
                if (GameHub.Instance.CurrentGame.TimeLeft < TimeSpan.FromSeconds(10))
                {
                    ClockTextBlockHundredsViewbox.Visibility = Visibility.Visible;
                    ClockTextBlockViewbox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ClockTextBlockHundredsViewbox.Visibility = Visibility.Collapsed;
                    ClockTextBlockViewbox.Visibility = Visibility.Visible;
                }
            }
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentGame")
            {
                DataContext = GameHub.Instance.CurrentGame;

                if (GameHub.Instance.CurrentGame != null)
                    GameHub.Instance.CurrentGame.PropertyChanged += CurrentGame_PropertyChanged;
            }
        }
    }
}
