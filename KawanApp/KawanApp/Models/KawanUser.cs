using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class KawanUser : User
    {
        public double Rating { get; set; }
        public int[] Stars 
        { 
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
        }
        public string AverageResponseTime { get; set; }
    }
}
