using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class CategoriesController : Controller
    {

        sabiShopEntities Db = new sabiShopEntities();

        private static List<SelectListItem> list;
        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetSubCategoriesByCategory(int id, string name)
        {
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            //if (logedinUserId == 0)
            //{
            //    return RedirectToAction("SabiLogin", "Login");
            //}
            int logedinRoleID = Convert.ToInt32(Session["RoleID"]);
            ViewBag.Category = name;
            ViewBag.logedinRoleID = logedinRoleID;
            //if (logedinRoleID == 2)
            //{
            //    ViewBag.itemsList = Db.items.OrderByDescending(x => x.CreatedBy).ToList();
            //    return View();
            //}
            //else
            //{
            var location = Session["Location"].ToString();
            ViewBag.CategoryId = id;
            ViewBag.SubCategorylist = Db.Shops.Where(d => d.CategoryId == id && d.location == location).ToList();
            return View();
            //}
        }

        public ActionResult GetSubCategories(string location, string shopname, int shopid)
        {
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            int logedinRoleID = Convert.ToInt32(Session["RoleID"]);
            var shop = Db.Shops.Where(d => d.Shopid == shopid).FirstOrDefault();
            ViewBag.ShopData = shop;
            var subCategory = Db.SubCategories.Where(x => x.Shopid == shopid).ToList();
            ViewBag.SubCategorylist = subCategory;
            var itemList = new List<SubCategoryItems>();
            foreach (var sub in subCategory)
            {
                var sci = new SubCategoryItems();
                var it = Db.items.Where(x => x.SubCategorieId == sub.SubCategorieId && x.isDeleted != "true").ToList();
                sci.SubCategorieId = sub.SubCategorieId;
                sci.name = sub.name;
                sci.items = it;
                itemList.Add(sci);
            }

            ViewBag.Category = shopname;
            ViewBag.SubCategoryId = shopid;
            ViewBag.Ratings = Convert.ToInt32(shop.Ratings);
            ViewBag.WithoutRatings = 5 - ViewBag.Ratings;
            ViewBag.logedinUserId = logedinUserId;
            ViewBag.logedinRoleID = logedinRoleID;
            //ViewBag.SubCategorylist = Db.SubCategories.Where(d => d.Shopid == shopid).Select(d => new SelectListItem{ Text = d.name, Value = d.SubCategorieId.ToString() }).ToList();
            //list = ViewBag.SubCategorylist;
            //ViewBag.SCats = items;

            return View(itemList);
        } 
        public ActionResult GetSubCategoriesItem(int id, string name)
        {
            var query = Db.items.Where(d => d.SubCategorieId == id).Select(d => d).ToList();
            List<item> items = new List<item>();            
            foreach (var SCate in query)
            {
                items.Add(SCate);   
            }
            ViewBag.Category = name;
                //Db.SubCategories.Where(d => d.SubCategorieId == id).Select(t=>t.name).FirstOrDefault().ToString();
            ViewBag.SubCategorylist = list;
            
            ViewBag.SCats = items;
            return View("GetSubCategories");
        }



        public ActionResult getItemsByCategories(int id , string name)
        {
            var query = Db.items.ToList();
            List<item> items = new List<item>();
            foreach (var item in query)
            {
                if (item.SubCategorieId == id)
                {
                    items.Add(item);
                }

            }
            ViewBag.SCategory = name;
            ViewBag.items = items;
            return View();
        }

        #region CRUD Category
        public ActionResult GetAllCategories()
        {
            var category = Db.Categories.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetCategories", category);
        }
        public string AddCategory(FormCollection fm)
        {
            string categoryName = fm["CategoryName"].ToString();
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var category = Db.Categories.Where(x => x.CategoryName.ToLower() == categoryName.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (category == null)
            {
                Category c = new Category();
                c.CategoryName = categoryName;
                c.CreatedBy = logedinUserId;
                c.CreatedDate = DateTime.Now;
                c.isDeleted = "false";
                Db.Categories.Add(c);
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string UpdateCategory(FormCollection fm)
        {
            string categoryName = fm["UpdateCategoryName"].ToString();
            string updateCategoryId = fm["UpdateCategoryId"].ToString();
            int categoryId = int.Parse(updateCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var category = Db.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            if (category != null)
            {
                category.CategoryName = categoryName;
                category.ModifiedBy = logedinUserId;
                category.ModifiedDate = DateTime.Now;
                category.isDeleted = "false";
                Db.Entry(category).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string DeleteCategory(string categoryId)
        {
            int cId = int.Parse(categoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            var category = Db.Categories.Where(x => x.CategoryId == cId).FirstOrDefault();
            if (category != null)
            {
                category.ModifiedBy = logedinUserId;
                category.ModifiedDate = DateTime.Now;
                category.isDeleted = "true";
                Db.Entry(category).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        #endregion
        // Post: Categories
    }
}