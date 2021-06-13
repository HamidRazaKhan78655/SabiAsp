using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabiAsp.Models
{
    public class Cart
    {
        public item itemDetails { get; set; }
        public Shop Shop { get; set; }
        public SubCategory SubCat { get; set; }
        public UserItemCard cartDetail { get; set; }
    }
}