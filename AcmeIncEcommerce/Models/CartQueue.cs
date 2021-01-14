using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcmeIncEcommerce.Models
{
    public class CartQueue
    {
        public int CartQueueID { get; set; }
        public string CartID { get; set; }

        public int ProductID { get; set; }
        [Range(0, 50, ErrorMessage = "Please enter a quantity between 0 and 50")]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }
    }
}