using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class UserAuthentication
    {
        public string StudentId { get; set; }
        public string Password { get; set; }

        public UserAuthentication(string studentid, string password = "")
        {
            this.StudentId = studentid;
            this.Password = password;
        }
    }
}
