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

        User getLoggedUser()
        {
            String name = db.getLogInUserName();
            return db.People.SingleOrDefault(c => c.userName.Equals(name));
        }

       

        //
        // GET: /User/

        public ActionResult Index()
        {
            User user = getLoggedUser();
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

        public ActionResult ManageWishList(int watchedUserId = 0,bool showReservedBy=false)
        {
            User watchedUser = db.People.SingleOrDefault(c => c.Id == watchedUserId);
            ViewBag.LoggedUser = getLoggedUser();
            ViewBag.WatchedUser = watchedUser;
            if (watchedUser != null && ViewBag.LoggedUser!=null)
            {
                if (showReservedBy)
                    ViewBag.ItemsReservedByUser = db.getItemsReservedBy(ViewBag.LoggedUser);
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
            
            User loggedUser = getLoggedUser(),
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

        [HttpPost]
        public ActionResult AddProductsToList(string[] productsToAdd)
        {
            Product product;
            User user = getLoggedUser();
            if (productsToAdd != null && user!=null)
            {
                foreach (string i in productsToAdd)
                {
                    product = db.Products.First(n => n.Name.Equals(i));
                    WishListItem t = new WishListItem() { receiver = user, product = product, giver = null, giverID = null };
                    //user.wishListItems.Add(t);
                    db.Gifts.Add(t);
                    db.SaveChanges();
                    RedirectToAction("Index");
                }
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