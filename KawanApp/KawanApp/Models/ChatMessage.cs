using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class ChatMessage
    {
        public string ServerKey { get { return App.ServerKey; } }
        public string Text { get; set; }
        public string SendingUser { get; set; }
        public string ReceivingUser { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
