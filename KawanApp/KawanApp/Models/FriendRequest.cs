using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class FriendRequest
    {
        public string ServerKey { get { return App.ServerKey; } }
        public string SendingStudentId { get; set; }
        public string ReceivingStudentId { get; set; }
    }
}
