using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class UserTypeToPageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CurrentUserType;
            if (value is string)
            {
                CurrentUserType = (string)value;

                if (CurrentUserType == "Kawan")
                    return "Activities";
                else if (CurrentUserType == "International Student")
                    return "Satisfactory Forms";
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
