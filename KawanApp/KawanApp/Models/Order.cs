using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Order
    {
        public int Order_id { get; set; }
        public Product Product { get; set; }
        public International_students International_students { get; set; }
    }
}
