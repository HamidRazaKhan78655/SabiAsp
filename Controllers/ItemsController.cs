using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class ItemsController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        // GET: Items
        public ActionResult Index()
        {
            try
            {
                ViewBag.Customercategories = Db.Categories.Select(d => new SelectListItem { Text = d.CategoryName, Value = d.CategoryId.ToString() });
            }
            catch (Exception)
            {
            }
            return View();
        }



        [HttpGet]
        public ActionResult GetCategories(int id)
        {
            if (id.ToString() == "")
            {
                return Json(ShowAllItems(), JsonRequestBehavior.AllowGet);
            }

            using (var db = new sabiShopEntities())
            {
                var data = db.SubCategories.Where(d => d.CategoryId == id).Select(d => new { name = d.name, id = d.SubCategorieId.ToString(), image = d.image }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public ActionResult GetSubCategoriesItem(int id)
        {
            using (var db = new sabiShopEntities())
            {

                var data = db.items.Where(d => d.SubCategorieId == id).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ShowAllItems()
        {
            using (var db = new sabiShopEntities())
            {
                var data = db.items.Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}