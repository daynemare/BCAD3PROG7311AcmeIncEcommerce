using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcmeIncEcommerce.DataAccessLayer;

namespace AcmeIncEcommerce.Models
{
    public class ShoppingCart
    {
        private string ShoppingCartID { get; set; }
        private const string ShoppingCartSessionKey = "ShoppingCartID";
        private DatabaseContext db = new DatabaseContext();

        private string GetCartID()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
            {
                HttpContext.Current.Session[ShoppingCartSessionKey]
                = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                Guid tempCartID = Guid.NewGuid();
                HttpContext.Current.Session[ShoppingCartSessionKey] = tempCartID.ToString();
            }
            return HttpContext.Current.Session[ShoppingCartSessionKey].ToString();
        }

        public static ShoppingCart GetCart()
        {
            ShoppingCart cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartID();
            return cart;
        }

        public void AddToCart(int productID,int quantity)
        {
            var cartqueue = db.CartQueues.FirstOrDefault(b => b.CartID == ShoppingCartID &&
            b.ProductID == productID);

            if (cartqueue == null)
            {
                cartqueue = new CartQueue
                {
                    ProductID = productID,
                    CartID = ShoppingCartID,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                db.CartQueues.Add(cartqueue);
            }
            else
            {
                cartqueue.Quantity += quantity;
            }
            db.SaveChanges();
        }

        public void RemoveQueue(int productID)
        {
            var cartqueue = db.CartQueues.FirstOrDefault(b => b.CartID == ShoppingCartID &&
            b.ProductID == productID);
            if(cartqueue!=null)
            {
                db.CartQueues.Remove(cartqueue);
            }
            db.SaveChanges();
        }
       
        public void UpdateCart(List<CartQueue> queues)
        {
            foreach(var queue in queues)
            {
                var cartqueue = db.CartQueues.FirstOrDefault(b => b.CartID == ShoppingCartID &&
                b.ProductID == queue.ProductID);
                if (cartqueue != null)
                {
                    if (queue.Quantity == 0)
                    {
                        RemoveQueue(queue.ProductID);
                    }
                    else
                    {
                        cartqueue.Quantity = queue.Quantity;
                    }
                }
            }

            db.SaveChanges();
        }

        public void EmptyCart()
        {
            var cartqueues = db.CartQueues.Where(b => b.CartID == ShoppingCartID);
            foreach(var cartqueue in cartqueues)
            {
                db.CartQueues.Remove(cartqueue);
            }
            db.SaveChanges();
        }

        public List<CartQueue> GetCartQueues()
        {
            return db.CartQueues.Where(b => b.CartID == ShoppingCartID).ToList();
        }

        public decimal GetTotalCost()
        {
            decimal cartTotal = decimal.Zero;
            
            if(GetCartQueues().Count>0)
            {
                cartTotal = db.CartQueues.Where(b => b.CartID == ShoppingCartID).Sum(b => b.Product.ProductPrice * b.Quantity);

            }
            return cartTotal;
        }

        public int GetNumberOfItems()
        {
            int numberOfItems = 0;

            if(GetCartQueues().Count>0)
            {
                numberOfItems = db.CartQueues.Where(b => b.CartID == ShoppingCartID)
                .Sum(b => b.Quantity);
            }

            return numberOfItems;
        }

        public void MigrateShoppingCart(string username)
        {
            var cart = db.CartQueues.Where(b => b.CartID == ShoppingCartID).ToList();

            var usersCart = db.CartQueues.Where(b => b.CartID == username).ToList();

            if(usersCart != null)
            {
                string lastID = username;
                ShoppingCartID = username;

                foreach(var queue in cart)
                {
                    AddToCart(queue.ProductID, queue.Quantity);
                }

                ShoppingCartID = lastID;
                EmptyCart();
            }
            else
            {
                foreach(var cartqueue in cart)
                {
                    cartqueue.CartID = username;
                }
                db.SaveChanges();
            }
            HttpContext.Current.Session[ShoppingCartSessionKey] = username;
        
        }

        public decimal CreateOrderRows(int orderID)
        {
            decimal orderTotal = 0;

            var cartqueues = GetCartQueues();

            foreach (var item in cartqueues)
            {
                OrderRow orderRow = new OrderRow
                {
                    Product = item.Product,
                    ProductID = item.ProductID,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.ProductPrice,
                    OrderID = orderID
                };

                orderTotal += (item.Quantity * item.Product.ProductPrice);
                db.OrderRows.Add(orderRow);
            }

            db.SaveChanges();
            EmptyCart();
            return orderTotal;
        }

    }
}