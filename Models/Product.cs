using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Models
{
    class Product
    {
        public int term;
        public double count;

        public Product(int date, double _quantity)
        {
            this.term = date;
            this.count = _quantity;
        }
        public override string ToString()
        {

            return ("Срок: " + this.term.ToString() + " " + "Склад: " + this.count.ToString());
        }
    }
}
