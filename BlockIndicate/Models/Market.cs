using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class Market
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
