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
    public class ItemsController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        // GET: Items
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult GetCategories(int id)
        {
            if (id.ToString() == "")
            {
                return Json(ShowAllItems(), JsonRequestBehavior.AllowGet);
            }

            using (var db = new sabiShopEntities())
            {
                var data = db.SubCategories.Where(d => d.CategoryId == id).Select(d => new { name = d.name, id = d.SubCategorieId.ToString(), image = d.image }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public ActionResult GetSubCategoriesItem(int id)
        {
            /*if (Session["Login"] == null)
            {
                var data =new { text=""};
                return Json(data, JsonRequestBehavior.AllowGet);
            }*/
            using (var db = new sabiShopEntities())
            {

               var  data = db.items.Where(d => d.SubCategorieId == id).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult ShowAllItems()
        {
            using (var db = new sabiShopEntities())
            {
                var data = db.items.Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image ,weight=d.Weight,price=d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        } 
        
        [HttpGet]
        public ActionResult GetSortList(int id)
        {
            using (var db = new sabiShopEntities())
            {
                
                if (id==1)
                {
                    var data= db.items.OrderBy(x => x.CreatedDate).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                
                    var data1 = db.items.OrderBy(x => x.Price).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                    return Json(data1, JsonRequestBehavior.AllowGet); 
            }
        }

        #region CRUD SubCategory
        public ActionResult GetAllItems()
        {
            ViewBag.Categories = Db.Categories.Where(x => x.isDeleted != "true").ToList();
            var subCategory = Db.SubCategories.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetSubCategories", subCategory);
        }
        public string AddItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["itemName"].ToString();
            string weight = fm["itemWeight"].ToString();
            string price = fm["itemPrice"].ToString();
            string updateshopId = fm["shopIddrp"].ToString();
            int shopId = int.Parse(updateshopId);
            string updateSubCategoryId = fm["SubCategorydrp"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            item item = new item();
            var itemData = Db.items.Where(x => x.name.ToLower() == name.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (item == null)
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
                        item.image = _fileName;
                    }
                }
                catch (Exception ex)
                {

                }
            }

                item.name = name;
                item.Weight = weight;
                item.Price = price;
                item.Shopid = shopId;
                item.SubCategorieId = subCategoryId;
                item.isDeleted = "false";
                item.CreatedDate = DateTime.Now;
                item.CreatedBy = logedinUserId;
                Db.items.Add(item);
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string UpdateItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["updateItemName"].ToString();
            string updateItemId = fm["updateItemId"].ToString();
            int itemId = int.Parse(updateItemId);
            string weight = fm["updateItemWeight"].ToString();
            string price = fm["updateItemPrice"].ToString();
            string updateshopId = fm["UpdateShopIddrp"].ToString();
            int shopId = int.Parse(updateshopId);
            string updateSubCategoryId = fm["UpdateSubCategorydrp"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            var item = Db.items.Where(x => x.ItemId == itemId && x.isDeleted != "true").SingleOrDefault();
            if (item != null)
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
                            item.image = _fileName;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                item.name = name;
                item.Weight = weight;
                item.Price = price;
                item.Shopid = shopId;
                item.SubCategorieId = subCategoryId;
                item.isDeleted = "false";
                item.ModifiedBy = logedinUserId;
                item.ModifiedDate = DateTime.Now;
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string DeleteItem(string itemId)
        {
            int itmId = int.Parse(itemId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            var item = Db.items.Where(x => x.ItemId == itmId && x.isDeleted != "true").SingleOrDefault();
            if (item != null)
            {
                item.ModifiedBy = logedinUserId;
                item.ModifiedDate = DateTime.Now;
                item.isDeleted = "true";
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        #endregion



    }
}