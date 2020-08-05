using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class News
    {
        public int News_id { get; set; }
        public string News_content { get; set; }
        public KawanUser Kawan { get; set; }
    }
}
