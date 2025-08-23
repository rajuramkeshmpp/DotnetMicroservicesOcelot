using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } 

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
