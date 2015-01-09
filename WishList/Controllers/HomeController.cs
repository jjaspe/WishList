using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WishList.Models;

namespace WishList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.returnURL = "Index";
            WishListDBContext.SynchronizeDatabases();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ActionName("Profile")]
        public ActionResult UserProfile()
        {
            WishListDBContext db = new WishListDBContext();
            String username = db.getLogInUserName();
            if(!String.IsNullOrEmpty(username))
            {
                User user = db.People.SingleOrDefault(c => c.userName.Equals(username));
                if (user != null)
                    return RedirectToAction("Edit", "User", new { id = user.Id });
                else
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

            
        }
    }
}
