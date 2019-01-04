using System;
using System.Collections.Generic;
using System.Text;
using BlockIndicate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlockIndicate.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BinanceData> BinanceData { get; set; }
        public DbSet<HuobiData> HuobiData { get; set; }
        public DbSet<DojiFinder> DojiFinders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-BlockIndicate-9511E877-3BD8-4E77-9C3A-3C29B4F74771;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        public ApplicationDbContext()
            : base()
        {
        }
    }
}
