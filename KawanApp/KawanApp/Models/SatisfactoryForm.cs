using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class SatisfactoryForm
    {
        public string ServerKey { get { return App.ServerKey; } }
        public int Index { get; set; }
        public int ActivitiesId { get; set; }
        public string KawanPic { get; set; }
        public string KawanFirstName { get; set; }
        public string KawanLastName { get; set; }
        public string KawanFullName { get { return KawanFirstName + " " + KawanLastName; } }
        public Activity ActivityInfo { get; set; }
        public DateTime? Date { get; set; }
        public bool IsFilled { get; set; }
        public int Rating { get; set; }
        public string ListImprovements { get; set; }
        public string ListLiked { get; set; }
        public string Comments { get; set; }
    }
}
