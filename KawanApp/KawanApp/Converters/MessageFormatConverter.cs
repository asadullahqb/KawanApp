using System;
using System.Globalization;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class MessageFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string FormattedName = "";
            int length;
            string ending;
            if(value is string)
            {
                FormattedName = (string)value;
            }

            if(GetParameter(parameter).Equals("satForm"))
            {
                length = 19;
                ending = "..";
            }
            else
            {
                length = 20;
                ending = "...";
            }

            if (FormattedName.Length > length)
                return FormattedName.Substring(0, length) + ending; //Cut off the message at {length} characters
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
