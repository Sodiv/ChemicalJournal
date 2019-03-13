﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp.Data
{
    public class Debet
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Sum { get; set; }
                
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
