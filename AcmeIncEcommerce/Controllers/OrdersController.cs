using AcmeIncEcommerce.DataAccessLayer;
using AcmeIncEcommerce.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AcmeIncEcommerce.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {

            private DatabaseContext db = new DatabaseContext();

            private ApplicationUserManager _userManager;

            public ApplicationUserManager UserManager
            {
                get
                {
                    return _userManager ??
                 HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                private set
                {
                    _userManager = value;
                }
            }

            // GET: Orders
            public ActionResult Index(string orderSearch, string startDate, string endDate, string orderSortOrder)
            {
                var orders = db.Orders.OrderBy(o => o.DateCreated).Include(o => o.OrderRows);

                if (!User.IsInRole("Administrator"))
                {
                    orders = orders.Where(o => o.UserID == User.Identity.Name);
                }

                if (!String.IsNullOrEmpty(orderSearch))
                {
                    orders = orders.Where(o => o.OrderID.ToString().Equals(orderSearch) ||
                     o.UserID.Contains(orderSearch) ||
                     o.DeliveryName.Contains(orderSearch) ||
                     o.DeliveryAddress.AddressLine1.Contains(orderSearch) ||
                     o.DeliveryAddress.AddressLine2.Contains(orderSearch) ||
                     o.DeliveryAddress.Town.Contains(orderSearch) ||
                     o.DeliveryAddress.County.Contains(orderSearch) ||
                     o.DeliveryAddress.Postcode.Contains(orderSearch) ||
                     o.TotalPrice.ToString().Equals(orderSearch) ||
                     o.OrderRows.Any(ol => ol.ProductName.Contains(orderSearch)));
                }

                DateTime parsedStartDate;
                if (DateTime.TryParse(startDate, out parsedStartDate))
                {
                    orders = orders.Where(o => o.DateCreated >= parsedStartDate);
                }

                DateTime parsedEndDate;
                if (DateTime.TryParse(endDate, out parsedEndDate))
                {
                    orders = orders.Where(o => o.DateCreated <= parsedEndDate);
                }

                ViewBag.DateSort = String.IsNullOrEmpty(orderSortOrder) ? "date" : "";
                ViewBag.UserSort = orderSortOrder == "user" ? "user_desc" : "user";
                ViewBag.PriceSort = orderSortOrder == "price" ? "price_desc" : "price";
                ViewBag.CurrentOrderSearch = orderSearch;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;

                switch (orderSortOrder)
                {
                    case "user":
                        orders = orders.OrderBy(o => o.UserID);
                        break;
                    case "user_desc":
                        orders = orders.OrderByDescending(o => o.UserID);
                        break;
                    case "price":
                        orders = orders.OrderBy(o => o.TotalPrice);
                        break;
                    case "price_desc":
                        orders = orders.OrderByDescending(o => o.TotalPrice);
                        break;
                    case "date":
                        orders = orders.OrderBy(o => o.DateCreated);
                        break;
                    default:
                        orders = orders.OrderByDescending(o => o.DateCreated);
                        break;
                }

                return View(orders);
            }


            // GET: Orders/Details/5
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Include(o => o.OrderRows).Where(o => o.OrderID == id).SingleOrDefault();

                if (order == null)
                {
                    return HttpNotFound();
                }

                if (order.UserID == User.Identity.Name || User.IsInRole("Administrator"))
                {
                    return View(order);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }

            // GET: Orders/Review
            public async Task<ActionResult> Review()
            {
                ShoppingCart cart = ShoppingCart.GetCart();
                Order order = new Order();

                order.UserID = User.Identity.Name;
                ApplicationUser user = await UserManager.FindByNameAsync(order.UserID);
                order.DeliveryName = user.FirstName + " " + user.LastName;
                order.DeliveryAddress = user.Address;
                order.OrderRows = new List<OrderRow>();
                foreach (var cartqueue in cart.GetCartQueues())
                {
                    OrderRow row = new OrderRow
                    {
                        Product = cartqueue.Product,
                        ProductID = cartqueue.ProductID,
                        ProductName = cartqueue.Product.ProductName,
                        Quantity = cartqueue.Quantity,
                        UnitPrice = cartqueue.Product.ProductPrice
                    };
                    order.OrderRows.Add(row);
                }
                order.TotalPrice = cart.GetTotalCost();
                return View(order);
            }


            // POST: Orders/Create
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "UserID,DeliveryName,DeliveryAddress")] Order order)
            {
                if (ModelState.IsValid)
                {
                    order.DateCreated = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();

                  
                    ShoppingCart cart = ShoppingCart.GetCart();
                    order.TotalPrice = cart.CreateOrderRows(order.OrderID);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = order.OrderID });
                }
                return RedirectToAction("Review");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
