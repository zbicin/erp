﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ERP.Models;

namespace ERP.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Items/
        public ActionResult Index()
        {
            List<Item> items = db.Items.ToList();
            ViewBag.AlertMessages = new List<string>();
            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].QuantityInStock < 5)
                {
                    ViewBag.AlertMessages.Add(items[i].Name + " - low quantity in stock");
                }
            }            

            return View(items);
        }

        //
        // GET: /Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Items/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Items/Create
        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        //
        // GET: /Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Items/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,QuantityInStock")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //
        // GET: /Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Items/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
