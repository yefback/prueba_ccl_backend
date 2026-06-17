using Microsoft.EntityFrameworkCore;
using PruebaCCL.Backend.Models;

namespace PruebaCCL.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
    }
}
