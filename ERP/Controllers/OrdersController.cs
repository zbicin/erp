using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using ERP.ViewModels;
using Newtonsoft.Json;

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
            List<Item> availaItemstems = db.Items.ToList();
            OrderViewModel orderViewModel = new OrderViewModel();

            IEnumerable<SelectListItem> selectList = db.Items.Select(s => new SelectListItem{
                    Text = s.Name,
                    Value = s.Id.ToString()
                });
            orderViewModel.Id = id;
            orderViewModel.AvailableItems = selectList;
            orderViewModel.CanceledAt = order.CanceledAt;
            orderViewModel.CompletedAt = order.CompletedAt;
            orderViewModel.CreatedAt = order.CreatedAt;
            orderViewModel.DeliveredAt = order.DeliveredAt;
            orderViewModel.ShippedAt = order.ShippedAt;

            List<OrderElement> elements = db.OrderElements.Where(el => el.OrderId == id).ToList();
            orderViewModel.SelectedItems = elements;
            return View(orderViewModel);
        }

        //
        // POST: /Order/Edit/5
        [HttpPost]
        public ActionResult Edit(OrderViewModel viewModel)
        {
            Order order = new Order
            {
                Id = viewModel.Id,
                CanceledAt = viewModel.CanceledAt,
                CompletedAt = viewModel.CompletedAt,
                CreatedAt = viewModel.CreatedAt,
                DeliveredAt = viewModel.DeliveredAt,
                ShippedAt = viewModel.ShippedAt
            };
            
            if (viewModel.SelectedItems != null)
            {
                foreach (OrderElement element in viewModel.SelectedItems)
                {
                    OrderElement orderElement = db.OrderElements.SingleOrDefault(o => o.Id == element.Id);
                    if (orderElement != null)
                    {
                        orderElement.Quantity = element.Quantity;
                        db.Entry(orderElement).State = EntityState.Modified;
                    }
                    else
                    {
                        orderElement = new OrderElement
                        {
                            ItemName = element.ItemName,
                            Quantity = element.Quantity,
                            OrderId = viewModel.Id
                        };
                        db.OrderElements.Add(orderElement);
                    }
                }
            }

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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

        public async Task<string> GetAvailableItems()
        {
            List<Item> items = db.Items.ToList();

            return JsonConvert.SerializeObject(items);
        }
    }
}
