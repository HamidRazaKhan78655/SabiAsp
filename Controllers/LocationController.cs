using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabiAsp.Controllers
{
    public class LocationController : Controller
    {
        sabiShopEntities Db = new sabiShopEntities();

        #region CRUD Location
        // GET: Location
        public ActionResult GetAllLocation()
        {
            var loc = Db.Locations.Where(l => l.isDeleted == "false").ToList();
            return PartialView("", loc);
        }
        public string AddLocation(FormCollection fm)
        {
            string name = fm["LocationName"].ToString();
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            Location loc = new Location();
            var itemData = Db.Locations.Where(x => x.LocationName.ToLower() == name.ToLower() && x.isDeleted == "false").FirstOrDefault();
            if (itemData == null)
            {
                loc.LocationName = name;
                loc.isDeleted = "false";
                loc.CreatedDate = DateTime.Now;
                loc.CreatedBy = logedinUserId;
                Db.Locations.Add(loc);
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string UpdateLocation(FormCollection fm)
        {
            string name = fm["updateLocationName"].ToString();
            string updateLocationId = fm["updateLocationId"].ToString();
            int locId = int.Parse(updateLocationId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);

            var loc = Db.Locations.Where(x => x.LocationId == locId && x.isDeleted == "false").SingleOrDefault();
            if (loc != null)
            {
                loc.LocationName = name;
                loc.isDeleted = "false";
                loc.ModifiedBy = logedinUserId;
                loc.ModifiedDate = DateTime.Now;
                Db.Entry(loc).State = EntityState.Modified;
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public string DeleteLocation(string locationId)
        {
            int locId = int.Parse(locationId);
            int logedinUserId = Convert.ToInt32(Session["UserId"]);
            var loc = Db.Locations.Where(x => x.LocationId == locId && x.isDeleted == "false").SingleOrDefault();
            if (loc != null)
            {
                loc.ModifiedBy = logedinUserId;
                loc.ModifiedDate = DateTime.Now;
                loc.isDeleted = "true";
                Db.Locations.Add(loc);
                Db.SaveChanges();
                return "success";
            }
            return "error";
        }
        #endregion
    }
}