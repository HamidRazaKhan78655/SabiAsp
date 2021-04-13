using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
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
            AuthSecret = "369127745f2fcaf1abfda30eb08383bf9b6d254c",
            BasePath = "https://accounts.google.com/o/oauth2/auth"
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
        public ActionResult Create(Login login)
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

        private void addLoginDateiFirebase(Login login)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = login;

            PushResponse response = client.Push("Login/", data);

            data.UserName = response.Result.name;
            SetResponse setResponse = client.Set("Login/"+data.UserName,data);
        }
    }
}