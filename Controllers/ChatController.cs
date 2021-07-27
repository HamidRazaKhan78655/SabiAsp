using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class ChatController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();
        // GET: Chat
        public ActionResult Chat(string userId)
        {
            int uId = Convert.ToInt32(userId);
            ViewBag.userId = uId;
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            ViewBag.logedinUserId = logedinUserId;
            ViewBag.logedinUserName =  Session["Name"].ToString();
            var c = Db.GetUserGroup(logedinUserId, uId).SingleOrDefault();
            if (c == null || c == 0)
            {
                List<GetUserChat_Result> uc = new List<GetUserChat_Result>();
                return PartialView("Chat", uc);
            }
            var um = Db.GetUserChat(c, logedinUserId, uId).ToList();
            return PartialView("Chat", um);
        }
        public string PostChat(int logedinUserId, int vendorId, string message)
        {
            Group grp = new Group();
            UserGroup usrGrp = new UserGroup();
            UserMessage usrMsg = new UserMessage();
            UserMessageRecipient usrMsgRecp = new UserMessageRecipient();
            var g = Db.GetUserWithGroup(vendorId).SingleOrDefault();
            if (g == null)
            {
                grp.GroupName = Session["Name"].ToString() + " With " + Session["VendorName"].ToString();
                grp.IsDeleted = false;
                grp.CreatedBy = logedinUserId;
                grp.CreatedDate = DateTime.Now;
                Db.Groups.Add(grp);
                Db.SaveChanges();
            }

            usrGrp.GroupId = g == null ? grp.GroupId : g.GroupId;
            usrGrp.UserId = logedinUserId;
            usrGrp.IsDeleted = false;
            usrGrp.CreatedBy = logedinUserId;
            usrGrp.CreatedDate = DateTime.Now;
            Db.UserGroups.Add(usrGrp);
            Db.SaveChanges();

            usrMsg.Message = message;
            usrMsg.UserId = logedinUserId;
            usrMsg.IsDeleted = false;
            usrMsg.CreatedBy = logedinUserId;
            usrMsg.CreatedDate = DateTime.Now;
            Db.UserMessages.Add(usrMsg);
            Db.SaveChanges();

            usrMsgRecp.UserGroupId = usrGrp.UserGroupId;
            usrMsgRecp.MessageId = usrMsg.MessageId;
            usrMsgRecp.UserId = vendorId;
            usrMsgRecp.IsRead = false;
            usrMsgRecp.IsDeleted = false;
            usrMsgRecp.CreatedBy = logedinUserId;
            usrMsgRecp.CreatedDate = DateTime.Now;
            Db.UserMessageRecipients.Add(usrMsgRecp);
            Db.SaveChanges();
            return "success";
        }
        public ActionResult GetUserWithGroup(int vendorId) 
        {
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            var userList = Db.GetUserWithGroup(logedinUserId).ToList();
            return PartialView("GetUserWithGroup", userList);
        }

        public ActionResult VendorChat(string userId)
        {
            int uId = Convert.ToInt32(userId);
            ViewBag.userId = uId;
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            ViewBag.logedinUserId = logedinUserId;
            ViewBag.logedinUserName = Session["Name"].ToString();
            var c = Db.GetUserGroup(uId, logedinUserId).SingleOrDefault();
            ViewBag.GroupId = c;
            if (c == null || c == 0)
            {
                List<GetUserChat_Result> uc = new List<GetUserChat_Result>();
                return PartialView("VendorChat", uc);
            }
            var um = Db.GetUserChat(c, uId, logedinUserId).ToList();
            return PartialView("VendorChat", um);
        }
        public string PostVendorChat(int logedinUserId, int userId, int groupId, string message)
        {
            UserGroup usrGrp = new UserGroup();
            UserMessage usrMsg = new UserMessage();
            UserMessageRecipient usrMsgRecp = new UserMessageRecipient();

            usrGrp.GroupId = groupId;
            usrGrp.UserId = logedinUserId;
            usrGrp.IsDeleted = false;
            usrGrp.CreatedBy = logedinUserId;
            usrGrp.CreatedDate = DateTime.Now;
            Db.UserGroups.Add(usrGrp);
            Db.SaveChanges();

            usrMsg.Message = message;
            usrMsg.UserId = logedinUserId;
            usrMsg.IsDeleted = false;
            usrMsg.CreatedBy = logedinUserId;
            usrMsg.CreatedDate = DateTime.Now;
            Db.UserMessages.Add(usrMsg);
            Db.SaveChanges();

            usrMsgRecp.UserGroupId = usrGrp.UserGroupId;
            usrMsgRecp.MessageId = usrMsg.MessageId;
            usrMsgRecp.UserId = userId;
            usrMsgRecp.IsRead = false;
            usrMsgRecp.IsDeleted = false;
            usrMsgRecp.CreatedBy = logedinUserId;
            usrMsgRecp.CreatedDate = DateTime.Now;
            Db.UserMessageRecipients.Add(usrMsgRecp);
            Db.SaveChanges();
            return "success";
        }

    }
}