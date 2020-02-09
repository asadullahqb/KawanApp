using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Session
    {
        public string ServerKey { get { return App.ServerKey; } }
        public string SessionId { get; set; }
        public string StudentId { get; set; }
        public DateTime StartOrEnd { get; set; }
        public string Type { get; set; }
    }
}
