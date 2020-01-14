using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class KawanUser
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Campus { get; set; }
        public string School { get; set; }
        public double Rating { get; set; }
        public string Country { get; set; }
        public string AboutMe { get; set; }
        public string Pic { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
    }
}
