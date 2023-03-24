using KTMetoda.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace KTMetoda
{
    /// <summary>
    /// Interaction logic for IzracunWindow.xaml
    /// </summary>
    /// 
    public partial class IzracunWindow : Window
    {
        public ObservableCollection<string> Alternative { get; set; }
        public ObservableCollection<Parameter> Parametri { get; set; }
        public DataTable Data { get; set; }


        public IzracunWindow(ObservableCollection<string> Alternative, ObservableCollection<Parameter> Parametri)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            this.Parametri = Parametri;
            DataContext = this;

            int steviloVrst = Parametri.Count;
            int steviloStolpcev = Alternative.Count;

            //List<int> data = new List<int>();
            List<int> data = new List<int>();
            ////// initialize the data source with the desired number of objects
            //for (int i = 0; i < steviloVrst; i++)
            //{
            //    data.Add(0);
            //}

            //// set the data source for the DataGrid
            //GridAlternative.ItemsSource = data;

            foreach (string alternativa in Alternative)
            {
                int item = 0;
                data.Add(item);

                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = alternativa;
                column.Binding = new Binding(alternativa);
                column.Width = 200;
                column.IsReadOnly = false;
                GridAlternative.Columns.Add(column);
            }
            GridAlternative.ItemsSource = data;
        }
    }
}