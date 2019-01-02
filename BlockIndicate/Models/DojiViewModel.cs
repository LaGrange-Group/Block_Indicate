using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class DojiViewModel
    {
        public DojiFinder CreateDojiFined { get; set; }
        public List<DojiFinder> SetFinders { get; set; }
        public Customer Customer { get; set; }

    }
}
