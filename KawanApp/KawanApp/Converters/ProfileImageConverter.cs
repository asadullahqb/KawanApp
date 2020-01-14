using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ProfileImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Pic = "";
            if (value is string)
            {
                Pic = (string)value;
            }
            if (!Pic.Equals("n/a"))
                //return App.Server + Pic;
                return "http://www.imcc.usm.my/kawan/" + Pic;
            else
                return Pic;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
