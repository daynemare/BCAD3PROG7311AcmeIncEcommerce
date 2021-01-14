using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcmeIncEcommerce.Models
{
    public class Product
    {

        public int ProductID { get; set; }
        [Required(ErrorMessage = "The product name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "A Product name should range from 3 to 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "The Product name should be composed only of letters and numbers")]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        
        [Required(ErrorMessage = "The product description cannot be left blank")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Please enter a product description between 10 and 200 characters in length")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please enter a product description made up of letters and numbers only")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }


        [Required(ErrorMessage = "The price cannot be left blank")]
        [Range(0.10, 100000.99, ErrorMessage = "Please enter a price between 0.10 and 100000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [RegularExpression(@"^(?:\d|[,])+$", ErrorMessage = "The price must be a number up to two decimal places (comma)")]
        [Display(Name = "Price")]

        public decimal ProductPrice { get; set; }

        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductImageMapping> ProductImageMappings { get; set; }

    }
}