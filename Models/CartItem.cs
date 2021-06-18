using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabiAsp.Models
{
    public class CartItem
    {
        public Cart cart { get; set; }
        public item item { get; set; }
    }
}