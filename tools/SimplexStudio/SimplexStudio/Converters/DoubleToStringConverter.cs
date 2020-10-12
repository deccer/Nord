using System;
using System.Globalization;
using System.Windows.Data;

namespace SimplexStudio.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value.ToString();
            return double.TryParse(stringValue, out var doubleValue) ? doubleValue : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = System.Convert.ToDouble(value);
            return doubleValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}