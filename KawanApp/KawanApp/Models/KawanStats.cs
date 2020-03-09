using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class KawanStats
    {
        public string TimeSpent { get; set; }
        public int StudentsHelped { get; set; }
        public int ActivitiesLogged { get; set; }
        public ListOfRanks ListOfRanks { get; set; }
    }

    public class ListOfRanks
    {
        public int FirstMonth { get; set; }
        public int SecondMonth { get; set; }
        public int ThirdMonth { get; set; }
        public int PredictedMonth { get; set; }
    }
}
