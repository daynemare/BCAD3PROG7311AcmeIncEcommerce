using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcmeIncEcommerce.Models;


namespace AcmeIncEcommerce.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = ShoppingCart.GetCart();
            ShoppingCartViewModel viewModel = new ShoppingCartViewModel
            {
                CartQueues = cart.GetCartQueues(),
                SumTotalCost = cart.GetTotalCost()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productID, int quantity)
        {
            ShoppingCart cart = ShoppingCart.GetCart();
            cart.AddToCart(productID, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(ShoppingCartViewModel viewModel)
        {
            ShoppingCart cart = ShoppingCart.GetCart();
            cart.UpdateCart(viewModel.CartQueues);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveQueue(int id)
        {
            ShoppingCart cart = ShoppingCart.GetCart();
            cart.RemoveQueue(id);
            return RedirectToAction("Index");
        }

        public PartialViewResult Summary()
        {
            ShoppingCart cart = ShoppingCart.GetCart();
            ShoppingCartSummaryViewModel vm = new ShoppingCartSummaryViewModel
            {
                NumberOfItems = cart.GetNumberOfItems(),
                TotalCost = cart.GetTotalCost()

            };

                return PartialView(vm);
        }
    }
}