using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OrderService.Models;

namespace OrderService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }

    }
}
