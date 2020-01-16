using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class BoolUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CurrentUserType;
            bool IsVisible = false;
            if (value is string)
            {
                CurrentUserType = (string)value;

                switch (CurrentUserType)
                {
                    case "Kawan":
                        IsVisible = false;
                        break;
                    case "International Student":
                        IsVisible = true;
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
