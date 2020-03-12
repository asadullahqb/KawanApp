using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ProfilePlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOwnProfile;
            var converter = new ImageSourceConverter();
            if (value is bool)
            {
                IsOwnProfile = (bool)value;

                if (IsOwnProfile)
                    return (ImageSource)converter.ConvertFromInvariantString("profileplaceholdergreen.png");
                else
                    return (ImageSource)converter.ConvertFromInvariantString("profileplaceholder.png");
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
