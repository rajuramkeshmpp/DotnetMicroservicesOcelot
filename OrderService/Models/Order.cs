using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; } 

       

        public string CustomerName { get; set; }
    }
}
