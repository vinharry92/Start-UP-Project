using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using No_Core_Auth.Model;

namespace No_Core_Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //create Roles for our Application
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new { Id = "1",Name="admin",NormalizedName="ADMIN"},
                new { Id = "2", Name = "customer", NormalizedName = "CUSTOMER" },
                new { Id = "3", Name = "moderator", NormalizedName = "MODERATOR" }
                );
        }

        public DbSet<ProductModel> productModels { get; set; }
    }
}
