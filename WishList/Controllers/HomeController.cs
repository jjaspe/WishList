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
        WishListDBContext db = new WishListDBContext();

        public void InitializeUsers()
        {
            UsersContext userContext = new UsersContext();
            //For all profiles in userContexts that dont have their usernames already in context,
            //add them to context with name = userName. User can edit it later
            foreach (UserProfile profile in userContext.UserProfiles)
            {
                User p = db.People.SingleOrDefault(n => n.userName.Equals(profile.UserName));

                if (p == null)
                    db.People.Add(new User() { Name = profile.UserName, userName = profile.UserName });
            }
            db.SaveChanges();
        }

        public ActionResult Index()
        {
            InitializeUsers();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.returnURL = "Index";
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
