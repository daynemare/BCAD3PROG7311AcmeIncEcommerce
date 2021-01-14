﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcmeIncEcommerce.Models
{
    public class Order
    {
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [Display(Name = "User")]
        public string UserID { get; set; }
        [Display(Name = "Deliver to")]
        public string DeliveryName { get; set; }
        [Display(Name = "Delivery Address")]
        public Address DeliveryAddress { get; set; }
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Time of Order")]
        public DateTime DateCreated { get; set; }
        public List<OrderRow> OrderRows { get; set; }
    }
}