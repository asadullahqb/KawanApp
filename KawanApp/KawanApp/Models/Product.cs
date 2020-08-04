using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace KawanApp.Models
{
    public class Product
    {
        public int Prod_id { get; set; }
        public string Prod_name { get; set; }
        public Kawan Kawan { get; set; }
        public Category Category { get; set; }
        public string Prod_description { get; set; }
        public double Prod_price { get; set; }
        public int Prod_status { get; set; }
        public string Prod_image1 { get; set; }
        public string Prod_image2 { get; set; }
        public string Prod_image3 { get; set; }
        public string Prod_image4 { get; set; }
        public string Prod_image5 { get; set; }
    }
}
