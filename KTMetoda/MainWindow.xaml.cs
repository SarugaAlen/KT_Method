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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KTMetoda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Alternative { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Alternative = new ObservableCollection<string>();
            DataContext = this;
        }

        private void DodajAlternativo_Click(object sender, RoutedEventArgs e)
        {
            string alternativa = VnosAlternative.Text;
            Alternative.Add(alternativa);
        }

        private void IzbrisiAlternativo_Click(object sender, RoutedEventArgs e)
        {
            string selectedAlternative = AlternativaListBox.SelectedItem as string;
            if (AlternativaListBox.Items.Count == 0)
            {
                MessageBox.Show("Napak: Seznam  je prazen!", "Napaka", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (selectedAlternative != null)
                {
                    Alternative.Remove(selectedAlternative);
                }
            }           
        }
        private void NaprejParametre_Click(object sender, RoutedEventArgs e)
        {
            if (Alternative.Count == 0)
            {
                MessageBox.Show("Napaka: Seznam alternativ je prazen!", "Napaka", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ParametriWindow parametriWindow = new ParametriWindow(Alternative);
            parametriWindow.Show();
            this.Close();
        }
    }
}
