using Facebook;
using SabiAsp.Encryption;
using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        
        public ActionResult SabiLogin(string type = "")
        {
            if (type == "")
                ViewBag.Type = 0;
            else
                ViewBag.Type = Convert.ToInt32(type);

            if (type=="1")
            {
                ViewBag.subTitleLogin = " Partner Merchant Login";
            }
            else if (type == "2")
            {
                ViewBag.subTitleLogin = "Customer Login";
            }

            return View();
        }

        [HttpPost]
        public ActionResult SabiLogin(FormCollection admin)
        {
            HttpContext.Session["Connectionstring"] = System.Configuration.ConfigurationManager.ConnectionStrings["sabiShopEntities"].ToString();
            string username = admin["username"].ToString();
            string password = admin["pass"].ToString();

            var User = Db.users.Where(x => x.username.ToLower() == username.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (User != null)
            {
                /*if (User.RoleID == 3)
                {
                    var vendor = Db.vendors.Where(v => v.UserId == User.UserId && v.Status == "Accepted").SingleOrDefault();
                    if (vendor == null)
                        return View();
                }
                */
                string DecryptPassword = Encrypto.DecryptString(User.password);
                if (password == DecryptPassword)
                {
                    Session["UserId"] = User.UserId.ToString();
                    Session["Username"] = User.username.ToString();
                    Session["Name"] = User.name.ToString().Split(' ')[0];
                    Session["EmailAddress"] = User.EmailAddress.ToString();
                    Session["Contact"] = User.Contact.ToString();
                    Session["Address"] = User.Address.ToString();
                    Session["RoleType"] = User.RoleType.ToString();
                    Session["RoleID"] = User.RoleID.ToString();
                    Session["image"] = User.image == null ? "" : User.image.ToString();
                    Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                    Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";
                    FormsAuthentication.SetAuthCookie(User.username, false);
                    if (User.RoleID == 1)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else if (User.RoleID == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (User.RoleID == 3)
                    {
                        return RedirectToAction("VendorView", "Vendor");
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
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Session["UserId"] = 0;
            Session["Username"] = "";
            Session["Name"] = "";
            Session["RoleType"] = "";
            Session["RoleID"] = 0;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SabiRegister(int type)
        {
            ViewBag.type = type == 2 ? 2 : 3;
            ViewBag.Categories = Db.Categories.Where(x => x.isDeleted != "true").OrderByDescending(x => x.CreatedBy).ToList();
            return View();
        }
        [HttpPost]
        public string SabiRegister(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            try
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
                    if (roleId == 2)
                    {
                        u.RoleType = "User";
                    }
                    else
                    {
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
                        v.CreatedBy = u.UserId;
                        v.CreatedDate = DateTime.Now;
                        Db.vendors.Add(v);
                        Db.SaveChanges();

                        string Category = fm["Category"].ToString();
                        int categoryId = int.Parse(Category);
                        string location = fm["Location"].ToString();
                        string shopName = fm["ShopName"].ToString();
                        string description = fm["Description"].ToString();
                        string deliveryFee = fm["DeliveryFee"].ToString();
                        string deliveryTimeMax = fm["DeliveryTimeMax"].ToString();
                        string deliveryTimeMin = fm["DeliveryTimeMin"].ToString();
                        string minOrder = fm["MinOrder"].ToString();

                        Shop s = new Shop();
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
                                    if (i == 0)
                                    {
                                        s.image = _fileName;
                                    }
                                    else if (i == 1)
                                    {
                                        s.Logo = _fileName;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        s.vendorid = v.vendorid;
                        s.CategoryId = categoryId;
                        s.shopname = shopName;
                        s.Description = description;
                        s.DeliveryFee = deliveryFee;
                        s.DeliveryTime = deliveryTimeMin + " " + "-" + " " + deliveryTimeMax;
                        s.MinOrder = minOrder;
                        s.location = location;
                        s.isDeleted = "false";
                        s.CreatedBy = u.UserId;
                        s.CreatedDate = DateTime.Now;
                        Db.Shops.Add(s);
                        Db.SaveChanges();
                    }

                    Session["UserId"] = u.UserId.ToString();
                    Session["Username"] = u.username.ToString();
                    Session["Name"] = u.name.ToString().Split(' ')[0];
                    Session["RoleType"] = u.RoleType.ToString();
                    Session["RoleID"] = u.RoleID.ToString();
                    Session["DateFormate"] = "{0:MMM dd, yyyy HH:mm tt}";
                    Session["ShortDateFormate"] = "{0:MMM dd, yyyy}";
                    FormsAuthentication.SetAuthCookie(u.username, false);

                    return "success";
                }
            }
            catch (Exception ex)
            {

                return "error";
            }


            return "error";
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
                    Session["Name"] = User.name.Split(' ')[0];
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
                        Session["Name"] = name.Split(' ')[0];
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
                        Session["Name"] = name.Split(' ')[0];
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
                        Session["Name"] = name.Split(' ')[0];
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
                        Session["Name"] = name.Split(' ')[0];
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