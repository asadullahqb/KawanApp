using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Notification
    {
        public string Type { get; set; } //Friend, chat message
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
