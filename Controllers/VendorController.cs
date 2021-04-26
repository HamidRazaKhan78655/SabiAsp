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
        public ActionResult SubCategories(string vendorid, string cate)
        {
            int? vId = int.Parse(vendorid);
            int? cId = int.Parse(cate);
            var shop = Db.Shops.Where(x => x.vendorid == vId).SingleOrDefault();
            var subCategories = Db.SubCategories.Where(x => x.shopid == shop.Shopid && x.CategoryId == cId && x.isDeleted != "true").ToList();
            ViewBag.subcats = subCategories;
            ViewBag.categoryid = cate;
            ViewBag.vendorid = vendorid;
            ViewBag.shopid = shop.Shopid;
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
            subCategory.CreatedBy = int.Parse(venid);
            subCategory.CreatedDate = DateTime.Now;
            subCategory.isDeleted = "false";
            Db.SubCategories.Add(subCategory);
            Db.SaveChanges();
            return RedirectToAction("SubCategories", new { vendorid = venid , cate = categoryid });
        }
        public ActionResult UpdateSubCategory(FormCollection fm)
        {
            string name = fm["subCategoryName"].ToString();
            string venid = fm["vendorid"].ToString();
            string shopid = fm["shopid"].ToString();
            string categoryid = fm["categoryid"].ToString();
            string updateSubCategoryId = fm["updateSubCategoryId"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);

            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == subCategoryId && x.isDeleted != "true").SingleOrDefault();
            if (subCategory != null)
            {
                subCategory.shopid = int.Parse(shopid);
                subCategory.CategoryId = int.Parse(categoryid);
                subCategory.name = name;
                subCategory.ModifiedBy = int.Parse(venid);
                subCategory.ModifiedDate = DateTime.Now;
                subCategory.isDeleted = "false";
                Db.Entry(subCategory).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("SubCategories", new { vendorid = venid , cate = categoryid });
        }
        public ActionResult DeleteSubCategory(string subCategoryId, string vendorId, string categoryId)
        {
            int subCatId = int.Parse(subCategoryId);
            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == subCatId && x.isDeleted != "true").SingleOrDefault();
            if (subCategory != null)
            {
                subCategory.ModifiedBy = int.Parse(vendorId);
                subCategory.ModifiedDate = DateTime.Now;
                subCategory.isDeleted = "true";
                Db.Entry(subCategory).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("SubCategories", new { vendorid = vendorId, cate = categoryId });
        }
        public ActionResult showVendorItems(string vendorid, string sCateID)
        {
            int? vId = int.Parse(vendorid);
            int? subId = int.Parse(sCateID);
            var allitems = Db.items.Where(x=> x.vendorid == vId && x.SubCategorieId == subId && x.isDeleted != "true").ToList();
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
        public ActionResult UpdateVendorItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["updateItemName"].ToString();
            string updateItemId = fm["updateItemId"].ToString();
            int itemId = int.Parse(updateItemId);
            string weight = fm["updateItemWeight"].ToString();
            string price = fm["updateItemPrice"].ToString();
            string venid = fm["vendorid"].ToString();
            string sCateID = fm["sCateID"].ToString();
            string _fileName = string.Empty;

            var item = Db.items.Where(x => x.ItemId == itemId && x.isDeleted != "true").SingleOrDefault();

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
                        item.image = _fileName;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (item != null)
            {
                item.name = name;
                item.Weight = weight;
                item.Price = price;
                item.vendorid = int.Parse(venid);
                item.SubCategorieId = int.Parse(sCateID);
                item.isDeleted = "false";
                item.ModifiedBy = int.Parse(venid);
                item.ModifiedDate = DateTime.Now;
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("showVendorItems", new { vendorid = venid, sCateID = sCateID });
        }
        public ActionResult DeleteVendorItem(string itemId, string vendorId, string subCategoryId)
        {
            int id = int.Parse(itemId);
            var item = Db.items.Where(x => x.ItemId == id && x.isDeleted != "true").SingleOrDefault();
            if (item != null)
            {
                item.isDeleted = "true";
                item.ModifiedBy = int.Parse(vendorId);
                item.ModifiedDate = DateTime.Now;
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("showVendorItems", new { vendorid = vendorId, sCateID = subCategoryId });
        }

        #region CRUD for Vendor
        public ActionResult GetAllVendor()
        {
            ViewBag.vendors = Db.vendors.ToList();
            return View();
        }

        public ActionResult AddVendor(FormCollection fm)
        {
            string firstName = fm["FirstName"].ToString();
            string lastName = fm["LastName"].ToString();
            //string adminId = fm["adminId"].ToString();
            string username = fm["Username"].ToString();
            string emailAddress = fm["EmailAddress"].ToString();
            string contact = fm["Contact"].ToString();
            string address = fm["Address"].ToString();
            string password = fm["Password"].ToString();
            vendor v = new vendor();
            v.name = firstName + " " + lastName;
            v.EmailAddress = emailAddress;
            v.Contact = contact;
            v.username = username;
            v.password = Encryption.Encrypto.EncryptString(password.Trim());
            v.Address = address;
            //u.RoleID = 2;
            v.CreatedBy = 1;
            v.CreatedDate = DateTime.Now;
            v.isDeleted = "false";
            Db.vendors.Add(v);
            Db.SaveChanges();
            return RedirectToAction("GetAllVendor");
        }
        public ActionResult UpdateVendor(FormCollection fm)
        {
            string name = fm["UpdateVendorName"].ToString();
            string emailAddress = fm["UpdateVendorEmailAddress"].ToString();
            string contact = fm["UpdateVendorContact"].ToString();
            string address = fm["UpdateVendorAddress"].ToString();
            string userName = fm["UpdateVendorUsername"].ToString();
            string updateVendorId = fm["UpdateVendorId"].ToString();
            int vendorId = int.Parse(updateVendorId);

            var vendor = Db.vendors.Where(x => x.vendorid == vendorId).SingleOrDefault();
            if (vendor != null)
            {
                vendor.name = name;
                vendor.EmailAddress = emailAddress;
                vendor.Contact = contact;
                vendor.Address = address;
                vendor.username = userName;
                vendor.ModifiedBy = 1;
                vendor.ModifiedDate = DateTime.Now;
                vendor.isDeleted = "false";
                Db.Entry(vendor).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("GetAllVendor");
        }
        public ActionResult DeleteVendor(string vendorId)
        {
            int vId = int.Parse(vendorId);
            var vendor = Db.vendors.Where(x => x.vendorid == vId && x.isDeleted != "true").SingleOrDefault();
            if (vendor != null)
            {
                vendor.ModifiedBy = 1;
                vendor.ModifiedDate = DateTime.Now;
                vendor.isDeleted = "true";
                Db.Entry(vendor).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("GetAllVendor");
        }
        #endregion
    }
}