using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class FeedbackListConverter : IValueConverter
    {
        //Used for IsVisible property of the ListViews for Compliments and Criticisms.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating;
            string feedbackType = GetParameter(parameter);
            if (value is int)
            {
                rating = (int)value;
                if (feedbackType.Equals("compliment"))
                {
                    if (rating == 5)
                        return true;
                    else
                        return false;
                }
                else // if(feedbackType.Equals("criticism"))
                {
                    if (rating == 5)
                        return false;
                    else
                        return true;
                }
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        string GetParameter(object parameter)
        {
            if (parameter is string)
                return (string)parameter;

            return "";
        }
    }
}
