using Facebook;
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
        
        public ActionResult SabiLogin(string type)
        {
            if (type=="1")
            {
                ViewBag.type = "vendor";
            }
            else if (type=="2")
            {
                ViewBag.type = "Buyer";
            }
            return View();
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

        [HttpPost]
        public string  GoogleLogin(string email, string name, string gender, string lastname, string location,int id)
        {
            try
            {
                if (id==1)
                {
                    //vendor
                    vendor v = new vendor();
                    v.vendorid = 6;
                    v.name = name;
                    v.password = email;
                    v.isDeleted = "false";
                    Db.vendors.Add(v);
                    Db.SaveChanges();
                    return "Ok";
                }
                else if (id==2)
                {
                    //Buyer
                    return "Ok";
                }
                return "";
            }
            catch (Exception)
            {

               
            }

            return "";
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
            dynamic me = fb.Get("me?fields=link,id,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;
            string id = me.id;
            TempData["email"] = me.id;
            Session["Login"] = me.id;
            TempData["first_name"] = me.first_name;
            TempData["lastname"] = me.last_name;
            TempData["picture"] = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(email, false);
            if (UserTypeId==1)
            {
                //Vendor
                try
                {
                    vendor v = new vendor();
                    v.vendorid =5;
                    v.name = me.first_name+" "+me.last_name;
                    v.password = me.email;
                    v.isDeleted = "false";
                    Db.vendors.Add(v);
                    Db.SaveChanges();
                }
                catch (Exception e)
                {
                    var r = e;
                    
                }


            }
            else if (UserTypeId==2)
            {
                //Buyer
                try
                {
                    user u = new  user();
                    u.id = 1;
                    u.name = me.first_name + me.last_name;
                    u.password = me.email;
                    u.RoleID = 1;
                    u.isDeleted = "false";
                    Db.users.Add(u);
                    Db.SaveChanges();
                }
                catch (Exception)
                {

                    
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}