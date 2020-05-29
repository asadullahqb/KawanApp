using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Activity
    {
        public int Index { get; set; }
        public string KawanStudentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StudentHelped { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DateRange { get { return StartDate.ToString("dd/MM") + " - " + EndDate.ToString("dd/MM, yyyy"); } }
    }
}
