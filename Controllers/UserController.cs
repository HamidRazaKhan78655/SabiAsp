using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            ViewBag.Users = Db.users.ToList();
            return View();
        }

        public ActionResult GetUserByName(string Name)
        {
            var userList = new List<user>();
            if (!String.IsNullOrEmpty(Name))
            {
                userList = Db.users.Where(s => s.name.Contains(Name)).ToList();
            }
            ViewBag.Users = userList;
            return RedirectToAction("Index");
        }
        public ActionResult GetUserById(string userId)
        {
            int? uId = int.Parse(userId);
            var user = Db.users.Where(x => x.UserId == uId && x.isDeleted != "true").SingleOrDefault();
            ViewBag.User = user;
            return View();
        }
        public string AddUser(FormCollection fm)
        {
            string firstName = fm["FirstName"].ToString();
            string lastName = fm["LastName"].ToString();
            //string adminId = fm["adminId"].ToString();
            string username = fm["Username"].ToString();
            string emailAddress = fm["EmailAddress"].ToString();
            string contact = fm["Contact"].ToString();
            string address = fm["Address"].ToString();
            string password = fm["Password"].ToString();
            string roleType = fm["RoleType"].ToString();
            int roleId = int.Parse(roleType);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower() && x.EmailAddress == emailAddress.ToLower()).FirstOrDefault();
            if (User == null)
            {
                user u = new user();
                u.name = firstName + " " + lastName;
                u.EmailAddress = emailAddress;
                u.Contact = contact;
                u.username = username;
                u.password = Encryption.Encrypto.EncryptString(password.Trim());
                u.Address = address;
                u.RoleID = roleId;
                if (roleId == 1)
                    u.RoleType = "Admin";
                else if (roleId == 2)
                    u.RoleType = "User";
                else
                    u.RoleType = "Vendor";

                u.CreatedBy = logedinUserId;
                u.CreatedDate = DateTime.Now;
                u.isDeleted = "false";
                Db.users.Add(u);
                Db.SaveChanges();

                if (roleId == 3)
                {
                    vendor v = new vendor();
                    v.UserId = u.UserId;
                    v.isDeleted = "false";
                    v.CreatedBy = 1;
                    v.CreatedDate = DateTime.Now;
                    Db.vendors.Add(v);
                    Db.SaveChanges();

                    Shop s = new Shop();
                    s.vendorid = v.vendorid;
                    if (firstName.Contains("'"))
                        s.shopname = firstName + " " + "Shop";
                    else
                        s.shopname = firstName + "'" + " " + "Shop";

                    s.isDeleted = "false";
                    s.CreatedBy = 1;
                    s.CreatedDate = DateTime.Now;
                    Db.Shops.Add(s);
                    Db.SaveChanges();
                }
                return "success";
            }

            return "error";
        }
        public string UpdateUser(FormCollection fm)
        {
            string name = fm["UpdateUserName"].ToString();
            string emailAddress = fm["UpdateUserEmailAddress"].ToString();
            string contact = fm["UpdateUserContact"].ToString();
            string address = fm["UpdateUserAddress"].ToString();
            string userName = fm["UpdateUserUsername"].ToString();
            string roleType = fm["UpdateRoleType"].ToString();
            int roleId = int.Parse(roleType);
            string updateUserId = fm["UpdateUserId"].ToString();
            int userId = int.Parse(updateUserId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var user = Db.users.Where(x => x.UserId == userId).SingleOrDefault();
            if (user != null)
            {
                user.name = name;
                user.EmailAddress = emailAddress;
                user.Contact = contact;
                user.Address = address;
                user.username = userName;
                user.RoleID = roleId;
                if (roleId == 1)
                    user.RoleType = "Admin";
                else if (roleId == 2)
                    user.RoleType = "User";
                else
                    user.RoleType = "Vendor";

                user.ModifiedBy = logedinUserId;
                user.ModifiedDate = DateTime.Now;
                user.isDeleted = "false";
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public ActionResult DeleteUser(string userId)
        {
            int uId = int.Parse(userId);
            var user = Db.users.Where(x => x.UserId == uId && x.isDeleted != "true").SingleOrDefault();
            if (user != null)
            {
                user.ModifiedBy = 1;
                user.ModifiedDate = DateTime.Now;
                user.isDeleted = "true";
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}