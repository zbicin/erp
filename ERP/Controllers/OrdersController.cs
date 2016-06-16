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

            OrderViewModel orderViewModel = new OrderViewModel();           
            orderViewModel.CanceledAt = order.CanceledAt;
            orderViewModel.CompletedAt = order.CompletedAt;
            orderViewModel.CreatedAt = order.CreatedAt;
            orderViewModel.DeliveredAt = order.DeliveredAt;
            orderViewModel.ShippedAt = order.ShippedAt;
            orderViewModel.Status = order.Status.ToString();
            List<OrderElement> elements = db.OrderElements.Where(el => el.OrderId == id).ToList();
            orderViewModel.SelectedItems = elements;

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(orderViewModel);
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
        public string Create(OrderViewModel viewModel)
        {
            DateTime createdAt = DateTime.Now;

            if (viewModel.SelectedItems != null){
                List<OrderElement> distinctOrderElements =
                    viewModel.SelectedItems.GroupBy(s => s.ItemName).Select(s => new OrderElement()
                    {
                        ItemName = s.First().ItemName,
                        Quantity = s.Sum(q => q.Quantity)

                    }).ToList();


                foreach (var element in distinctOrderElements)
                {
                    Item item = db.Items.SingleOrDefault(e => e.Name == element.ItemName);
                    if (element.Quantity > item.QuantityInStock)
                    {
                        ViewBag.ErrorMessage = "Number of items in order exceeds warehouse stock";
                        ModelState.AddModelError("", "Number of items in order exceeds warehouse stock");
                        return "Number of items in order exceeds warehouse stock";
                    }
                }
            }
        
            Order order = new Order
            {
                CreatedAt = createdAt
            };

            if (viewModel.SelectedItems != null)
            {
                foreach (OrderElement element in viewModel.SelectedItems)
                {
                    OrderElement orderElement = new OrderElement
                    {
                        ItemName = element.ItemName,
                        Quantity = element.Quantity,
                        OrderId = viewModel.Id
                    };
                    db.OrderElements.Add(orderElement);

                    Item affectedItemInWarehouse = db.Items.SingleOrDefault(i => i.Name == orderElement.ItemName);
                    if (affectedItemInWarehouse != null)
                    {
                        affectedItemInWarehouse.QuantityInStock -= orderElement.Quantity;
                        db.Entry(affectedItemInWarehouse).State = EntityState.Modified;
                    }
                }                    
            }

            db.Orders.Add(order);
            db.SaveChanges();
            return "OK";            
        }

        //
        // GET: /Order/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = db.Orders.SingleOrDefault(o => o.Id == id);            
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
            orderViewModel.Status = order.Status.ToString();
            List<OrderElement> elements = db.OrderElements.Where(el => el.OrderId == id).ToList();
            orderViewModel.SelectedItems = elements;
            return View(orderViewModel);
        }

        //
        // POST: /Order/Edit/5
        [HttpPost]
        public string Edit(OrderViewModel viewModel)
        {
            DateTime now = DateTime.Now;

            if (viewModel.SelectedItems != null)
            {
                List<OrderElement> distinctOrderElements =
                    viewModel.SelectedItems.GroupBy(s => s.ItemName).Select(s => new OrderElement()
                    {
                        ItemName = s.First().ItemName,
                        Quantity = s.Sum(q => q.Quantity)

                    }).ToList();


                foreach (var element in distinctOrderElements)
                {
                    Item item = db.Items.SingleOrDefault(e => e.Name == element.ItemName);
                    if (element.Quantity > item.QuantityInStock)
                    {
                        ViewBag.ErrorMessage = "Number of items in order exceeds warehouse stock";
                        ModelState.AddModelError("", "Number of items in order exceeds warehouse stock");
                        return "Number of items in order exceeds warehouse stock";
                    }
                }
            }

            Order order = new Order
            {
                Id = viewModel.Id,
                CanceledAt = viewModel.CanceledAt,
                CompletedAt = viewModel.CompletedAt,
                CreatedAt = viewModel.CreatedAt,
                DeliveredAt = viewModel.DeliveredAt,
                ShippedAt = viewModel.ShippedAt
            };

            switch (viewModel.Status)
            {
                case "Canceled":
                    order.CanceledAt = now;
                    break;
                case "Completed":
                    order.CompletedAt = now;
                    break;
                case "Delivered":
                    order.DeliveredAt = now;
                    break;
                case "Shipped":
                    order.ShippedAt = now;
                    break;
            }

            if (viewModel.SelectedItems != null)
            {
                foreach (OrderElement element in viewModel.SelectedItems)
                {
                    bool amountChanged = false;
                    int amountDifference = 0;

                    OrderElement orderElement = db.OrderElements.SingleOrDefault(o => o.Id == element.Id);
                    if (orderElement != null)
                    {
                        if (element.Quantity != orderElement.Quantity)
                        {
                            amountChanged = true;
                            amountDifference = element.Quantity - orderElement.Quantity;
                        }
                        orderElement.Quantity = element.Quantity;
                        db.Entry(orderElement).State = EntityState.Modified;
                    }
                    else
                    {
                        amountChanged = true;
                        amountDifference = element.Quantity;
                        orderElement = new OrderElement
                        {
                            ItemName = element.ItemName,
                            Quantity = element.Quantity,
                            OrderId = viewModel.Id
                        };
                        db.OrderElements.Add(orderElement);
                    }

                    Item affectedItemInWarehouse = db.Items.SingleOrDefault(i => i.Name == orderElement.ItemName);
                    if (affectedItemInWarehouse != null)
                    {
                        if (order.Status == Order.OrderStatus.Canceled)
                        {
                            affectedItemInWarehouse.QuantityInStock += orderElement.Quantity;
                            db.Entry(affectedItemInWarehouse).State = EntityState.Modified;
                        }
                        else
                        if (amountChanged == true)
                        {
                            affectedItemInWarehouse.QuantityInStock -= amountDifference;
                            db.Entry(affectedItemInWarehouse).State = EntityState.Modified;
                        }   
                    }
                }
            }

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return "OK";
        }

        //
        // GET: /Order/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.SingleOrDefault(o => o.Id == id);

            if (order != null)
            {
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Id = id;                
                orderViewModel.CanceledAt = order.CanceledAt;
                orderViewModel.CompletedAt = order.CompletedAt;
                orderViewModel.CreatedAt = order.CreatedAt;
                orderViewModel.DeliveredAt = order.DeliveredAt;
                orderViewModel.ShippedAt = order.ShippedAt;
                orderViewModel.Status = order.Status.ToString();
                List<OrderElement> elements = db.OrderElements.Where(el => el.OrderId == id).ToList();
                orderViewModel.SelectedItems = elements;

                return View(orderViewModel);
            }

            return View();
        }

        //
        // POST: /Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
                // TODO: Add delete logic here
                Order order = db.Orders.SingleOrDefault(o => o.Id == id);
                if(order != null)
                {
                    if (order.Status == Order.OrderStatus.Created || order.Status == Order.OrderStatus.Completed)
                    {
                        List<OrderElement> orderElements = db.OrderElements.Where(o => o.OrderId == id).ToList();
                        foreach (var orderElement in orderElements)
                        {
                            Item item = db.Items.SingleOrDefault(i => i.Name == orderElement.ItemName);
                            item.QuantityInStock += orderElement.Quantity;
                            db.Entry(item).State = EntityState.Modified;
                        }
                    }

                    db.Orders.Remove(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
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
