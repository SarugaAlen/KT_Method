using KTMetoda.Model;
using System;
using System.Collections;
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;


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

        List<int> vsote = new List<int>();


        public IzracunWindow(ObservableCollection<string> Alternative, ObservableCollection<Parameter> Parametri)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            this.Parametri = Parametri;
            DataContext = this;

            int steviloVrst = Parametri.Count;
            int steviloStolpcev = Alternative.Count;


            for (int i = 0; i < steviloVrst; i++)
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
            AddTextBlocks(steviloStolpcev);
            GridAlternative.ItemsSource = Data;
            GridAlternative.AutoGenerateColumns = false;
            GridAlternative.CanUserAddRows = false;

            GridAlternative.CellEditEnding += GridAlternative_CellEditEnding;
        }
        private void GridAlternative_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the edited cell
            var cell = e.EditingElement as TextBox;

            // If the cell is null or empty, return
            if (cell == null || string.IsNullOrWhiteSpace(cell.Text))
            {
                return;
            }

            // Get the row and column of the edited cell
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Convert the new value to an integer
            if (!int.TryParse(cell.Text, out int newValue) || newValue < 1 || newValue > 10)
            {
                // If the user entered an invalid value, display a message and reset the cell's value
                MessageBox.Show("Napačen vnos! Vnesite število med 1 in 10");
                return;
            }

            // If the list for this column doesn't have enough items (rows), add them
            while (Data[columnIndex].Count <= rowIndex)
            {
                Data[columnIndex].Add(0);
            }

            // Update the corresponding item in the Data list with the new value
            Data[columnIndex][rowIndex] = newValue;

            // Cancel the edit so that the new value is not discarded
            e.Cancel = true;

            // Update the cell's value to the new value
            cell.Text = newValue.ToString();
        }

        private void AddTextBlocks(int alternative)
        {
            Rezultati.Children.Clear();
            for (int i = 0; i < alternative; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = 200;
                textBlock.FontSize = 40;
                Rezultati.Children.Add(textBlock);
            }
        }

        private void Izracunaj_Click(object sender, RoutedEventArgs e)
        {
            int najvecjaVsota = int.MinValue;
            int najvecjiStolpecIndex = -1;

            for (int i = 0; i < Rezultati.Children.Count; i++)
            {
                List<int> column = Data[i];

                int index = 0;

                foreach (Parameter parameter in Parametri)
                {
                    column[index] *= parameter.Utez;
                    index++;
                }
                int sum = column.Sum();
                vsote.Add(sum);
                Rezultati.Children[i].SetValue(TextBlock.TextProperty, sum.ToString());

                if (sum > najvecjaVsota)
                {
                    najvecjaVsota = sum;
                    najvecjiStolpecIndex = i;
                }
            }
            Najboljsa.Text = "Najboljša alternativa je " + Alternative[najvecjiStolpecIndex] + " z " + najvecjaVsota + " točkami";
            //MessageBox.Show("Najboljša alternativa je " + Alternative[najvecjiStolpecIndex] + " z " + najvecjaVsota + " točkami");
        }

        private void AlternativeChart_Click(object sender, RoutedEventArgs e)
        {
            var chart = new LiveCharts.Wpf.CartesianChart();
            var series = new LiveCharts.Wpf.ColumnSeries
            {
                Title = "Vrednosti",
                Values = new LiveCharts.ChartValues<int>(vsote),
                DataLabels = true,
                LabelPoint = point => $"{point.Y}"
            };
            chart.Series.Add(series);

            chart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = new LiveCharts.ChartValues<string>(Alternative),
                FontSize = 16
            });

            var window = new Window
            {
                Content = chart,
                Width = 800,
                Height = 600
            };
            window.ShowDialog();
        }

        private void ParametriChart_Click(object sender, RoutedEventArgs e)
        {
            PieChart pieChart = new PieChart
            {
                LegendLocation = LegendLocation.Bottom
            };

            foreach (Parameter parameter in Parametri)
            {
                PieSeries pieSeries = new PieSeries
                {
                    Title = parameter.Ime,
                    Values = new ChartValues<int> { parameter.Utez },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.SeriesView.Title} ({chartPoint.Participation:P})"
                };
                pieChart.Series.Add(pieSeries);
            }

            var window = new Window
            {
                Content = pieChart,
                Width = 800,
                Height = 600
            };
            window.ShowDialog();
        }

        private void AnalizaObcutljivosti_Click(object sender, RoutedEventArgs e)
        {
            // torej potem mores naredit da uporabnik izbere za kateri parameter hoce spreminjat vrednost od 1 do 10
            // in potem se moras enkrat izracunat vse vrednosti v tabeli in vskako shrani (pomoje najboljse da v nek list in potem naredis iz tega graf) 
            //torej vsaka alternativa dobi svoj list ko bo mel not vse izracune ko se utez spremeni od 1 do 10
            List<Sestevek> sestevki = new List<Sestevek>();
            Parameter izbran = ComboBoxParametrov.SelectedItem as Parameter;
            int[] utezi = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            foreach (int utez in utezi)
            {
                izbran.Utez = utez;
                Izracunaj_Click(sender, e);
                int index = 0;
                foreach (TextBlock textBlock in Rezultati.Children)
                {
                    sestevki.Add(new Sestevek(Alternative[index], int.Parse(textBlock.Text)));
                    index++;
                }              
            }

            MessageBox.Show(sestevki.First().ToString());

            //MessageBox.Show(izbran.Ime);

            //for (int i = 0; i < 11; i++)
            //{
            //    Sestevek sestevek = new Sestevek();
            //    for (int j = 0; j < Alternative.Count; j++)
            //    {
            //        List<int> column = Data[j];

            //        int index = 0;
            //        izbran.Utez = i;
            //        foreach (Parameter parameter in Parametri)
            //        {
            //            column[index] *= parameter.Utez;
            //            index++;
            //        }
            //        int sum = column.Sum();
            //        string alternativa = Alternative[j];
            //        sestevek.Ime = alternativa;
            //        sestevek.Vrednost = sum;

            //    }
            //    sestevki.Add(sestevek);
            //}
            //Izracunaj_Click(sender, e);
            //List<int> vsote = new List<int>();
            //foreach (int vsota in this.vsote)
            //{
            //    vsote.Add(vsota);
            //}
        }


    }
}