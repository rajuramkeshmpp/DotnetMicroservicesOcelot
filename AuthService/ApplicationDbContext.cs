using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Role> Roles { get; set; }
    }
}
