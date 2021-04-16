﻿using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SabiAsp.Controllers
{
    public class HomeController : Controller
    {
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
            try
            {
                ViewBag.Customercategories = Db.Categories.Select(d => new SelectListItem { Text = d.CategoryName, Value = d.CategoryId.ToString() });
            }
            catch (Exception)
            {

                
            }
           

            return View();
        }


        [AllowAnonymous]
        public ActionResult Facebook()
        {
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
            TempData["email"] = me.id;
            TempData["first_name"] = me.first_name;
            TempData["lastname"] = me.last_name;
            TempData["picture"] = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //private Methods

        [HttpGet]
        public ActionResult GetCategories(int id)
        {
            using (var db = new sabiShopEntities())
            {
                var data = db.Categories.Where(d => d.CategoryId == id).Select(d => new { Text = d.CategoryName, Value = d.CategoryId.ToString() }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}