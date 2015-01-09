using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WishList.Models;

namespace WishList.Controllers
{
    public class UserController : Controller
    {
        private WishListDBContext db = new WishListDBContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            String username = db.getLogInUserName();
            User user = db.People.SingleOrDefault(c => c.userName.Equals(username));
            if (user != null)
                return View(db.People.ToList().Where(c => c.Id != user.Id));
            else
                return RedirectToAction("LoggedOut");
        }

        public ActionResult LoggedOut()
        {
            return View();
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.People.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }


        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.People.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}