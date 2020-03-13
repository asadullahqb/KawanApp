using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace KawanApp.Models
{
    public class KawanUser : User
    {
        public double Rating { get; set; }
        public int[] Stars 
        {
            #region Constructing Stars Array
            //0 is for empty star, 1 is for half star, 2 is for filled star. s[n-1] refers to star position N, where N = {1,2,...5} and (N = n-1)
            get
            {
                int[] s = new int[5]; //This array stores the code for each star.

                if (Rating == 5)
                {
                    s[4] = 2; s[3] = 2; s[2] = 2; s[1] = 2; s[0] = 2; //Fill all stars
                }
                else if (Rating == 4)
                {
                    s[3] = 2; s[2] = 2; s[1] = 2; s[0] = 2; //Fill first 4 stars
                }
                else if (Rating == 3)
                {
                    s[2] = 2; s[1] = 2; s[0] = 2; //Fill first 3 stars
                }
                else if (Rating == 2)
                {
                    s[1] = 2; s[0] = 2; //Fill first 2 stars
                }
                else if (Rating == 1)
                {
                    s[0] = 2; //Fill first star
                }
                else if ((Rating - 4) > 0)
                    if (((Rating - 4) * 10) >= 5)
                    {
                        s[4] = 1; s[3] = 2; s[2] = 2; s[1] = 2; s[0] = 2; //Fill half of last star and fill all other stars
                    }
                    else
                    {
                        s[4] = 0; s[3] = 2; s[2] = 2; s[1] = 2; s[0] = 2; //Leave last star blank and fill all other stars
                    }
                else if ((Rating - 3) > 0)
                    if (((Rating - 3) * 10) >= 5)
                    {
                        s[3] = 1; s[2] = 2; s[1] = 2; s[0] = 2; //Fill half of second last star and fill all stars before it
                    }
                    else
                    {
                        s[3] = 0; s[2] = 2; s[1] = 2; s[0] = 2; //Leave second last star blank and fill all other stars before it
                    }
                else if ((Rating - 2) > 0)
                    if (((Rating - 2) * 10) >= 5)
                    {
                        s[2] = 1; s[1] = 2; s[0] = 2; //Fill half of middle star and fill all other stars before it
                    }
                    else
                    {
                        s[2] = 0; s[1] = 2; s[0] = 2; //Leave middle star blank and fill all other stars before it
                    }
                else if ((Rating - 1) > 0)
                    if (((Rating - 1) * 10) >= 5)
                    {
                        s[1] = 1; s[0] = 2; //Fill half second star and fill first star
                    }
                    else
                    {
                        s[1] = 0; s[0] = 2; //Leave second star blank and fill first star
                    }
                else if ((Rating) > 0)
                    if ((Rating * 10) >= 5)
                        s[0] = 1; //Fill half of first star
                    else
                        s[0] = 0; //Leave all stars blank
                return s; 
            }
            #endregion
        }
        public string AverageResponseTime { get; set; }
        public double AverageResponseTimeSeconds { get; set; }

        public bool IsAnyFilterFieldsNotNull
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName))
                    return true;
                else if (!string.IsNullOrEmpty(Email))
                    return true;
                else if (!string.IsNullOrEmpty(Gender))
                    return true;
                else if (!string.IsNullOrEmpty(PhoneNum))
                    return true;
                else if (!string.IsNullOrEmpty(Campus))
                    return true;
                else if (!string.IsNullOrEmpty(School))
                    return true;
                else if (!string.IsNullOrEmpty(Country))
                    return true;
                else if (!string.IsNullOrEmpty(AboutMe))
                    return true;
                else if (!string.IsNullOrEmpty(AverageResponseTime))
                    return true;
                else if (AverageResponseTimeSeconds>0)
                    return true;
                else
                    return false;
            }
        }

        public KawanUser NormaliseFilterFields()
        {
            if (string.IsNullOrEmpty(FirstName))
                FirstName = "";

            if (string.IsNullOrEmpty(Email))
                Email = "";

            if (string.IsNullOrEmpty(Gender))
                Gender = "";

            if (string.IsNullOrEmpty(PhoneNum))
                PhoneNum = "";

            if (string.IsNullOrEmpty(Campus))
                Campus = "";

            if (string.IsNullOrEmpty(School))
                School = "";

            if (string.IsNullOrEmpty(Country))
                Country = "";

            if (string.IsNullOrEmpty(AboutMe))
                AboutMe = "";

            if((AverageResponseTimeSeconds==0))
            {
                if (string.IsNullOrEmpty(AverageResponseTime))
                    AverageResponseTime = "";
            }
            else
            {
                //Convert AverageResponseTimeSeconds to AverageResponseTime
                double Value = AverageResponseTimeSeconds;
                if (Value == 0)
                    AverageResponseTime = "";
                else if (Value > 0 && Value < 1.4)
                    AverageResponseTime = "--";
                else if (Value > 1 && Value < 61)
                {
                    AverageResponseTime = TimeSpan.FromMinutes(Value - 1).ToString("%m'm'");
                }
                else if (Value > 60 && Value < 84)
                {
                    Value -= 60;
                    AverageResponseTime = TimeSpan.FromHours(Value).ToString("%h'h'");
                }
                else if (Value > 83 && Value < 115)
                {
                    Value -= 83;
                    AverageResponseTime = TimeSpan.FromDays(Value).ToString("%d'd'");
                }
                else
                    AverageResponseTime = "";
            }
            
            AverageResponseTime = AverageResponseTime.Replace(" ", string.Empty);

            return new KawanUser()
            {
                FirstName = FirstName.ToLower(),
                Email = Email.ToLower(),
                Gender = Gender.ToLower(),
                PhoneNum = PhoneNum.ToLower(),
                Campus = Campus.ToLower(),
                School = School.ToLower(),
                Country = Country.ToLower(),
                AboutMe = AboutMe.ToLower(),
                AverageResponseTime = AverageResponseTime.ToLower()
            };
        }
    }
}
