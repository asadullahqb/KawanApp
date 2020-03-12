using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class PhotoUpload
    {
        public string ServerKey { get { return App.ServerKey; } }
        public string CurrentUser { get; set; }
        public string FileName { get; set; }
        public string Base64 { get; set; }
        public string UserType { get; set; }
    }
}
