using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    public partial class Product
    {
        public Product()
        {
            Debets = new HashSet<Debet>();
            Kredits = new HashSet<Kredit>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Debet> Debets { get; set; }
        public virtual ICollection<Kredit> Kredits { get; set; }
    }
}
