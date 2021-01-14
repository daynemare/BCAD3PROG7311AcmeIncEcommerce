namespace AcmeIncEcommerce.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AcmeIncEcommerce.DataAccessLayer.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AcmeIncEcommerce.DataAccessLayer.DatabaseContext context)
        {

            var categories = new List<Category>
            {
                new Category {CategoryName = "Studio Monitors"},
                new Category {CategoryName = "Headphones"},
                new Category {CategoryName = "Microphones"},
                new Category {CategoryName = "Audio Interfaces"},
                new Category {CategoryName = "Music Software"},
                new Category {CategoryName = "Midi Controllers"}
            };

            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.CategoryName, c));
            context.SaveChanges();

            var products = new List<Product>
            {

                new Product{ProductName="Monkey Banana Gibbon Air",ProductDescription="The gibbon air 2 system is an active near field monitor system with bluetooth technology it consists of an active and a passive loudspeaker",ProductPrice=3749.99M,CategoryID=categories.Single(c=>c.CategoryName=="Studio Monitors").CategoryID},
                new Product{ProductName="KRK Rokit RP5 Studio Monitors",ProductDescription="Krk systems is one of the worlds most respected manufacturers of studio reference monitor",ProductPrice=7500.99M,CategoryID=categories.Single(c=>c.CategoryName=="Studio Monitors").CategoryID},

                new Product{ProductName="AKG K72 Headphones",ProductDescription="Professional drivers clear sound in the studio and beyond our k72 headphones deliver authoritative, extended low frequency response that gives definition to kick drums and bass guitars",ProductPrice=1049.99M,CategoryID=categories.Single(c=>c.CategoryName=="Headphones").CategoryID},
                new Product{ProductName="Behringer BH470",ProductDescription="Compact studio monitoring headphones",ProductPrice=699.99M,CategoryID=categories.Single(c=>c.CategoryName=="Headphones").CategoryID},

                new Product{ProductName="M900 Recording Microphone",ProductDescription="Suitable for use with computers use a connection by 3 point 5 mm jack",ProductPrice=1200.99M,CategoryID=categories.Single(c=>c.CategoryName=="Microphones").CategoryID},
                new Product{ProductName="BM800 Condensor Microphone",ProductDescription="High quality condensor microphone",ProductPrice=800.99M,CategoryID=categories.Single(c=>c.CategoryName=="Microphones").CategoryID},

                new Product{ProductName="Behringer ADA8200",ProductDescription="Great audio interface for digital to analog conversion",ProductPrice=5500.99M,CategoryID=categories.Single(c=>c.CategoryName=="Audio Interfaces").CategoryID},
                new Product{ProductName="MAudio Air",ProductDescription="Great interface for recording proffesional level vocals and guitar",ProductPrice=2500.99M,CategoryID=categories.Single(c=>c.CategoryName=="Audio Interfaces").CategoryID},

                new Product{ProductName="FL Studio Producer Edition",ProductDescription="One of the best digital audio workshops for music production around",ProductPrice=3000.99M,CategoryID=categories.Single(c=>c.CategoryName=="Music Software").CategoryID},
                new Product{ProductName="Xfer Records Serum",ProductDescription="The dream synthesizer did not seem to exist a wavetable synthesizer with a truly high quality sound, visual and creative workflow oriented interface to make creating and altering sounds fun instead of tedious",ProductPrice=3000.99M,CategoryID=categories.Single(c=>c.CategoryName=="Music Software").CategoryID},

                new Product{ProductName="MAudio Oxygen 49",ProductDescription="Great midi keyboard controller for recording scores in your digital audio workshop",ProductPrice=2500.99M,CategoryID=categories.Single(c=>c.CategoryName=="Midi Controllers").CategoryID}
            };

            products.ForEach(c => context.Products.AddOrUpdate(p => p.ProductName, c));
            context.SaveChanges();

            var images = new List<ProductImage>
            {
                new ProductImage { FileName="headphonesakgk72.jpg" },
                new ProductImage { FileName="headphonesbehringer.jpg" },

                new ProductImage { FileName="interfacemaudio.jpg" },
                new ProductImage { FileName="interfacrBehringer.jpg" },

                 new ProductImage { FileName="microphonebm800.jpg" },
                 new ProductImage { FileName="microphonem900.jpg" },

                 new ProductImage { FileName="midimaudiooxygen49.jpg" },

                 new ProductImage { FileName="monitorgibbonair.jpg" },
                 new ProductImage { FileName="monitorkrk.jpg" },

                 new ProductImage { FileName="softwareflstudioproduceredition.jpg" },
                 new ProductImage { FileName="softwareserum.jpg" }

            };

            images.ForEach(c => context.ProductImages.AddOrUpdate(p => p.FileName, c));
            context.SaveChanges();


            var imageMappings = new List<ProductImageMapping>
            {
                new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "headphonesakgk72.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "AKG K72 Headphones").ProductID, ImageNumber = 0 },
                new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "headphonesbehringer.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "Behringer BH470").ProductID, ImageNumber = 0 },


                   new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "microphonebm800.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "BM800 Condensor Microphone").ProductID, ImageNumber = 0 },
                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "microphonem900.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "M900 Recording Microphone").ProductID, ImageNumber = 0 },

                          new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "interfacemaudio.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "MAudio Air").ProductID, ImageNumber = 0 },
                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "interfacrBehringer.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "Behringer ADA8200").ProductID, ImageNumber = 0 },

                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "monitorgibbonair.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "Monkey Banana Gibbon Air").ProductID, ImageNumber = 0 },
                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "monitorkrk.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "KRK Rokit RP5 Studio Monitors").ProductID, ImageNumber = 0 },

                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "softwareflstudioproduceredition.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "FL Studio Producer Edition").ProductID, ImageNumber = 0 },

                        new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "softwareserum.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "Xfer Records Serum").ProductID, ImageNumber = 0 },

                              new ProductImageMapping { ProductImageID= images.Single(i => i.FileName == "midimaudiooxygen49.jpg").ID,
                    ProductID = products.Single( c=> c.ProductName == "MAudio Oxygen 49").ProductID, ImageNumber = 0 },

            };

            imageMappings.ForEach(c => context.ProductImageMappings.AddOrUpdate(im => im.ProductImageID, c));
            context.SaveChanges();


            var orders = new List<Order>
            {
                new Order {  DeliveryAddress = new Address{AddressLine1 = "53 Fox St", Town = "Hartbeesfontein", County = "North West", Postcode = "2600"},
                             TotalPrice =2500.99M, UserID="joe@examplecustomer.com", DateCreated=new DateTime(2020, 3, 22) ,DeliveryName="Joe Black" }
                  ,
                 new Order {  DeliveryAddress = new Address{AddressLine1 = "691 Kamp St",Town = "Belhar",County = "Western Cape",Postcode = "7504"},
                             TotalPrice =7500.99M, UserID="ntobeko@examplecustomer.com", DateCreated=new DateTime(2020, 6, 12) ,DeliveryName="Ntobeko Nhlanhla" }
                 ,
                  new Order {  DeliveryAddress = new Address{AddressLine1 = "481 Main Rd",Town = "Stanger",County = "KwaZulu-Natal",Postcode = "4453"},
                             TotalPrice =800.99M, UserID="vilina@examplecustomer.com", DateCreated=new DateTime(2020, 5, 8) ,DeliveryName="Maharaj Vilina" }

            };

            orders.ForEach(c => context.Orders.AddOrUpdate(o => o.DateCreated, c));
            context.SaveChanges();

            var orderRows = new List<OrderRow>
            {
                new OrderRow { OrderID=1, ProductID = products.Single( c=> c.ProductName == "MAudio Oxygen 49").ProductID,
                    ProductName ="MAudio Oxygen 49", Quantity =1, UnitPrice=products.Single( c=> c.ProductName == "MAudio Oxygen 49").ProductPrice },

                  new OrderRow { OrderID=2, ProductID = products.Single( c=> c.ProductName == "KRK Rokit RP5 Studio Monitors").ProductID,
                    ProductName ="KRK Rokit RP5 Studio Monitors", Quantity =1, UnitPrice=products.Single( c=> c.ProductName == "KRK Rokit RP5 Studio Monitors").ProductPrice },

                   new OrderRow { OrderID=3, ProductID = products.Single( c=> c.ProductName == "BM800 Condensor Microphone").ProductID,
                    ProductName ="BM800 Condensor Microphone", Quantity =1, UnitPrice=products.Single( c=> c.ProductName == "BM800 Condensor Microphone").ProductPrice }

            };

            orderRows.ForEach(c => context.OrderRows.AddOrUpdate(or => or.OrderID, c));
            context.SaveChanges();



        }
    }
}
