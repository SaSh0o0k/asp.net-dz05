using Microsoft.EntityFrameworkCore;
using WebApplication99.Data.Entities;
using WebApplication99.Interfaces;

namespace WebApplication99.Data
{
    public class DataEFContext : DbContext
    {
        public DataEFContext(DbContextOptions<DataEFContext> options)
            : base(options) { }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
    }

}
