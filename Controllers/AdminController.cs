using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
 
    public class AdminController : Controller
    {
        sabiShopEntities1 Db = new sabiShopEntities1();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            var categories = Db.Categories.ToList();
            ViewBag.categories = categories;
            return View();
        }
        // GET: Admin
        [HttpPost]
        public ActionResult Index(FormCollection admin)
        {
            string user = admin["username"].ToString();
            string pass = admin["pass"].ToString();
            if (user.Equals("sabi") && pass.Equals("1234"))
            {
                
                return RedirectToAction("Admin");
            }
            else
            {
                return View();
            }
        }
    }
}