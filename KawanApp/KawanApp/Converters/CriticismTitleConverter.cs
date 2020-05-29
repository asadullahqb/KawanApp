using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class CriticismTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating;
            if (value is int)
            {
                string IsFilled = (string)parameter;
                rating = (int)value;
                if(IsFilled.Equals("IsNotFilled"))
                {
                    if (rating == 5)
                        return "What did you like?";
                    else
                        return "What could be done better?";
                }
                else
                {
                    if (rating == 5)
                        return "What you liked:";
                    else
                        return "What should be done better:";
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
