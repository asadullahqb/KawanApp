using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class UserAuthentication
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserAuthentication(string email = "", string password = "")
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
