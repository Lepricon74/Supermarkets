using MaterialDesignThemes.Wpf;
using Supermarkets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Supermarkets
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new TimeMarketViewModel();
        }
      
        private void Modeling_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow.Visibility = Visibility.Collapsed;
            ModelWindow.Visibility = Visibility.Visible;
            SpeedBox.SelectedIndex = 0;

        }

        private void StopModeling_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow.Visibility = Visibility.Visible;
            ModelWindow.Visibility = Visibility.Collapsed;
            SpeedsButtons.Visibility = Visibility.Collapsed;
            PlayPause.Kind = PackIconKind.Play;
        }   

        private void StartModeling_Click(object sender, RoutedEventArgs e)
        {
            if (SpeedsButtons.Visibility == Visibility.Collapsed)
            {
                SpeedsButtons.Visibility = Visibility.Visible;
                PlayPause.Kind = PackIconKind.Pause;
            }
            else
            {
                SpeedsButtons.Visibility = Visibility.Collapsed;
                PlayPause.Kind = PackIconKind.Play;
            }
        }
    }
}
