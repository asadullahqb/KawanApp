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
            //string Server = App.Server + kawan/;
            string Server = "http://www.imcc.usm.my/kawan/";
            string Pic = "";
            if (value is string)
            {
                Pic = (string)value;
            }
            if (!Pic.Equals("n/a"))
                return Server + Pic;
            else
                return Pic;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
