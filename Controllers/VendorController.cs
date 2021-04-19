using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                int vId = int.Parse(loginID);
                var query = Db.vendors.Where(x=> x.vendorid == vId).SingleOrDefault();
                ViewBag.Login = query.vendorid.ToString();
                ViewBag.LoginName = query.name.ToString();
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
            int? vId = int.Parse(vendorid);
            int? cId = int.Parse(cate);
            var shop = Db.Shops.Where(x=> x.vendorid == vId).SingleOrDefault();
            var subCategories = Db.SubCategories.Where(x=> x.shopid == shop.Shopid && x.CategoryId == cId).ToList();
            ViewBag.subcats = subCategories;
            ViewBag.categoryid = cate;
            ViewBag.vendorid = vendorid;
            ViewBag.shopid = shop.Shopid;
            return View();
        }
        public ActionResult showVendorItems(string vendorid, string sCateID)
        {
            int? vId = int.Parse(vendorid);
            int? subId = int.Parse(sCateID);
            var allitems = Db.items.Where(x=> x.vendorid == vId && x.SubCategorieId == subId).ToList();
            ViewBag.items = allitems;
            ViewBag.sCateID = sCateID;
            ViewBag.vendorid = vendorid;
            return View();
        }
        public ActionResult AddVendorItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["itemName"].ToString();
            string weight = fm["itemWeight"].ToString();
            string price = fm["itemPrice"].ToString();
            string venid = fm["vendorid"].ToString();
            string sCateID = fm["sCateID"].ToString();

            item item = new item();

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
                        var path = Server.MapPath("~/CompanyImages/");
                        string SavePath = path + namefile;
                        b.Save(SavePath, ImageFormat.Png);
                        item.image = file.FileName;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            item.name = name;
            item.Weight = weight;
            item.Price = price;
            item.vendorid = int.Parse(venid);
            item.SubCategorieId = int.Parse(sCateID);
            item.isDeleted = "false";
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = int.Parse(venid);
            Db.items.Add(item);
            Db.SaveChanges();
            return RedirectToAction("showVendorItems", new { vendorid = venid, sCateID = sCateID });
        }
    }
}