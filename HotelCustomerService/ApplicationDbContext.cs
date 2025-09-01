using Microsoft.EntityFrameworkCore;

namespace HotelCustomerService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        
    }
}
