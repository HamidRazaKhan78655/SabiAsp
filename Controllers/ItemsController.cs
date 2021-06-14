using Newtonsoft.Json;
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
        
        public ActionResult ItemView(int itemID)
        {
            var query = Db.items.Where(x => x.ItemId == itemID).FirstOrDefault();
            var querySub = Db.SubCategories.Where(x => x.SubCategorieId == query.SubCategorieId).FirstOrDefault();
            var queryShop = Db.Shops.Where(x => x.Shopid == querySub.Shopid).FirstOrDefault();
            ItemViewGen itemView = new ItemViewGen();
            itemView.ShopName = queryShop.shopname;
            itemView.SubCategoryName = querySub.name;
            itemView.itemDetails = query;
            return PartialView("ItemView", itemView);
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
                var data = db.SubCategories.Where(d => d.Shopid == id).Select(d => new { name = d.name, id = d.SubCategorieId.ToString(), image = d.image }).ToList();
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

                var data = db.items.Where(d => d.SubCategorieId == id).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult ShowAllItems()
        {
            using (var db = new sabiShopEntities())
            {
                var data = db.items.Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetSortList(int id)
        {
            using (var db = new sabiShopEntities())
            {

                if (id == 1)
                {
                    var data = db.items.OrderBy(x => x.CreatedDate).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var data1 = db.items.OrderBy(x => x.Price).Select(d => new { name = d.name, id = d.ItemId.ToString(), image = d.image, weight = d.Weight, price = d.Price }).ToList();
                return Json(data1, JsonRequestBehavior.AllowGet);
            }
        }

        #region CRUD Item
        public ActionResult GetAllItemsBySubCategory(int subCategorieId)
        {
            var item = new List<item>();
            if (subCategorieId == 0)
                item = Db.items.Where(x => x.SubCategorieId == subCategorieId && x.isDeleted != "true").ToList();
            else
                item = Db.items.Where(x => x.isDeleted != "true").ToList();

            return PartialView("GetItems", item);
        }
        public ActionResult GetAllItems()
        {
            var item = Db.items.Where(x => x.isDeleted != "true").ToList();
            return PartialView("GetItems", item);
        }
        public string AddShopItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["shopItemName"].ToString();
            string weight = fm["shopItemWeight"].ToString();
            string price = fm["shopItemPrice"].ToString();
            string updateShopUserId = fm["shopUserId"].ToString();
            int shopUserId = int.Parse(updateShopUserId);
            string updateSubCategoryId = fm["shopSubCategorydrp"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            item item = new item();
            var itemData = Db.items.Where(x => x.name.ToLower() == name.ToLower() && x.isDeleted != "true").FirstOrDefault();
            if (itemData == null)
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
                var vendor = Db.vendors.Where(x => x.UserId == shopUserId).SingleOrDefault();
                var shop = Db.Shops.Where(x => x.vendorid == vendor.vendorid).SingleOrDefault();

                item.name = name;
                item.Weight = weight;
                item.Price = price;
                item.SubCategorieId = shop.Shopid;
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
        public string AddItem(IEnumerable<HttpPostedFileBase> files, FormCollection fm)
        {
            string name = fm["itemName"].ToString();
            string weight = fm["itemWeight"].ToString();
            string price = fm["itemPrice"].ToString();
            string updateSubCategoryId = fm["SubCategorydrp"].ToString();
            int subCategoryId = int.Parse(updateSubCategoryId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            string _fileName = string.Empty;

            item item = new item();
            var itemData = Db.items.Where(x => x.name.ToLower() == name.ToLower() && x.SubCategorieId == subCategoryId && x.isDeleted != "true").FirstOrDefault();
            if (itemData == null)
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
                item.SubCategorieId = shopId;
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

        public string GetShopName(int userId)
        {
            var vendor = Db.vendors.Where(x => x.UserId == userId).SingleOrDefault();
            var shop = Db.Shops.Where(x => x.vendorid == vendor.vendorid).SingleOrDefault();
            return shop.shopname;
        }
        public ActionResult SortItemList(string value, string subCategoryId)
        {
            int subCId = int.Parse(subCategoryId);
            var itemList = new List<item>();
            if (value == "1")
            {
                itemList = Db.items.Where(x => x.SubCategorieId == subCId && x.isDeleted != "true").OrderByDescending(x => x.CreatedDate).ToList();
            }
            else if (value == "2")
            {
                itemList = Db.items.Where(x => x.SubCategorieId == subCId && x.isDeleted != "true").OrderByDescending(x => x.name).ToList();
            }
            else
            {
                itemList = Db.items.Where(x => x.SubCategorieId == subCId && x.isDeleted != "true").ToList();
            }

            return PartialView("SortedItems", itemList);
        }
        [HttpGet]
        public ActionResult BuyItemView(int ? Userid=0)
        {
            List<Cart> cartList = new List<Cart>();
   
            if (Userid==0)
            {
                ViewBag.noItemSelect = "";
            }
            else
            {
                
                var itemslist = Db.UserItemCards.Where(x => x.UesrId == Userid && x.isDeleted != "true").ToList();
                foreach (var item in itemslist) {
                    Cart crt = new Cart();
                    var itemselect = Db.items.Where(x=>x.ItemId == item.ItemId).FirstOrDefault();
                    var subCatelect = Db.SubCategories.Where(x => x.SubCategorieId == itemselect.SubCategorieId).FirstOrDefault();
                    var shopSelect = Db.Shops.Where(x => x.Shopid == subCatelect.Shopid).FirstOrDefault();
                    crt.itemDetails = itemselect;
                    crt.Shop = shopSelect;
                    crt.SubCat = subCatelect;
                    crt.cartDetail = item;
                    cartList.Add(crt);
                }


            }
            
            return View(cartList);
        }
        public string AddtoCart(string itemId, string userID)
        {
            int itemID = int.Parse(itemId);
            int userId = int.Parse(userID);
            UserItemCard cartitem = new UserItemCard();
            var itemSelected = Db.items.Where(x => x.ItemId == itemID).FirstOrDefault();
            var UserSelected = Db.users.Where(x => x.UserId == userId).FirstOrDefault();
            cartitem.ItemId = itemSelected.ItemId;
            cartitem.UesrId = UserSelected.UserId;
            cartitem.quantity = 1;
            try {
                Db.UserItemCards.Add(cartitem);
                Db.SaveChanges();
                return JsonConvert.SerializeObject("ok");
            }
            catch {
                return JsonConvert.SerializeObject("no");
            }
        }

        public bool SaveBuyProducts(string userid,string itemid)
        {
            try
            {
                int uid = int.Parse(userid);
                int iid= int.Parse(itemid);
                if (userid != null && itemid != null || userid != "" && itemid != "")
                {
                    var itemData = Db.UserItemCards.Where(x => x.UesrId == uid && x.ItemId ==iid && x.isDeleted != "true").Count();
                    if (itemData==0)
                    {
                        UserItemCard card = new UserItemCard();
                        card.ItemId = iid;
                        card.UesrId = uid;
                        card.isDeleted = "false";
                        Db.UserItemCards.Add(card);
                        Db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {

                return false;
            }


            return false;
        }
        public ActionResult SearchItem(string id)
        {
            using (var db = new sabiShopEntities())
            {
                var data= db.items.Where(t => t.location.Contains(id) || t.name.Contains(id)).Select(p => p).ToList();
                return View(data);
            }
        }
        public ActionResult ItemBySubCategories(int Sid)
        {
            var subCategory = Db.SubCategories.Where(x => x.SubCategorieId == Sid).ToList();
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


            return PartialView("ItemBySubCategories" , itemList);
        }
        public ActionResult SearchItemBySubCategories(string key , int Sid)
        {
            var subCategory = Db.SubCategories.Where(x => x.Shopid == Sid).ToList();
            var itemList = new List<SubCategoryItems>();
            foreach (var sub in subCategory)
            {
                var sci = new SubCategoryItems();
                var it = Db.items.Where(x => x.SubCategorieId == sub.SubCategorieId && x.isDeleted != "true" && x.name.Contains(key)).ToList();
                sci.SubCategorieId = sub.SubCategorieId;
                sci.name = sub.name;
                sci.items = it;
                itemList.Add(sci);
            }


            return PartialView("SearchItemBySubCategories", itemList);
        }

    }
}