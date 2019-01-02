using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Customer")]
        public string Name { get; set; }

        //[Display(Name = "Phone Number")]
        //public string PhoneNumber { get; set; }

        [NotMapped]
        public bool isSuperAdmin { get; set; }
    }
}
