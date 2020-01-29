using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KawanApp.Models
{
    public class User
    {
        public string ServerKey { get { return App.ServerKey; } }
        public int Index { get; set; }
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public int FriendStatus { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Campus { get; set; }
        public string CampusShort { get { if (!string.IsNullOrEmpty(Campus)) return Regex.Replace(Campus, "(.*) Campus", "$1"); else return null; } }
        public string School { get; set; }
        public string SchoolShort { get { if (!string.IsNullOrEmpty(School)) return Regex.Replace(School, "School of (.*)", "$1"); else return null; } }
        public string Country { get; set; }
        public string AboutMe { get; set; }
        public string Pic { get; set; }
        public string Type { get; set; }
    }
}
