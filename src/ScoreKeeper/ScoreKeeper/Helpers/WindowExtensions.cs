using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ScoreKeeper.Helpers
{
    public static class WindowExtensions
    {
        public static void MaximizeTo(this Window window, Screen screen)
        {
            var secondaryScreen = Screen.AllScreens.FirstOrDefault(s => s.DeviceName == screen.DeviceName);

            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;

                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
        }
    }
}
