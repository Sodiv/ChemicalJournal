using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    public class Department
    {
        public Department()
        {
            Kredits = new HashSet<Kredit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Kredit> Kredits { get; set; }
    }
}
