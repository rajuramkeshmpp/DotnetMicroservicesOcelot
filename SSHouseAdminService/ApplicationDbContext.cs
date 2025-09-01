using Microsoft.EntityFrameworkCore;

namespace SSHouseAdminService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       
    }
}
