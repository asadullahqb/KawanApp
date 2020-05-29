using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class ActivitiesForServer
    {
        public string ServerKey { get { return App.ServerKey; } }
        public List<Activity> Activities { get; set; }
    }
}
