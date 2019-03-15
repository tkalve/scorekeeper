using System;
using System.Globalization;
using System.Windows.Data;

namespace ScoreKeeper.Converters
{
    public class TenthsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int milliseconds)
            {
                return milliseconds / 100;
            }

            if (value is string millisecondsString)
            {
                return int.Parse(millisecondsString) / 100;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int hundreds)
            {
                return hundreds * 100;
            }

            return value;
        }
    }
}