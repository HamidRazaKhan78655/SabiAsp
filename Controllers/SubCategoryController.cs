﻿using SabiAsp.Models;
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
        public ActionResult GetAllSubCategories()
        {
            ViewBag.Categories = Db.Categories.Where(x => x.isDeleted != "true").ToList();
            var subCategory = Db.SubCategories.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetSubCategories", subCategory);
        }
        public string AddSubCategory(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string subCategoryName = fm["SubCategoryName"].ToString();
            string updateCategoryId = fm["CategoryDrp"].ToString();
            int categoryId = int.Parse(updateCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            var subCategory = Db.SubCategories.Where(x => x.name.ToLower() == subCategoryName.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (subCategory == null)
            {
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
        public string UpdateSubCategory(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string subCategoryName = fm["UpdateSubCategoryName"].ToString();
            string updateCategoryId = fm["UpdateCategoryDrp"].ToString();
            int categoryId = int.Parse(updateCategoryId);
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