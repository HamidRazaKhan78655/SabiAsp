using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ActionResult GetAllSubCategories()
        {
            ViewBag.Categories = Db.Categories.Where(x => x.isDeleted != "true").ToList();
            var subCategory = Db.SubCategories.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetSubCategories", subCategory);
        }
        public string AddSubCategory(FormCollection fm)
        {
            string subCategoryName = fm["SubCategoryName"].ToString();
            string updateCategoryId = fm["CategoryDrp"].ToString();
            int categoryId = int.Parse(updateCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var subCategory = Db.SubCategories.Where(x => x.name.ToLower() == subCategoryName.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (subCategory == null)
            {
                SubCategory sc = new SubCategory();
                sc.name = subCategoryName;
                sc.CategoryId = categoryId;
                sc.CreatedBy = logedinUserId;
                sc.CreatedDate = DateTime.Now;
                sc.isDeleted = "false";
                Db.SubCategories.Add(sc);
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string UpdateSubCategory(FormCollection fm)
        {
            string subCategoryName = fm["UpdateSubCategoryName"].ToString();
            string updateCategoryId = fm["UpdateCategoryId"].ToString();
            int categoryId = int.Parse(updateCategoryId);
            string updateSubCategoryId = fm["UpdateSubCategoryId"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == subCategoryId).FirstOrDefault();
            if (subCategory != null)
            {
                subCategory.name = subCategoryName;
                subCategory.CategoryId = categoryId;
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
    }
}