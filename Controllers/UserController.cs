using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
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
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            if (logedinUserId == 0)
            {
                return RedirectToAction("SabiLogin", "Login");
            }
            else
            {
                int logedinRoleID = Convert.ToInt32(Session["RoleID"]);
                if (logedinRoleID == 1)
                {
                    ViewBag.Users = Db.users.OrderByDescending(x => x.CreatedBy).ToList();
                    ViewBag.Categories = Db.Categories.Where(x => x.isDeleted != "true").OrderByDescending(x => x.CreatedBy).ToList();
                    ViewBag.SubCategories = Db.SubCategories.Where(x => x.isDeleted != "true").OrderByDescending(x => x.CreatedBy).ToList();
                    ViewBag.Items = Db.items.Where(x => x.isDeleted != "true").OrderByDescending(x => x.CreatedBy).ToList();
                    ViewBag.Shops = Db.Shops.Where(x => x.isDeleted != "true").OrderByDescending(x => x.CreatedBy).ToList();
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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
            string username = fm["Username"].ToString();
            string emailAddress = fm["EmailAddress"].ToString();
            string contact = fm["Contact"].ToString();
            string address = fm["Address"].ToString();
            string password = fm["Password"].ToString();
            string roleType = fm["RoleType"].ToString();
            int roleId = int.Parse(roleType);
            string vendorCategoryDrp = fm["VendorCategoryDrp"].ToString();
            int categoryId = int.Parse(vendorCategoryDrp);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower() && x.EmailAddress == emailAddress.ToLower() && x.isDeleted != "true").FirstOrDefault();
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
                u.ShopName = "NA";
                if (roleId == 1)
                {
                    u.RoleType = "Admin";
                }
                else if (roleId == 2)
                {
                    u.RoleType = "User";
                }
                else {
                    u.RoleType = "Vendor";
                    if (firstName.Contains("'"))
                        u.ShopName = firstName + " " + "Shop";
                    else
                        u.ShopName = firstName + "'" + " " + "Shop";
                }
 
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
                    s.CategoryId = categoryId;
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
                user.ShopName = "NA";
                if (roleId == 1)
                {
                    user.RoleType = "Admin";
                }
                else if (roleId == 2)
                {
                    user.RoleType = "User";
                }
                else
                {
                    user.RoleType = "Vendor";
                    if (name.Contains("'"))
                        user.ShopName = name + " " + "Shop";
                    else
                        user.ShopName = name + "'" + " " + "Shop";
                }

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

        public ActionResult GetUsersByType(string type)
        {
            var users = new List<user>();
            if (type == "All")
                users = Db.users.OrderByDescending(x => x.CreatedBy).ToList();
            else
                users = Db.users.Where(u=> u.RoleType == type).OrderByDescending(x => x.CreatedBy).ToList();

            if (users.Count == 0)
            {
                users = new List<user>();
            }
            return PartialView("GetUsersByType", users);
        }

        public string UpdateUserProfile(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["ProfileUserName"].ToString();
            string emailAddress = fm["ProfileUserEmailAddress"].ToString();
            string contact = fm["ProfileUserContact"].ToString();
            string userName = fm["ProfileUserUsername"].ToString();
            string address = fm["ProfileUserAddress"].ToString();
            int userId = Convert.ToInt32(Session["UserId"]);

            var user = Db.users.Where(x => x.UserId == userId).SingleOrDefault();
            if (user != null)
            {
                string _fileName = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    try
                    {
                        var file = Request.Files[i];
                        string namefile = string.Empty;
                        namefile = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        if (string.IsNullOrEmpty(namefile) == false)
                        {
                            Bitmap b = (Bitmap)Bitmap.FromStream(file.InputStream);
                            _fileName = namefile.Replace(@"'", "") + "_" + DateTime.Now.ToString("mmss") + ".png";
                            var path = Server.MapPath("~/CompanyImages/");
                            string SavePath = path + _fileName;
                            b.Save(SavePath, ImageFormat.Png);
                            user.image = _fileName;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                user.name = name;
                user.EmailAddress = emailAddress;
                user.Contact = contact;
                user.Address = address;
                user.username = userName;
                user.ModifiedBy = userId;
                user.ModifiedDate = DateTime.Now;
                user.isDeleted = "false";
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();

                Session["UserId"] = user.UserId.ToString();
                Session["Username"] = user.username.ToString();
                Session["Name"] = user.name.ToString();
                Session["EmailAddress"] = user.EmailAddress.ToString();
                Session["Contact"] = user.Contact.ToString();
                Session["Address"] = user.Address.ToString();
                Session["RoleType"] = user.RoleType.ToString();
                Session["RoleID"] = user.RoleID.ToString();
                Session["image"] = user.image == null ? "" : user.image.ToString();

                return "success";
            }
            return "error";
        }
    }
}