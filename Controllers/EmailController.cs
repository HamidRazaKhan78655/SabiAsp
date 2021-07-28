using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class EmailController : Controller
    {
        sabiShopEntities db = new sabiShopEntities();
        // GET: Email
        public string sendEmailTOVendors()
        {
            int userid = int.Parse(Session["UserId"].ToString());
            var getAllCarts = db.UserItemCards.Where(carts => carts.UesrId == userid).ToList();

            
            foreach (var cart in getAllCarts) {
                var item = db.items.Where(i => i.ItemId==cart.ItemId).FirstOrDefault();
                var subCategory = db.SubCategories.Where(s => s.SubCategorieId == item.SubCategorieId).FirstOrDefault();
                var getShop = db.Shops.Where(s => s.Shopid == subCategory.Shopid).FirstOrDefault();
                var getVendor = db.vendors.Where(s => s.vendorid == getShop.vendorid).FirstOrDefault();
                Tracking tracking = new Tracking();
                tracking.ItemId = item.ItemId;
                tracking.to = userid;
                tracking.from = getVendor.vendorid;
                tracking.step1 = "incomplete";
                tracking.step2 = "incomplete";
                tracking.step3 = "incomplete";
                tracking.state = "inprogress";
                db.Trackings.Add(tracking);
                db.SaveChanges();


                /* SmtpClient smtpClient = new SmtpClient("mail.MyWebsiteDomainName.com", 25);

                 smtpClient.Credentials = new System.Net.NetworkCredential("info@MyWebsiteDomainName.com", "myIDPassword");
                 // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                 smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                 smtpClient.EnableSsl = true;
                 MailMessage mail = new MailMessage();

                 //Setting From , To and CC
                 mail.From = new MailAddress("hamidraza78655@gmail.com", "Click Sabi");
                 mail.To.Add(new MailAddress(getVendor.user.EmailAddress.ToString()));
                 //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                 smtpClient.Send(mail);*/
            }
            var trackings = db.Trackings.Where(t => t.to == userid).ToList();
            string trackingIDs = "";
            foreach (var track in trackings) {
                trackingIDs = track.trackingId + ","+trackingIDs;
            }
            foreach (var del in getAllCarts) {
                db.UserItemCards.Remove(del);
            }
            db.SaveChanges();
            Session["CartCount"] = 0;
            return trackingIDs;
        }
    }
}