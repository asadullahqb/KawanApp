using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
                ImageSource imgsrc;
                var converter = new ImageSourceConverter();
                friendStatus = (int)value;

                switch (friendStatus)
                {
                    case 0:
                        imgsrc = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                        break;
                    case 1:
                        imgsrc = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                        break;
                    case 2:
                        imgsrc = (ImageSource)converter.ConvertFromInvariantString("sendMessage.png");
                        break;
                    default:
                        imgsrc = null;
                        break;
                }

                return imgsrc;
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
