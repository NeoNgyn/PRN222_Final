using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
    public class DataContext : IdentityDbContext<User>
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình thuộc tính Price trong ProductModel
            // Ví dụ: decimal(18, 2) nghĩa là tổng cộng 18 chữ số, với 2 chữ số sau dấu thập phân
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Hoặc decimal(10,2), decimal(19,4), tùy theo nhu cầu của bạn

            base.OnModelCreating(modelBuilder);
        }
    }
}
