using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;

namespace Supermarkets.Models
{
    class TimeMarketModel : BindableBase
    {        
        public Random rnd = new Random();
        public bool pause;
        public int SelectedMarket = 0;
        public int DelayTime = 1;
        public int ModelingTime = 60;
        private int[] speeds = new int[] { 1000, 500, 200, 100, 20, 0};
        public int[] ChartSteps = new int[] { 1, 2, 5, 10, 50, 100};
        public int[] ChartDetails = new int[] { 10, 15, 20, 25, 30};       
        public int CurrentSpeed = 0;
        public int ChartDetail = 0;
        private string[] productsNames = { "A", "B", "C", "D", "E", "F", "G" };
        private int[] productsTerm = { 72, 72, 504, 2160, 72, 8760, 8760 };

        public ObservableCollection<int> ProductsSupplies;
        public ObservableCollection<int> SupplyIntervals;
        public ObservableCollection<double> ShoppingСart;

        private int currentDay = 0;

        public int ChartStep = 1;
        public string CurrentDay { get { return "День: " + currentDay; } }

        private int currentTime = 0;
        public string CurrentTime{ get { return "Час: " + currentTime; } }

        public SeriesCollection Chart = new SeriesCollection();
        public List<string> Labels = new List<string>();
        private Market market;

        public ObservableCollection<Market> Market { get; }
        

        public TimeMarketModel()
        {
            Market = new ObservableCollection<Market>();           
            ChangeMarket(0,true);                      
        }
        private void SetStartChart()
        {
            ProductList[] productSums = market.ToArray();
            for (int i = 0; i < 7; i++)
            {
                Chart.Add(new LineSeries
                {
                    Title = "Product " + productsNames[i],
                    Values = new ChartValues<double> { productSums[i].CurrentSum }
                });
            }
            Labels.Add(currentDay + ":" + currentTime);
        }

        public void OneHour()
        {
            int customersInHour = 0;
            market.Customers = 0;
            market.PotentialServed = 0;

            for (int i = 0; i < 7; i++)
            {
                if (market.ToArray()[i].NextSupply == 0)
                {
                    if (market.ToArray()[i].List.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Плановая поставка товара " + productsNames[i] + " Количество: +" + ProductsSupplies[i] + "\t\t\tДень: " + currentDay + " Время: " + currentTime);
                    }
                    market.ToArray()[i].Add(new Product(productsTerm[i], ProductsSupplies[i]));
                    market.ToArray()[i].UpdateMax();
                    market.ToArray()[i].NextSupply = SupplyIntervals[i];
                }
                market.ToArray()[i].NextSupply--;
            }

            if (currentTime >= 8 && currentTime < 21)
            {
                if (currentTime >= 8 && currentTime < 13) customersInHour = rnd.Next(80, 120);
                if (currentTime >= 13 && currentTime < 17) customersInHour = rnd.Next(120, 180);
                if (currentTime >= 17 && currentTime < 21) customersInHour = rnd.Next(400, 500);
                market.Customers = customersInHour;
                market.PotentialServed = rnd.Next(55, 65) * market.Cashboxes;

                for (int i = 0; i < 7; i++)
                {
                    if (market.ToArray()[i].List.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Товара " + productsNames[i] + " нет на складе. " + "Упущенная реализация товара: " + String.Format("{0:0.00}", ShoppingСart[i] * customersInHour) + "\tДень: " + currentDay + " Время: " + currentTime);
                        continue;
                    }

                    Product product = market.ToArray()[i].List[0];
                    if (product.term < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Внимание! Товар " + productsNames[i] + " Просрочен." + " Количество: " + product.count + "\t\tДень: " + currentDay + " Время: " + currentTime);
                        market.ToArray()[i].List.RemoveAt(0);
                        continue;
                    }
                    if (market.ToArray()[i].List.Count == 1)
                    {
                        double productBought = ShoppingСart[i] * customersInHour;
                        if (product.count - productBought >= 0)
                        {
                            product.count = Math.Round(product.count - productBought, 2);
                            continue;
                        }
                        else
                        {
                            market.ToArray()[i].RemoveFirst();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Товар " + productsNames[i] + " полность закончился. Не хватило: " + Math.Round(productBought - product.count) + "\t\t\tДень: " + currentDay + " Время: " + currentTime);
                            continue;
                        }
                    }
                    product.count = Math.Round(product.count - ShoppingСart[i] * customersInHour, 2);
                    if (product.count <= 0)
                    {
                        market.ToArray()[i].List[1].count += product.count;
                        market.ToArray()[i].List.RemoveAt(0);
                        continue;
                    }
                }
            }
          
            currentTime++;
            
            foreach (ProductList listProduct in market)
            {
                listProduct.UpdateCurrentSum();
                foreach (Product product in listProduct.List)
                {
                    product.term--;
                }
            }

            if (currentTime == 24)
            {
                currentDay++;
                RaisePropertyChanged("CurrentDay");
                currentTime = 0;
            }
            RaisePropertyChanged("CurrentTime");           
            

        }


