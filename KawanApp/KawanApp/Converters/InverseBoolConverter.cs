using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool truth;
            if (value is bool)
                truth = (bool)value;
            else
                return false;

            return !truth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
