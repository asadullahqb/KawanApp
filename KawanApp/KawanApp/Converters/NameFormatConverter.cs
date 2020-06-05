using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class NameFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string FormattedName = "";
            if(value is string)
            {
                FormattedName = (string)value;
            }

            string ending = (GetParameter(parameter).Equals("satForm")) ? ".." : "...";

            if (FormattedName.Length > 6)
                return FormattedName.Substring(0, 6) + ending; //Cut off the name at 6 characters
            else
                return FormattedName;
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
