using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Models
{
    public class message
    {
        public bool status { get; set; }
        public string mensaje { get; set; }

        public List<Comercio> data { get; set; }
    }


}
