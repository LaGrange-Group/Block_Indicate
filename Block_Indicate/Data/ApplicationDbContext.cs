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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
