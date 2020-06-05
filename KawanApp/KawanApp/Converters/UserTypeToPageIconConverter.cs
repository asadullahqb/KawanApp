using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class UserTypeToPageIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CurrentUserType;
            if (value is string)
            {
                var converter = new ImageSourceConverter();
                CurrentUserType = (string)value;

                if (CurrentUserType == "Kawan")
                    return (ImageSource)converter.ConvertFromInvariantString("activities.png");
                else if (CurrentUserType == "International Student")
                    return (ImageSource)converter.ConvertFromInvariantString("satisfactoryForms.png");
                else
                    return null;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
