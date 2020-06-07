using LiveCharts;
using Supermarkets.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.ViewModels
{
    class TimeMarketViewModel : BindableBase
    {
        readonly TimeMarketModel _model = new TimeMarketModel();
        public ObservableCollection<Market> Markets => _model.Market;

        public SeriesCollection Chart => _model.Chart;
        public List<String> Labels => _model.Labels;
        public TimeMarketViewModel()
        {
            //таким нехитрым способом мы пробрасываем изменившиеся свойства модели во View
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };           
            StartPause = new DelegateCommand(() => {
                if (_model.pause)
                {
                    _model.pause = false;
                    _model.OneHourAsync();                  
                }
                else
                {
                    _model.pause = true;
                }

            });           
            Stop = new DelegateCommand(() => { 
                _model.ChangeMarket(_model.SelectedMarket, false);
                _model.CurrentSpeed = 0;
                
            });
            Reset = new DelegateCommand(() => { _model.ChangeMarket(_model.SelectedMarket, true); });
            Step = new DelegateCommand(() => { _model.OneHourSync(); });
            UpdateSupply = new DelegateCommand(() => { _model.UpdateSupplyParametersInProducts(); });
        }

        public int ModelingTime
        {
            get { return _model.ModelingTime; }
            set
            {
                if ((value > 360) || (value < 1)) _model.ModelingTime = 60;
                else _model.ModelingTime = value;
                RaisePropertyChanged("ModelingTime");
            }
        }
        public string CurrentTime => _model.CurrentTime;
        public string CurrentDay => _model.CurrentDay;
        public ObservableCollection<int> SupplyIntervals
        {
            get { return _model.SupplyIntervals; }
            set
            {

                _model.SupplyIntervals = value;               
                RaisePropertyChanged("SupplyIntervals");
            }
        }
        public ObservableCollection<int> ProductsSupplies
        {
            get { return _model.ProductsSupplies; }
            set
            {
                _model.ProductsSupplies = value;
                RaisePropertyChanged("SupplyIntervals");
            }
        }
        public ObservableCollection<double> ShoppingСart
        {
            get { return _model.ShoppingСart; }
            set
            {
                _model.ShoppingСart = value;
                RaisePropertyChanged("ShoppingСart");
            }
        }
        public int SelectMarket 
        {
            get { return _model.SelectedMarket; }
            set
            {
                _model.ChangeMarket(value,true);
                
            }
        }
        public int SelectChartDetail
        {
            get { return _model.ChartDetail; }
            set
            {
                _model.ChartDetail=value;

            }
        }
        public int SelectSpeed
        {
            get { return _model.CurrentSpeed; }
            set
            {
                _model.CurrentSpeed = value;
                _model.ChartStep = _model.ChartSteps[value];
            }
        }
        public DelegateCommand StartPause { get; }       
        public DelegateCommand Stop { get; }
        public DelegateCommand Reset { get; }
        public DelegateCommand Step { get; }

        public DelegateCommand UpdateSupply  {get; }
}
}
