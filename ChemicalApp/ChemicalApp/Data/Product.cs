using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Debet> Debets { get; set; }
        public ICollection<Kredit> Kredits { get; set; }
    }
}
