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
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentGame")
            {
                DataContext = GameHub.Instance.CurrentGame;
            }
        }
    }
}
