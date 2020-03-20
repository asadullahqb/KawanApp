﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class LoginReply : ReplyMessage
    {
        public string UserType { get; set; }
        public string CurrentFirstName { get; set; }
        public string CurrentPic { get; set; }
    }
}
