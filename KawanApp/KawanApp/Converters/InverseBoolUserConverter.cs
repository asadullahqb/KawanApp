using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class InverseBoolUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Type;
            bool IsVisible = true;
            if (value is string)
            {
                Type = (string)value;

                switch (Type)
                {
                    case "Kawan":
                        IsVisible = true;
                        break;
                    case "International Student":
                        IsVisible = false;
                        break;
                }

                return !IsVisible;
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
