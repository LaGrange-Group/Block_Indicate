using System;
using System.Collections.Generic;
using System.Text;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Block_Indicate.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FourHourDoji> FourHourDojis { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Block_Indicate-EDDD8D4E-C699-40FD-8558-960B75DAC603;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public ApplicationDbContext()
            : base()
        {
        }
    }
}
