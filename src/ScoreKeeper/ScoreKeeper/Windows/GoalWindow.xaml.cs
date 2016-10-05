using System.Windows;

namespace ScoreKeeper.Windows
{
    /// <summary>
    /// Interaction logic for GoalWindow.xaml
    /// </summary>
    public partial class GoalWindow : Window
    {
        public GoalWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Blackout(bool enable)
        {
            if (enable)
            {
                BlackoutPanel.Visibility = Visibility.Visible;
                WhiteoutPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                BlackoutPanel.Visibility = Visibility.Collapsed;
                WhiteoutPanel.Visibility = Visibility.Collapsed;

            }
        }

        public void Whiteout(bool enable)
        {
            if (enable)
            {
                BlackoutPanel.Visibility = Visibility.Collapsed;
                WhiteoutPanel.Visibility = Visibility.Visible;
            }
            else
            {
                BlackoutPanel.Visibility = Visibility.Collapsed;
                WhiteoutPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
