using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KawanApp.Models
{
    public class Activity
    {
        public int Index { get; set; }
        public string KawanStudentId { get; set; } //Not needed for the activity when fetching and viewing. Only needed when storing the activity.
        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherDescription { get; set; } //Used for view only
        public string StudentHelped { get; set; }
        public string StudentPic { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFullName { get { return StudentFirstName + " " + StudentLastName; } }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; } //Used for view only
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; } //Used for view only
        public string DateRange { get { return StartDate.ToString("dd/MM") + " - " + EndDate.ToString("dd/MM, yyyy"); } }
    }
}
