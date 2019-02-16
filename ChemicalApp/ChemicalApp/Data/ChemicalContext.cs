using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    class ChemicalContext : DbContext
    {
        public ChemicalContext() : base("name=ChemicalDB")
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Debet> Debets { get; set; }
        public virtual DbSet<Kredit> Kredits { get; set; }
    }
}
