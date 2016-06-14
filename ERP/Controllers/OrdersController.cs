using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP.Models;
using System.Data.Entity;

namespace ERP.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Order/
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        //
        // GET: /Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Order/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Order/Create
        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        //
        // GET: /Order/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = db.Orders.SingleOrDefault(o => o.Id == id);
            return View(order);
        }

        //
        // POST: /Order/Edit/5
        [HttpPost]
        public ActionResult Edit(Order order)
        {
            try
            {
                // TODO: Add update logic here
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
