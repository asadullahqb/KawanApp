using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class KawanUser : User
    {
        public double Rating { get; set; }
        public string AverageResponseTime { get; set; }
    }
}
