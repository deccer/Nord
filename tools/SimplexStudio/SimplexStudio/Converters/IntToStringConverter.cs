using System;
using System.Globalization;
using System.Windows.Data;
using ImTools;

namespace SimplexStudio.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value.ToString();
            return int.TryParse(stringValue, out var intValue) ? intValue : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.TryParse(value.ToString(), out var intValue) ? intValue : 0;
        }
    }
}
