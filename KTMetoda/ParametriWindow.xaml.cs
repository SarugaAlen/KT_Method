using KTMetoda.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace KTMetoda
{
    /// <summary>
    /// Interaction logic for ParametriWindow.xaml
    /// </summary>
    public partial class ParametriWindow : Window
    {
        public ObservableCollection<string> Alternative;
        public ObservableCollection<Dictionary<string, int>> Parametri { get; set; }

        public ParametriWindow(ObservableCollection<string> Alternative)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            Parametri = new ObservableCollection<Dictionary<string, int>>();
            DataContext = this;
        }

        private void DodajParameter_Click(object sender, RoutedEventArgs e)
        {
            //ParametriListBox.ItemsSource = Parametri;
            Dictionary<string, int> parameter = new Dictionary<string, int>();
            parameter.Add(VnosParameter.Text, Convert.ToInt32(Utez.Content));
            Parametri.Add(parameter);
            VnosParameter.Text = "";
            Utez.Content = "";
        }

        private void IzbrisParameter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NaprejOceno_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
