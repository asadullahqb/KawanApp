using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class DescriptionToIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Description;
            if (value is string)
            {
                Description = (string)value;
                if (Description == null) 
                    return false;
                else
                    return Description.Equals("Other");
            }
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
