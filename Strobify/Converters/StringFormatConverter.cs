namespace Strobify.Converters
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))]
    public class StringOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
                var stringValue = value as string;
                Regex regex = new Regex("^[a-zA-Z0-9]+$");
                return stringValue != null && regex.IsMatch(stringValue) ? stringValue : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}