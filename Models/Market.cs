using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections;
using Supermarkets.Models;

namespace Supermarkets
{
    class Market : BindableBase, IEnumerable
    {
        public Market(string group, int _customers, int _cashboxes, int _served, Product a, Product b, Product c, Product d, Product e, Product f, Product g, ObservableCollection<int> _supplyIntervals)
        {
            this.MarketGroup = group;
            this.Customers = _customers;
            this.Cashboxes = _cashboxes;
            this.PotentialServed = _served;
            this.A = new ProductList(a, _supplyIntervals[0]);
            this.B = new ProductList(b, _supplyIntervals[1]);
            this.C = new ProductList(c, _supplyIntervals[2]);
            this.D = new ProductList(d, _supplyIntervals[3]);
            this.E = new ProductList(e, _supplyIntervals[4]);
            this.F = new ProductList(f, _supplyIntervals[5]);
            this.G = new ProductList(g, _supplyIntervals[6]);
        }
        public string MarketGroup { get; set; }
        public int Customers { get; set; }
        public int Cashboxes { get; set; }
        public int PotentialServed { get; set; }
        public ProductList A { get; set; }
        public ProductList B { get; set; }
        public ProductList C { get; set; }
        public ProductList D { get; set; }
        public ProductList E { get; set; }
        public ProductList F { get; set; }
        public ProductList G { get; set; }

        public ProductList[] ToArray()
        {
            return new ProductList[] { this.A, this.B, this.C, this.D, this.E, this.F, this.G };
        }

        public IEnumerator GetEnumerator()
        {
            return ToArray().GetEnumerator();
        }
    }
}
