using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    class Debet
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Sum { get; set; }

        public virtual Product Product { get; set; }
    }
}
