using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class VendorController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        string login = "1";
        string loginID = "1";
        // GET: Vendor
        public ActionResult Index()
        {
            if (login == "none")
            {
                ViewBag.Login = "none";
            }
            else {
                var query = Db.vendors.ToList();
                foreach (vendor ven in query) {
                    if (ven.vendorid.ToString().Trim().Equals(loginID.ToString())) {
                        ViewBag.Login = ven.vendorid.ToString(); ;
                        ViewBag.LoginName = ven.name.ToString(); ;
                    }
                }
            }

            return View();
        }
        public ActionResult Categories(string id)
        {
            ViewBag.id = id;
            ViewBag.Categories = Db.Categories.ToList();
            return View();
        }
        public ActionResult AddSubCategories(FormCollection fm)
        {
            string name = fm["SCateName"].ToString();
            string venid = fm["vendorid"].ToString();
            string shopid = fm["shopid"].ToString();
            string categoryid = fm["categoryid"].ToString();
            SubCategory subCategory = new SubCategory();
            subCategory.shopid = int.Parse(shopid);
            subCategory.CategoryId = int.Parse(categoryid);
            subCategory.name = name;
            Db.SubCategories.Add(subCategory);
            Db.SaveChanges();
            return RedirectToAction("SubCategories", new { vendorid = venid , cate = categoryid });
        }
        public ActionResult SubCategories(string vendorid , string cate)
        {
            var shops = Db.Shops.ToList();
            var subcats = Db.SubCategories.ToList();
            Shop shop1 = new Shop() ;
            List<SubCategory> subs = new List<SubCategory>();
            foreach (var shop in shops) {
                if (shop.vendorid.ToString().Equals(vendorid)) {
                    shop1 = shop;
                }
            }
            foreach (var subcat in subcats) {
                if (subcat.shopid.ToString().Equals(shop1.Shopid.ToString()) && subcat.CategoryId.ToString().Equals(cate.ToString())) {
                    subs.Add(subcat);
                }
            }
            ViewBag.subcats = subs;
            ViewBag.categoryid = cate;
            ViewBag.vendorid = vendorid;
            ViewBag.shopid = shop1.Shopid;
            return View();
        }
        public ActionResult showVendorItems(string vendorid, string sCateID)
        {
            List<item> items = new List<item>();

            var allitems = Db.items.ToList();
            foreach (var item in allitems) {
                if (vendorid.Equals(item.vendorid.ToString()) && sCateID.Equals(item.SubCategorieId.ToString())) {
                    items.Add(item);
                }
            }
            ViewBag.items = items;
            ViewBag.sCateID = sCateID;
            ViewBag.vendorid = vendorid;

            return View();
        }
        public ActionResult AddVendorItem(FormCollection fm)
        {
            string name = fm["itemname"].ToString();
            string venid = fm["vendorid"].ToString();
            string sCateID = fm["sCateID"].ToString();

            item item = new item();
            item.name = name;
            item.vendorid = int.Parse(venid);
            item.SubCategorieId = int.Parse(sCateID);
            Db.items.Add(item);
            Db.SaveChanges();
            return RedirectToAction("showVendorItems", new { vendorid = venid, sCateID = sCateID });
        }
    }
}