using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    public class ChemicalContext : DbContext
    {
        public ChemicalContext() : base("name=ChemicalDB")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Debet> Debets { get; set; }
        public DbSet<Kredit> Kredits { get; set; }
        public DbSet<Balance> Balances { get; set; }
    }
}
