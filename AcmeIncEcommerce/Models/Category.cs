using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AcmeIncEcommerce.Models;

namespace AcmeIncEcommerce.Models
{
    public class Category
    {
       public int CategoryID {get;set;}

        [Required(ErrorMessage = "The category name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a category name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a category name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Category")]
        public string CategoryName {get;set; }

        public virtual ICollection <Product> Products { get; set; }
        
    }
}