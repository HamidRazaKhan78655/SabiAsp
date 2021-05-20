using SabiAsp.Models;
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
            /*if (Session["Login"] == null)
            {
                var data =new { text=""};
                return Json(data, JsonRequestBehavior.AllowGet);
            }*/
            using (var db = new sabiShopEntities())
            {

               var  data = db.items.Where(d => d.SubCategorieId == id).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult ShowAllItems()
        {
            using (var db = new sabiShopEntities())
            {
                var data = db.items.Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image ,weight=d.Weight,price=d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        } 
        
        [HttpGet]
        public ActionResult GetSortList(int id)
        {
            using (var db = new sabiShopEntities())
            {
                
                if (id==1)
                {
                    var data= db.items.OrderBy(x => x.CreatedDate).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                
                    var data1 = db.items.OrderBy(x => x.Price).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                    return Json(data1, JsonRequestBehavior.AllowGet); 
            }
        }
       
    }
}