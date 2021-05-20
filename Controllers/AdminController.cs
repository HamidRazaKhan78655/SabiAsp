using SabiAsp.Encryption;
using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
 
    public class AdminController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();

        // GET: Admin
        public ActionResult Index()
        {
            var username = "sabi";
            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower()).FirstOrDefault();
            if (User == null)
            {
                user u = new user();
                u.name = "Admin";
                u.EmailAddress = "admin@test.com";
                u.Contact = "74173564";
                u.username = "sabi";
                u.password = Encryption.Encrypto.EncryptString("test1234");
                u.Address = "test admin isb";
                u.RoleID = 1;
                u.RoleType = "Admin";
                u.CreatedBy = 1;
                u.CreatedDate = DateTime.Now;
                u.isDeleted = "false";
                Db.users.Add(u);
                Db.SaveChanges();
            }
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
            HttpContext.Session["Connectionstring"] = System.Configuration.ConfigurationManager.ConnectionStrings["sabiShopEntities"].ToString();
            string username = admin["username"].ToString();
            string password = admin["pass"].ToString();

            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower() && x.RoleID == 1).FirstOrDefault();
            if (User != null)
            {
                string DecryptPassword = Encrypto.DecryptString(User.password);
                if (password == DecryptPassword)
                {
                    Session["UserId"] = User.UserId.ToString();
                    Session["Username"] = User.username.ToString();
                    Session["Name"] = User.name.ToString();
                    Session["RoleType"] = User.RoleType.ToString();
                    Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                    Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return View();
                }
            }

            //if (user.Equals("sabi") && pass.Equals("1234"))
            //{   
            //    return RedirectToAction("Admin");
            //}
            else
            {
                return View();
            }
        }
    }
}