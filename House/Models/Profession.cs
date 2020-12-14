using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Profession
    {
        public int ProfessionID { get; set; }
        public string Description { get; set; }
        public ICollection<Customer> customers { get; set; }
    }
}
