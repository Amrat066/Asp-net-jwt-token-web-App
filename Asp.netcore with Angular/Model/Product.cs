using System.ComponentModel.DataAnnotations;

namespace Asp.netcore_with_Angular.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Product Name is required.")]
        //[MinLength(1, ErrorMessage = "Product Name cannot be empty.")]
        public string ProductName { get; set; }

        //[Required(ErrorMessage = "Price is required.")]
        public int Price { get; set; }

        //[Required(ErrorMessage = "Description is required.")]
        //[MinLength(1, ErrorMessage = "Description cannot be empty.")]
        public string Description { get; set; }
        public int? Rating { get; set; }
        public bool Status { get; set; }
    }
}
