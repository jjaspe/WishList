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
    public class LinkController : Controller
    {
        private WishListDBContext db = new WishListDBContext();

        //
        // GET: /Link/

        public ActionResult Index()
        {
            return View(db.ProductLinks.ToList());
        }

        //
        // GET: /Link/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductLink productlink = db.ProductLinks.Find(id);
            if (productlink == null)
            {
                return HttpNotFound();
            }
            return View(productlink);
        }

        //
        // GET: /Link/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Link/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductLink productlink)
        {
            if (ModelState.IsValid)
            {
                db.ProductLinks.Add(productlink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productlink);
        }

        //
        // GET: /Link/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductLink productlink = db.ProductLinks.Find(id);
            if (productlink == null)
            {
                return HttpNotFound();
            }
            return View(productlink);
        }

        //
        // POST: /Link/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductLink productlink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productlink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productlink);
        }

        //
        // GET: /Link/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProductLink productlink = db.ProductLinks.Find(id);
            if (productlink == null)
            {
                return HttpNotFound();
            }
            return View(productlink);
        }

        //
        // POST: /Link/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductLink productlink = db.ProductLinks.Find(id);
            db.ProductLinks.Remove(productlink);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}