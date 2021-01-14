using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcmeIncEcommerce.Models;
using System.Data.Entity;
using AcmeIncEcommerce.Migrations;

namespace AcmeIncEcommerce.DataAccessLayer
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("name=DefaultConnection") {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductImageMapping> ProductImageMappings { get; set; }
        public DbSet<Category>Categories { get; set; }

        public DbSet<CartQueue>CartQueues { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }


    }
}