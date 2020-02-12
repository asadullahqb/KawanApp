using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class ChatMessageRequest : ChatMessage
    {
        public string CurrentUserType { get; set; }
    }
}
