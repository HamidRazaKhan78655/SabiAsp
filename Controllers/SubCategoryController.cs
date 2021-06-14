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
    public class SubCategoryController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        // GET: SubCategory
        public ActionResult Index()
        {
            return View();
        }

        #region CRUD SubCategory
        public ActionResult GetAllSubCategoriesByCategory(int shopId)
        {
            var subCategory = new List<SubCategory>();
            if (shopId == 0)
                subCategory = Db.SubCategories.Where(x => x.isDeleted != "true").ToList();
            else
                subCategory = Db.SubCategories.Where(x => x.Shopid == shopId && x.isDeleted != "true").ToList();

            return PartialView("ShowSubCategoriesByCategory", subCategory);
        }
        public ActionResult GetSubCategoriesByShopId(int userId)
        {
            var vendor = Db.vendors.Where(x => x.UserId == userId).SingleOrDefault();
            var shop = Db.Shops.Where(x => x.vendorid == vendor.vendorid).SingleOrDefault();
            var subCategory = Db.SubCategories.Where(x => x.Shopid == shop.Shopid && x.isDeleted != "true").ToList();
            return PartialView("GetShopSubCategories", subCategory);
        }
        public ActionResult GetAllSubCategoriesListByShopId(int shopid)
        {
            var subCategory = Db.SubCategories.Where(x => x.Shopid == shopid && x.isDeleted != "true").ToList();
            return PartialView("GetAllSubCategoriesListByShopId", subCategory);
        }
        public ActionResult GetAllSubCategories()
        {
            var subCategory = Db.SubCategories.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetSubCategories", subCategory);
        }
        public string AddSubCategory(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string subCategoryName = fm["SubCategoryName"].ToString();
            string VendorShopDrp = fm["VendorShopDrp"].ToString();
            int shopId = int.Parse(VendorShopDrp);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            SubCategory sc = new SubCategory();
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
                        sc.image = _fileName;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            sc.name = subCategoryName;
            sc.Shopid = shopId;
            sc.CreatedBy = logedinUserId;
            sc.CreatedDate = DateTime.Now;
            sc.isDeleted = "false";
            Db.SubCategories.Add(sc);
            Db.SaveChanges();
            return "success";
        }
        public string UpdateSubCategory(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string subCategoryName = fm["UpdateSubCategoryName"].ToString();
            string UpdateSVendorShopDrp = fm["UpdateSVendorShopDrp"].ToString();
            int shopId = int.Parse(UpdateSVendorShopDrp);
            string updateSubCategoryId = fm["UpdateSubCategoryId"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == subCategoryId).FirstOrDefault();
            if (subCategory != null)
            {
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
                            subCategory.image = _fileName;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                subCategory.name = subCategoryName;
                subCategory.Shopid = shopId;
                subCategory.ModifiedBy = logedinUserId;
                subCategory.ModifiedDate = DateTime.Now;
                subCategory.isDeleted = "false";
                Db.Entry(subCategory).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string DeleteSubCategory(string subCategoryId)
        {
            int scId = int.Parse(subCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == scId).FirstOrDefault();
            if (subCategory != null)
            {
                subCategory.ModifiedBy = logedinUserId;
                subCategory.ModifiedDate = DateTime.Now;
                subCategory.isDeleted = "true";
                Db.Entry(subCategory).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        #endregion

        public ActionResult SortSubCategoryList(string value, string categoryId)
        {
            int cId = int.Parse(categoryId);
            var subCategory = new List<SubCategory>();
            if (value == "1")
            {
                subCategory = Db.SubCategories.Where(x => x.Shopid == cId && x.isDeleted != "true").OrderByDescending(x => x.CreatedDate).ToList();
            }
            else if (value == "2")
            {
                subCategory = Db.SubCategories.Where(x => x.Shopid == cId && x.isDeleted != "true").OrderBy(x => x.name).ToList();
            }
            else
            {
                subCategory = Db.SubCategories.Where(x => x.Shopid == cId && x.isDeleted != "true").ToList();
            }

            return PartialView("SortedSubCategories", subCategory);
        }

    }
}