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

        public List<List<int>> Kopija { get; set; }

        public List<List<int>> Koncne { get; set; } = new List<List<int>>();

        List<int> vsote = new List<int>();


        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


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

            SeriesCollection = new SeriesCollection { };

        }
        private void GridAlternative_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var cell = e.EditingElement as TextBox;

            if (cell == null || string.IsNullOrWhiteSpace(cell.Text))
            {
                return;
            }
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            if (!int.TryParse(cell.Text, out int newValue) || newValue < 1 || newValue > 10)
            {
                MessageBox.Show("Napačen vnos! Vnesite število med 1 in 10");
                return;
            }

            while (Data[columnIndex].Count <= rowIndex)
            {
                Data[columnIndex].Add(0);
            }

            Data[columnIndex][rowIndex] = newValue;

            e.Cancel = true;
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
            Kopija = Data.Select(list => new List<int>(list)).ToList();

            int najvecjaVsota = int.MinValue;
            int najvecjiStolpecIndex = -1;
            //Kopija = Data.ToList();
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
            //Najboljsa.Text = "Najboljša alternativa je " + Alternative[najvecjiStolpecIndex] + " z " + najvecjaVsota + " točkami";
            MessageBox.Show("Najboljša alternativa je " + Alternative[najvecjiStolpecIndex] + " z " + najvecjaVsota + " točkami");
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
            Parameter izbran = ComboBoxParametrov.SelectedItem as Parameter;
            int utez = izbran.Utez;

            for (int i = 0; i < Rezultati.Children.Count; i++) // en column oz alternativa
            {
                List<int> column = Kopija[i];
                List<int> alternativ = new List<int>();

                for (int j = 1; j <= 10; j++)
                {
                    List<int> stolpec = column.ToList();
                    izbran.Utez = j;
                    int index = 0;
                    foreach (Parameter parameter in Parametri)
                    {
                        stolpec[index] *= parameter.Utez;
                        index++;
                    }
                    int sum = stolpec.Sum();
                    alternativ.Add(sum);
                    stolpec = column.ToList();
                }
                Koncne.Add(alternativ);
            }

            izbran.Utez = utez;
            
            SeriesCollection.Clear();
                       
            for (int k = 0; k < Alternative.Count; k++)
            {
                LineSeries newSeries = new LineSeries
                {
                    Title = Alternative[k],
                    Values = new ChartValues<int>(Koncne[k]),
                };

                SeriesCollection.Add(newSeries);               
            }
            var chart = new CartesianChart
            {
                Series = SeriesCollection,
            };
            chart.AxisX.Add(new Axis { Title = "Oteži", Labels = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" } });

            var window = new Window
            {
                Content = chart,
                Width = 800,
                Height = 600
            };
            window.ShowDialog();
            chart.Series.Clear();
            Koncne.Clear();
        }
    }
}

