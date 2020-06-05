using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class RatingStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int startype;
            if (value is int)
            {
                var converter = new ImageSourceConverter();
                startype = (int)value;

                switch (startype)
                {
                    case 0:
                        return (ImageSource)converter.ConvertFromInvariantString("starEmpty.png");
                    case 1:
                        return (ImageSource)converter.ConvertFromInvariantString("starHalf.png");
                    case 2:
                        return (ImageSource)converter.ConvertFromInvariantString("starFilled.png");
                }

                return null;
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
