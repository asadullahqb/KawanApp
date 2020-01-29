using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ProfileTypeToStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOwnProfile;
            if (value is bool)
            {
                var converter = new ImageSourceConverter();
                IsOwnProfile = (bool)value;

                if (IsOwnProfile)
                    return (ImageSource)converter.ConvertFromInvariantString("starFilledGreen.png");
                else
                    return (ImageSource)converter.ConvertFromInvariantString("starFilled.png"); ;
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
