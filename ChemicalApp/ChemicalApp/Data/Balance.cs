using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    public class Balance
    {
        public int Id { get; set; }
        public int Sum { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
