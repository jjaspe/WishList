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
            {
                ViewBag.User = user;
                return View(db.People.ToList().Where(c => c.Id != user.Id));
            }
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

        public ActionResult ManageWishList(int watchedUserId = 0)
        {
            String username = db.getLogInUserName();
            User loggedUser = db.People.SingleOrDefault(c => c.userName.Equals(username)),
                watchedUser = db.People.SingleOrDefault(c => c.Id == watchedUserId);
            ViewBag.LoggedUser = loggedUser;
            ViewBag.WatchedUser = watchedUser;
            if (watchedUser != null)
            {
                //if (user.WishListItems == null)
                //    user.WishListItems = new List<WishListItem>();
                return View(watchedUser.WishListItems);
            }

            return null;
        }

        [HttpPost]
        public ActionResult ManageWishList(string[] reservedItems, string[] removedItems, string action, int? watchedUserId)
        {
            if (action.Equals("Reserve Selected"))
                return Reserve(reservedItems, watchedUserId);
            else if (action.Equals("Remove Selected"))
                return Remove(removedItems);
            else
                return RedirectToAction("Index");
        }

        /// <summary>
        /// Reserves to loggedUserId items requested by watchedUserId.
        /// </summary>
        /// <param name="reservedItems">list of ids of reserved items</param>
        /// <param name="watchedUserId"></param>
        /// <returns></returns>
        public ActionResult Reserve(string[] reservedItems, int? watchedUserId)
        {
            WishListItem current;
            int itemId;
            String username = db.getLogInUserName();
            User loggedUser = db.People.SingleOrDefault(c => c.userName==username),
                watchedUser = db.People.SingleOrDefault(c => c.Id == watchedUserId);


            if (reservedItems != null && loggedUser != null && watchedUser != null && watchedUser.Id != loggedUser.Id)
            {
                foreach (string idString in reservedItems)
                {
                    itemId = Int32.Parse(idString);
                    current = db.Gifts.SingleOrDefault(c => c.Id == itemId);
                    if (current != null)
                    {
                        current.giver = loggedUser;
                        db.Entry(current).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes items from loggedUser's wish list
        /// </summary>
        /// <param name="removedItems">array of ids of items to remove</param>
        /// <param name="watchedUserId"></param>
        /// <returns></returns>
        public ActionResult Remove(string[] removedItems)
        {
            WishListItem current;
            int id;
            if (removedItems != null)
            {
                foreach (string idString in removedItems)
                {
                    id = Int32.Parse(idString);
                    current = db.Gifts.SingleOrDefault(c => c.Id == id);
                    if (current != null)
                        db.Gifts.Remove(current);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}