using SabiAsp.Models;
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

        private static List<SelectListItem> list;
        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetSubCategories(int id, string name)
        {
            var query = Db.items.ToList();
            List<item> items = new List<item>();            
            foreach (var SCate in query )
            {
                items.Add(SCate);   
            }
            ViewBag.Category = name;
            ViewBag.SubCategorylist = Db.SubCategories.Where(d => d.CategoryId == id).Select(d => new SelectListItem{ Text = d.name, Value = d.SubCategorieId.ToString() }).ToList();
            list = ViewBag.SubCategorylist;
            ViewBag.SCats = items;

            return View();
        } 
        public ActionResult GetSubCategoriesItem(int id, string name)
        {
            var query = Db.items.Where(d => d.SubCategorieId == id).Select(d => d).ToList();
            List<item> items = new List<item>();            
            foreach (var SCate in query)
            {
                items.Add(SCate);   
            }
            ViewBag.Category = name;
                //Db.SubCategories.Where(d => d.SubCategorieId == id).Select(t=>t.name).FirstOrDefault().ToString();
            ViewBag.SubCategorylist = list;
            
            ViewBag.SCats = items;
            return View("GetSubCategories");
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