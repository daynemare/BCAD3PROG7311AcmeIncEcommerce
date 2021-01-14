using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcmeIncEcommerce.Models
{
    public class ShoppingCartViewModel
    {
        public List<CartQueue> CartQueues { get; set; }
        [Display(Name="Shopping Cart Total:")]

        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal SumTotalCost { get; set; }
    }
}