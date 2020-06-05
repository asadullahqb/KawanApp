using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ProfileTypeToDarkStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOwnProfile;
            if (value is bool)
            {
                IsOwnProfile = (bool)value;

                if (IsOwnProfile)
                    return Color.FromHex("#508019");
                else
                    return Color.FromHex("#581059");
            }
            else
                return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