        public void ChangeMarket(int index, bool reset)
        {
            currentDay = 0;
            currentTime = 0;
            pause = true;
            SelectedMarket = index;
            
            if (index == 0)
            {
                if (reset)
                {
                    ShoppingСart = new ObservableCollection<double> { 0.5, 0.3, 1, 1, 1, 0.2, 1 };
                    SupplyIntervals = new ObservableCollection<int> { 17, 22, 10, 10, 10, 38, 16 };
                    ProductsSupplies = new ObservableCollection<int> { 1000, 800, 1200, 1200, 1200, 1000, 2000 };
                }
                market = new Market("1-3", 0, 8, 0, new Product(72, 3000), new Product(72, 1600), new Product(504, 3600), new Product(2160, 3600), new Product(72, 2400), new Product(8760, 1000), new Product(8760, 2000), SupplyIntervals);
            }
            if (index == 1)
            {
                if (reset)
                {
                    ShoppingСart = new ObservableCollection<double> { 1, 1, 1.9, 0.4, 1, 0.4, 0.3 };
                    SupplyIntervals = new ObservableCollection<int> { 25, 20, 15, 44, 10, 20, 25 };
                    ProductsSupplies = new ObservableCollection<int> { 3000, 2400, 3600, 2400, 1200, 1000, 1000 };
                }
                market = new Market("4-7", 0, 8, 0, new Product(72, 3000), new Product(72, 3200), new Product(504, 4800), new Product(2160, 2400), new Product(72, 2400), new Product(8760, 1000), new Product(8760, 1000), SupplyIntervals);
            }
            if (index == 2)
            {
                if (reset)
                {
                    ShoppingСart = new ObservableCollection<double> { 0.7, 0.7, 2.5, 1.5, 1, 0.5, 0.8 };
                    SupplyIntervals = new ObservableCollection<int> { 24, 28, 16, 20, 10, 32, 20 };
                    ProductsSupplies = new ObservableCollection<int> { 2000, 2400, 4800, 3600, 1200, 2000, 2000 };
                }
                market = new Market("8-12", 0, 8, 0, new Product(72, 5000), new Product(72, 3200), new Product(504, 6000), new Product(2160, 4800), new Product(72, 2400), new Product(8760, 2000), new Product(8760, 3000), SupplyIntervals);
            }
            Labels.Clear();
            Chart.Clear();
            Market.Clear();           
            Market.Add(market);
            ChartDetail = 0;
            SetStartChart();
            RaisePropertyChanged("CurrentTime");
            RaisePropertyChanged("CurrentDay");
            Console.Clear();
            if (reset)
            {
                RaisePropertyChanged("ShoppingСart");
                RaisePropertyChanged("SupplyIntervals");
                RaisePropertyChanged("ProductsSupplies");
            }
        }
      
        public void UpdateSupplyParametersInProducts()
        {
            
            for (int i = 0; i < 7; i++)
            {                              
                    market.ToArray()[i].NextSupply = SupplyIntervals[i];              
            }
        }
        
        public void OneHourSync()
        {
            OneHour();          
            UpdateIntetface();
        }
        public async void OneHourAsync()
        {                       
            while (currentDay < ModelingTime)
            {
                if (pause) return;
                else
                {                   
                    await Task.Run(() => OneHour());
                    await Task.Delay(speeds[CurrentSpeed]);
                    UpdateIntetface();
                }
               
            }                    
        }

        public void UpdateIntetface()
        {
            ChartStep--;
            Market.Clear();
            Market.Add(market);
            if (ChartStep == 0)
            {
                while (Labels.Count > ChartDetails[ChartDetail]) Labels.RemoveAt(0);
                Labels.Add(currentDay + ":" + currentTime);
                ProductList[] productSums = market.ToArray();
                for (int i = 0; i < 6; i++)
                {
                    while (Chart[i].Values.Count > ChartDetails[ChartDetail]) Chart[i].Values.RemoveAt(0);
                    Chart[i].Values.Add(productSums[i].CurrentSum);
                }
                ChartStep = ChartSteps[CurrentSpeed];
            }
        }
    }
}
