using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ProductService.Models;

namespace ProductService
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product>Products { get; set; }
        
    }
}
