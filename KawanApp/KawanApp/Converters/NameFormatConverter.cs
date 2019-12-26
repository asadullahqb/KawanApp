﻿using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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

            if (FormattedName.Length > 14)
                return FormattedName.Substring(0, 14) + "..."; //Cut off the name at 14 characters
            else
                return FormattedName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
