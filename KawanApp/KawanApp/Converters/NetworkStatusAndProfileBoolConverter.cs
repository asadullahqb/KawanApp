﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class NetworkStatusAndProfileBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsVisible;
            if (value is bool)
            {
                bool IsOwnProfile = (bool)value;
                if (IsOwnProfile)
                    IsVisible = App.NetworkStatus;
                else
                    IsVisible = true;
            }
            else
                return false;

            return IsVisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
