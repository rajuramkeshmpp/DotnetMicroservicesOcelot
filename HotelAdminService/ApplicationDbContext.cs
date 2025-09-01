using Microsoft.EntityFrameworkCore;

namespace HotelAdminService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       
    }
}
