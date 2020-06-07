using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Models
{
    class ProductList
    {
        public int NextSupply { get; set; }
        public double MaxCount { get; set; }

        public double CurrentSum;
        public List<Product> List { get; set; }

        public ProductList(Product product, int _nextSupply)
        {
            this.List = new List<Product>() { product };
            this.NextSupply = _nextSupply;
            this.MaxCount = product.count;
            this.CurrentSum = product.count;
        }
        public void RemoveFirst()
        {
            this.List.RemoveAt(0);
        }
        public void Add(Product product)
        {
            this.List.Add(product);
        }
        public void UpdateMax()
        {
            UpdateCurrentSum();
            if (CurrentSum > this.MaxCount) this.MaxCount = CurrentSum;
            
        }

        public void UpdateCurrentSum()
        {
            CurrentSum = 0;
            foreach (Product product in this.List)
            {
                CurrentSum += product.count;
            }           
        }
        public override string ToString()
        {
            string result = "";
            foreach (Product product in this.List)
            {
                result = result + product.ToString() + "\n";
            }
            result = result + "Поставка через: " + this.NextSupply + "\n";
            result = result + "Максимум: " + this.MaxCount;
            return result;
        }
    }
}
