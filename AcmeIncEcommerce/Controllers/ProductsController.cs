using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AcmeIncEcommerce.DataAccessLayer;
using AcmeIncEcommerce.Models;
using PagedList;

namespace AcmeIncEcommerce.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Products
        [AllowAnonymous]
        public ActionResult Index(string category,string search, string sortBy, int? page)
        {
            ProductIndexViewModel vm = new ProductIndexViewModel();

            var products = db.Products.Include(p => p.Category);

          
            //if the passed search phrase variable value is not equal to null then 
            //the products variable declared above will be instantiated with db product table data that has been filtered to get values that are relevant to the searchPhrase .
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                p.ProductDescription.Contains(search) ||
                p.Category.CategoryName.Contains(search));
                vm.Search = search;
            }

            vm.CatsWithCount = from matchingProducts in products
                                      where
                                      matchingProducts.CategoryID != null
                                      group matchingProducts by
                                       matchingProducts.Category.CategoryName into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };

            //if the passed category choice variable value is not equal to null then 
            //the products variable declared above will be instantiated with db product table data that has been filtered through query to get values relating to the category value. 
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.CategoryName == category);
                vm.Category = category;
            }

            //sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            int currentPage = (page ?? 1);
            vm.Products = products.ToPagedList(currentPage, Constants.PageItems);
            vm.SortBy = sortBy;
            vm.Sorts = new Dictionary<string, string>
            {
                {"Price low to high", "price_lowest" },
                {"Price high to low", "price_highest" }
            };



            return View(vm);
        }

        [AllowAnonymous]
        // GET: Products/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName");
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            Product product = new Product();
            product.ProductName = viewModel.ProductName;
            product.ProductDescription = viewModel.ProductDescription;
            product.ProductPrice = viewModel.ProductPrice;
            product.CategoryID = viewModel.CategoryID;
            product.ProductImageMappings = new List<ProductImageMapping>();

            //get a list of product images
            string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
           
                product.ProductImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = db.ProductImages.Find(int.Parse(productImages[0])),
                    ImageNumber = 0
                });
            

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "ProductName", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", viewModel.ProductImages[0]));
            }
            return View(viewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();

            foreach (var imageMapping in product.ProductImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", imageMapping.ProductImageID));
            }

            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }

            viewModel.ProductID = product.ProductID;
            viewModel.ProductName = product.ProductName;
            viewModel.ProductDescription = product.ProductDescription;
            viewModel.ProductPrice = product.ProductPrice;

            return View(viewModel);
        }


        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            var productToUpdate = db.Products.Include(p => p.ProductImageMappings).Where(p => p.ProductID == viewModel.ProductID).Single();

            if (TryUpdateModel(productToUpdate, "", new string[] { "ProductName", "ProductDescription", "ProductPrice", "CategoryID" }))
            {
                if (productToUpdate.ProductImageMappings == null)
                {
                    productToUpdate.ProductImageMappings = new List<ProductImageMapping>();
                }
                //get a list of product images
                string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < productImages.Length; i++)
                {
                   
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    
                    var image = db.ProductImages.Find(int.Parse(productImages[i]));
                 
                    if (imageMappingToEdit == null)
                    {
                     
                        productToUpdate.ProductImageMappings.Add(new ProductImageMapping
                        {
                            ImageNumber = i,
                            ProductImage = image,
                            ProductImageID = image.ID
                        });
                    }
                
                    else
                    {
                   
                        if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i]))
                        {
                           
                            imageMappingToEdit.ProductImage = image;
                        }
                    }
                }

              
                for (int i = productImages.Length; i < Constants.NumberOfProductImages; i++)
                {
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                 
                    if (imageMappingToEdit != null)
                    {
                     
                        db.ProductImageMappings.Remove(imageMappingToEdit);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // POST: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);

            var orderRows = db.OrderRows.Where(ol => ol.ProductID == id);
            foreach (var ol in orderRows)
            {
                ol.ProductID = null;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
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
