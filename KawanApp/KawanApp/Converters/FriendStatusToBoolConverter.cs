using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class FriendStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int friendStatus;
            if (value is int)
            {
                bool IsVisible;
                friendStatus = (int)value;

                switch (friendStatus)
                {
                    case 0:
                    case 1:
                        IsVisible = false;
                        break;
                    case 2:
                        IsVisible = false;
                        break;
                    case 3:
                        IsVisible = true;
                        break;
                    default:
                        IsVisible = false;
                        break;
                }

                return IsVisible;
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
