using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class CategoriesController : Controller
    {

        sabiShopEntities Db = new sabiShopEntities();
        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetSubCategories(int id, string name)
        {
            var query = Db.SubCategories.ToList();
            List<SubCategory> subCategories = new List<SubCategory>();
            foreach (var SCate in query ) {
                if (SCate.CategoryId == id) {
                    subCategories.Add(SCate);
                }
                
            }
            ViewBag.Category = name;
            ViewBag.SCats = subCategories;

            return View();
        }
        public ActionResult getItemsByCategories(int id , string name)
        {
            var query = Db.items.ToList();
            List<item> items = new List<item>();
            foreach (var item in query)
            {
                if (item.SubCategorieId == id)
                {
                    items.Add(item);
                }

            }
            ViewBag.SCategory = name;
            ViewBag.items = items;
            return View();
        }
        // Post: Categories
    }
}