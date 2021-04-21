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
            return View();
        }
    }
}