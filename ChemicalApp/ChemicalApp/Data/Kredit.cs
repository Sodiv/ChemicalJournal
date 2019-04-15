using System;

namespace ChemicalApp.Data
{
    public class Kredit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Sum { get; set; }
        public string Comment { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
