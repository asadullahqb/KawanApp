using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int friendStatus;
            if (value is int)
            {
                var converter = new ImageSourceConverter();
                friendStatus = (int)value;

                switch (friendStatus)
                {
                    case 0: //Not friends and friend request not sent
                        return (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    case 1: //Friend request sent
                        return (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    case 2: //Friend request received
                        return (ImageSource)converter.ConvertFromInvariantString("friendRequestReceived.png"); ;
                    case 3: //Friend
                        return (ImageSource)converter.ConvertFromInvariantString("sendMessage.png");
                    default:
                        return null;
                }
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
