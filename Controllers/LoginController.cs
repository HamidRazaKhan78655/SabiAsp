using Facebook;
using SabiAsp.Encryption;
using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SabiAsp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private static int UserTypeId = 0;
        sabiShopEntities Db = new sabiShopEntities();
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult Index()
        {
            
            return View();
        }  
        
        public ActionResult SabiLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SabiLogin(FormCollection admin)
        {
            HttpContext.Session["Connectionstring"] = System.Configuration.ConfigurationManager.ConnectionStrings["sabiShopEntities"].ToString();
            string username = admin["username"].ToString();
            string password = admin["pass"].ToString();

            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower()).FirstOrDefault();
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

                    if (User.RoleID == 1)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else if (User.RoleID == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult SabiRegister(string type)
        {
            if (type== "vendor")
            {
                ViewBag.type = "vendor";
            }
            else
            {
                ViewBag.type = "Buyer";
            }
            return View();
        }
        [HttpPost]
        public ActionResult SabiRegister(FormCollection formCollection)
        {
            if (true)
            {
                return RedirectToAction("index" , "Home");
            }
        }

        public string CheckSocialLogin(string email)
        {
            try
            {
                var User = Db.users.Where(x => x.username.ToLower() == email.ToLower() && x.EmailAddress == email.ToLower() && x.isDeleted != "true" && x.SocialLoginType != null).FirstOrDefault();
                if (User != null)
                {
                    Session["UserId"] = User.UserId.ToString();
                    Session["Username"] = User.username;
                    Session["Name"] = User.name;
                    Session["RoleType"] = User.RoleType;
                    Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                    Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                    return "success";
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception)
            {
                return "error";
            }
        
        }

        [HttpPost]
        public string  GoogleLogin(string email, string name, string gender, string lastname, string location,int id, string password)
        {
            try
            {
                vendor v = new vendor();
                user u = new user();
                if (id==1)
                {
                    //vendor
                    var User = Db.users.Where(x => x.username.ToLower() == email.ToLower() && x.EmailAddress == email.ToLower() && x.isDeleted != "true").FirstOrDefault();
                    if (User == null)
                    {
                        u.name = name;
                        u.username = email;
                        u.password = Encryption.Encrypto.EncryptString(password.Trim());
                        u.EmailAddress = email;
                        u.RoleID = 3;
                        u.RoleType = "Vendor";
                        u.isDeleted = "false";
                        u.CreatedBy = 1;
                        u.CreatedDate = DateTime.Now;
                        Db.users.Add(u);
                        Db.SaveChanges();

                        v.UserId = u.UserId;
                        v.isDeleted = "false";
                        v.CreatedBy = 1;
                        v.CreatedDate = DateTime.Now;
                        Db.vendors.Add(v);
                        Db.SaveChanges();

                        Shop s = new Shop();
                        s.vendorid = v.vendorid;
                        if (name.Contains("'"))
                            s.shopname = name + " " + "Shop";
                        else
                            s.shopname = name + "'" + " " + "Shop";

                        s.isDeleted = "false";
                        s.CreatedBy = 1;
                        s.CreatedDate = DateTime.Now;
                        Db.Shops.Add(s);
                        Db.SaveChanges();

                        Session["UserId"] = u.UserId.ToString();
                        Session["Username"] = email;
                        Session["Name"] = name;
                        Session["RoleType"] = "Vendor";
                        Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                        Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                        return "success";
                    }
                    return "error";
                }
                else if (id==2)
                {
                    //Buyer
                    var User = Db.users.Where(x => x.username.ToLower() == email.ToLower() && x.EmailAddress == email.ToLower() && x.isDeleted != "true").FirstOrDefault();
                    if (User == null)
                    {
                        u.name = name;
                        u.username = email;
                        u.EmailAddress = email;
                        u.password = Encryption.Encrypto.EncryptString(password.Trim());
                        u.RoleID = 2;
                        u.RoleType = "User";
                        u.isDeleted = "false";
                        u.CreatedBy = 1;
                        u.CreatedDate = DateTime.Now;
                        Db.users.Add(u);
                        Db.SaveChanges();

                        Session["UserId"] = u.UserId.ToString();
                        Session["Username"] = email;
                        Session["Name"] = name;
                        Session["RoleType"] = "User";
                        Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                        Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                        return "success";
                    }
                    return "error";
                }
                
            }
            catch (Exception ex)
            {

                return "error";
            }

            return "error";
        }


        [AllowAnonymous]
        public ActionResult Facebook(int UserType)
        {
            UserTypeId = UserType;
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "504519300582045",
                client_secret = "31c411cace2d50c76e6f3e2332312728",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "504519300582045",
                client_secret = "31c411cace2d50c76e6f3e2332312728",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            UserFacebookInfoModal userFacebookInfo = new UserFacebookInfoModal();
            dynamic me = fb.Get("me?fields=link,id,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            userFacebookInfo.UserTypeId = UserTypeId;
            userFacebookInfo.EmailAddress = me.email;
            userFacebookInfo.Username = me.id;
            userFacebookInfo.Name = me.first_name + " " + me.last_name;
            userFacebookInfo.Image = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(userFacebookInfo.EmailAddress, false);

            //user u = new user();
            //if (UserTypeId==1)
            //{
            //    //Vendor
            //    try
            //    {
            //        vendor v = new vendor();
            //        u.name = me.first_name + " " + me.last_name;
            //        u.username = me.first_name + " " + me.last_name;
            //        u.password = me.email;
            //        u.RoleID = 3;
            //        u.RoleType = "Vendor";
            //        u.isDeleted = "false";
            //        u.CreatedBy = 1;
            //        u.CreatedDate = DateTime.Now;
            //        Db.users.Add(u);
            //        Db.SaveChanges();

            //        v.UserId = u.UserId;
            //        v.isDeleted = "false";
            //        v.CreatedBy = 1;
            //        v.CreatedDate = DateTime.Now;
            //        Db.vendors.Add(v);
            //        Db.SaveChanges();

            //    }
            //    catch (Exception e)
            //    {
            //        var r = e;

            //    }


            //}
            //else if (UserTypeId==2)
            //{
            //    //Buyer
            //    try
            //    {
            //        u.UserId = 1;
            //        u.name = me.first_name + me.last_name;
            //        u.password = me.email;
            //        u.RoleID = 2;
            //        u.RoleType = "User";
            //        u.isDeleted = "false";
            //        u.CreatedBy = 1;
            //        u.CreatedDate = DateTime.Now;
            //        Db.users.Add(u);
            //        Db.SaveChanges();
            //    }
            //    catch (Exception)
            //    {


            //    }
            //}

            return View(userFacebookInfo);
        }

        public string CreateFacebookPassword(int id, string email, string username, string name, string image, string password) 
        {
            try
            {
                vendor v = new vendor();
                user u = new user();
                if (id == 1)
                {
                    //vendor
                    var User = Db.users.Where(x => x.username.ToLower() == email.ToLower() && x.EmailAddress == email.ToLower() && x.isDeleted != "true").FirstOrDefault();
                    if (User == null)
                    {
                        u.name = name;
                        u.username = username;
                        u.password = Encryption.Encrypto.EncryptString(password.Trim());
                        u.EmailAddress = email;
                        u.image = image;
                        u.RoleID = 3;
                        u.RoleType = "Vendor";
                        u.isDeleted = "false";
                        u.CreatedBy = 1;
                        u.CreatedDate = DateTime.Now;
                        Db.users.Add(u);
                        Db.SaveChanges();

                        v.UserId = u.UserId;
                        v.isDeleted = "false";
                        v.CreatedBy = 1;
                        v.CreatedDate = DateTime.Now;
                        Db.vendors.Add(v);
                        Db.SaveChanges();

                        Shop s = new Shop();
                        s.vendorid = v.vendorid;
                        if (name.Contains("'"))
                            s.shopname = name + " " + "Shop";
                        else
                            s.shopname = name + "'" + " " + "Shop";

                        s.isDeleted = "false";
                        s.CreatedBy = 1;
                        s.CreatedDate = DateTime.Now;
                        Db.Shops.Add(s);
                        Db.SaveChanges();

                        Session["UserId"] = u.UserId.ToString();
                        Session["Username"] = email;
                        Session["Name"] = name;
                        Session["RoleType"] = "Vendor";
                        Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                        Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                        return "success";
                    }
                    return "error";
                }
                else if (id == 2)
                {
                    //Buyer
                    var User = Db.users.Where(x => x.username.ToLower() == email.ToLower() && x.EmailAddress == email.ToLower() && x.isDeleted != "true").FirstOrDefault();
                    if (User == null)
                    {
                        u.name = name;
                        u.username = username;
                        u.EmailAddress = email;
                        u.password = Encryption.Encrypto.EncryptString(password.Trim());
                        u.image = image;
                        u.RoleID = 2;
                        u.RoleType = "User";
                        u.isDeleted = "false";
                        u.CreatedBy = 1;
                        u.CreatedDate = DateTime.Now;
                        Db.users.Add(u);
                        Db.SaveChanges();

                        Session["UserId"] = u.UserId.ToString();
                        Session["Username"] = email;
                        Session["Name"] = name;
                        Session["RoleType"] = "User";
                        Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                        Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";

                        return "success";
                    }
                    return "error";
                }

            }
            catch (Exception ex)
            {

                return "error";
            }

            return "error";
        }

    }
}