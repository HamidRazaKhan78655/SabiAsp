using Facebook;
using SabiAsp.Models;
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
        
        public ActionResult Index(string type = "")
        {
            try
            {
                Session["Login"] = "";
                Session["Location"] = type == "" ? "HRE" : type;
                //  ViewBag.Customercategories = Db.Categories.Select(d => new SelectListItem { Text = d.CategoryName, Value = d.CategoryId.ToString() });
                /*  var query = Db.SubCategories.ToList();
                  List<SubCategory> subCategories = new List<SubCategory>();
                      foreach (var SCate in query)
                      {
                          subCategories.Add(SCate);

                      }
                      ViewBag.Category = "All Items";
                      ViewBag.SCats = subCategories;
                      return View("GetSubCategories");*/

            }
            catch (Exception)
            {

                
            }
            return View();
        }
        public ActionResult SearchItems(string Name)
        {
            var SearchList = new List<item>();
            if (!String.IsNullOrEmpty(Name))
            {
                 SearchList = Db.items.Where(s => s.name.Contains(Name)).ToList();
            }
            ViewBag.items = SearchList;
            return PartialView();
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

       


    }
}