using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScoreKeeper.Windows
{
    /// <summary>
    /// Interaction logic for ScoreBugWindow.xaml
    /// </summary>
    public partial class ScoreBugWindow : Window
    {
        public ScoreBugWindow()
        {
            InitializeComponent();
        }

        private void ScoreBugWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = GameHub.Instance.CurrentGame;
            GameHub.Instance.PropertyChanged += Instance_PropertyChanged;

            if (GameHub.Instance.CurrentGame != null)
                GameHub.Instance.CurrentGame.PropertyChanged += CurrentGame_PropertyChanged;
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

        private void CurrentGame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimeLeft")
            {
                //if (GameHub.Instance.CurrentGame.TimeLeft < TimeSpan.FromSeconds(10))
                //{
                //    ClockTextBlockHundredsViewbox.Visibility = Visibility.Visible;
                //    ClockTextBlockViewbox.Visibility = Visibility.Collapsed;
                //}
                //else
                //{
                //    ClockTextBlockHundredsViewbox.Visibility = Visibility.Collapsed;
                //    ClockTextBlockViewbox.Visibility = Visibility.Visible;
                //}
            }
        }
    }
}
