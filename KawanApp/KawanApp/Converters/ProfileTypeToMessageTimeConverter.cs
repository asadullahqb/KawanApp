using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ProfileTypeToMessageTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOwnProfile;
            if (value is bool)
            {
                var converter = new ImageSourceConverter();
                IsOwnProfile = (bool)value;

                if (IsOwnProfile)
                    return (ImageSource)converter.ConvertFromInvariantString("messageTimeGreen.png");
                else
                    return (ImageSource)converter.ConvertFromInvariantString("messageTime.png"); ;
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
