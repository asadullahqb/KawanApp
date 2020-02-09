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
        public string Password { get; set; }
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
        public string SchoolShort 
        { 
            get 
            {
                string schoolshort = School;
                if (!string.IsNullOrEmpty(schoolshort))
                {
                    schoolshort = Regex.Replace(schoolshort, "School of (.*)", "$1"); // Remove "school of "
                    schoolshort = Regex.Replace(schoolshort, "Languages, Literacies and Translations", "Lang., Literacies and Translations"); // Shorten "Languages"
                    schoolshort = Regex.Replace(schoolshort, "Engineering", "Eng."); // Shorten "Engineering"
                    schoolshort = Regex.Replace(schoolshort, "Materials", "Mat."); // Shorten "Material"
                    schoolshort = Regex.Replace(schoolshort, "Mineral", "Min."); // Shorten "Mineral"
                    return schoolshort;
                }
                else 
                    return null; 
            } 
        }
        public string Country { get; set; }
        public string AboutMe { get; set; }
        public string Pic { get; set; }
        public string Type { get; set; }
    }
}
