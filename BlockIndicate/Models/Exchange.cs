﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class Exchange
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
