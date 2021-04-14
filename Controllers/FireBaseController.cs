  using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using SabiAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SabiAsp.Controllers
{
    public class FireBaseController : Controller
    {
        // GET: FireBase

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "QkEBWuXalB1NSe1MBeZwJ59um3hNkXZqOFJUJ6gi",
            BasePath = "https://sabi-27ae5-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client; 
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FireBaseLogin login)
        {
            
            try
            {
                addLoginDateiFirebase(login);
                ModelState.AddModelError(string.Empty,"Data Save Successfully");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Data Not Save Successfully"+ex.ToString());
            }
            return View();
        }

        private void addLoginDateiFirebase(FireBaseLogin login)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = login;
           SetResponse setResponse = client.Set(@"Login/" + data.UserID, data);  //Insert Query and set user id
           // PushResponse response = client.Push(@"Login/"+data.UserID, data);  //push Auto id 
          //  data.Email = response.Result.name;

            var res=setResponse.StatusCode; //if StutasCode return Ok means data Save Succsessfully

         //  var getcall = client.Get(@"Login/" + data.UserID);   //Select Query With Where

            //client.Update(@"Login/" + data.UserID, data);  //Update Query
            //client.Delete(@"Login/" + data.UserID);  //Delete Query
        }
    }
}