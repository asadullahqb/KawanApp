using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Report
    {
        public int Report_id { get; set; }
        public string Report_content { get; set; }
        public KawanUser KawanUser { get; set; }
    }
}
