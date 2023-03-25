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
        public List<List<int>> Data { get; set; } = new List<List<int>>();


        public IzracunWindow(ObservableCollection<string> Alternative, ObservableCollection<Parameter> Parametri)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            this.Parametri = Parametri;
            DataContext = this;

            int steviloVrst = Parametri.Count;
            int steviloStolpcev = Alternative.Count;

            for (int i = 0; i < steviloStolpcev; i++)
            {
                Data.Add(new List<int>());
            }

            for (int i = 0; i < steviloStolpcev; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = Alternative[i];
                column.Width = 200;
                column.IsReadOnly = false;
                column.Binding = new Binding($"Data[{i}]"); // Bind to the entire list for the corresponding row index
                GridAlternative.Columns.Add(column);
            }

            GridAlternative.ItemsSource = Data;
            GridAlternative.AutoGenerateColumns = false;
            GridAlternative.CanUserAddRows = false;

            GridAlternative.CellEditEnding += GridAlternative_CellEditEnding;
        }
        private void GridAlternative_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the row and column of the edited cell
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Convert the new value to an integer
            int intValue;
            if (int.TryParse(newValue, out intValue))
            {
                // If the list for this column doesn't have enough items (rows), add them
                while (Data[columnIndex].Count <= rowIndex)
                {
                    Data[columnIndex].Add(0);
                }

                // Update the corresponding item in the Data list with the new value
                Data[columnIndex][rowIndex] = intValue;

                // Cancel the edit so that the new value is not discarded
                e.Cancel = true;

                // Update the cell's value to the new value
                var cell = e.Column.GetCellContent(e.Row);
                if (cell != null)
                {
                    ((TextBox)cell).Text = newValue;
                }
            }
            else
            {
                // If the user entered an invalid value, display a message and reset the cell's value
                MessageBox.Show("Invalid input. Please enter an integer value.");
                ((TextBox)e.EditingElement).Text = Data[columnIndex][rowIndex].ToString();
            }
        }
    }
}