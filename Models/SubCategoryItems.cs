using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabiAsp.Models
{
    public class SubCategoryItems
    {
        public int SubCategorieId { get; set; }
        public string name { get; set; }
        public List<item> items { get; set; }
    }
}