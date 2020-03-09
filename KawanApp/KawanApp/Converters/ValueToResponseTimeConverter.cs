using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace KawanApp.Converters
{
    public class ValueToResponseTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Value;
            if (value is double)
            {
                Value = (double)value;
                Value = System.Convert.ToInt32(Value);
                if (Value == 0)
                    return null;
                else if (Value == 1)
                    return "--";
                else if (Value > 1 && Value < 61)
                {
                    return TimeSpan.FromMinutes(Value - 1).ToString("%m'm'");
                }
                else if (Value > 60 && Value < 84)
                {
                    Value -= 60;
                    return TimeSpan.FromHours(Value).ToString("%h'h'");
                }
                else if (Value > 83 && Value < 115)
                {
                    Value -= 83;
                    return TimeSpan.FromDays(Value).ToString("%d'd'");
                }
                else
                    return null;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string AverageResponseTime;
            if (value is string)
            {
                AverageResponseTime = (string)value;
                if (!(AverageResponseTime == null) && !(AverageResponseTime == "--"))
                {
                    if (AverageResponseTime.Contains("d"))
                    {
                        AverageResponseTime = Regex.Replace(AverageResponseTime, "(.*)d", "$1");
                        double Value = Int32.Parse(AverageResponseTime);
                        if (Value == 0)
                            return 0;
                        else if (Value > 0 && Value < 32)
                            Value += 83;
                        else
                            Value = 0; //Not supported
                        return Value;
                    }
                    else if (AverageResponseTime.Contains("h"))
                    {
                        AverageResponseTime = Regex.Replace(AverageResponseTime, "(.*)h", "$1");
                        double Value = Int32.Parse(AverageResponseTime);
                        if (Value == 0)
                            return 0;
                        else if (Value > 0 && Value < 24)
                            Value += 60;
                        else
                            Value = 0; //Not supported
                        return Value;
                    }
                    else if (AverageResponseTime.Contains("m"))
                    {
                        AverageResponseTime = Regex.Replace(AverageResponseTime, "(.*)m", "$1");
                        double Value = Int32.Parse(AverageResponseTime);
                        if (Value == 0)
                            return 0;
                        else
                            Value += 1;
                        return Value;
                    }
                    else
                        return 0;
                }
                else if (AverageResponseTime == "--")
                    return 1;
                else
                    return 0;
            }
            else
                return null;
        }
    }
}
