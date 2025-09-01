using Microsoft.EntityFrameworkCore;

namespace SSHouseOwnerService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        
    }
}
