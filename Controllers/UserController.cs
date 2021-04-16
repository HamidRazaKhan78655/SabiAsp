using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class UserController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        // GET: User
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
    }
}