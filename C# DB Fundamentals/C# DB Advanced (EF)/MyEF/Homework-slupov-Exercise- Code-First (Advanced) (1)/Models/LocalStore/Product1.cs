﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.LocalStore
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DistributorName { get; set; }
        public string Desctiption { get; set; }
        public decimal Price { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }

    }
}