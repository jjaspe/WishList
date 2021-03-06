﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WishList.Models;

namespace WishList.Controllers
{
    public class LinksController : Controller
    {
        private WishListDBContext db = new WishListDBContext();

        ProductLink fixURL(ProductLink productlink)
        {
            //Prepend http://www if not there
            if (!productlink.URL.StartsWith("https://") && !productlink.URL.StartsWith("http://"))
            {
                if (!productlink.URL.StartsWith("www."))
                    productlink.URL = "www." + productlink.URL;

                productlink.URL = "http://" + productlink.URL;
            }

            return productlink;
        }

        ProductLink fixPrice(ProductLink productlink)
        {
            productlink.Price = productlink.Price ?? 0.0;//Make price 0 if null
            return productlink;
        }

        //
        // GET: /Links/

        public ActionResult Index()
        {
            Product product = db.getKeptProduct();
            ViewBag.product = product;
            return View(product.Links.ToList());
        }

        //
        // GET: /Links/Details/5

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
        // GET: /Links/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Links/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductLink productlink)
        {
            Product product = db.getKeptProduct();
            if (product == null)
                return View(productlink);

            if (ModelState.IsValid)
            {
                product.Links.Add(this.fixPrice(this.fixURL(productlink)));

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productlink);
        }

        //
        // GET: /Links/Edit/5

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
        // POST: /Links/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductLink productlink)
        {
            if (ModelState.IsValid)
            {
                this.fixPrice(this.fixURL(productlink));
                db.Entry(productlink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productlink);
        }

        //
        // GET: /Links/Delete/5

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
        // POST: /Links/Delete/5

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