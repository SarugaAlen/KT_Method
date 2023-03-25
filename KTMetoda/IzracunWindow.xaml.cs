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
            List<int> vsote = new List<int>();

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

            AlternativeChart.Content = chart;

            //ChartValues<ObservableValue> chartValues = new ChartValues<ObservableValue>();
            //foreach (Parameter parameter in Parametri)
            //{
            //    chartValues.Add(new ObservableValue(parameter.Utez));
            //}
            //PieSeries pieSeries = new PieSeries
            //{
            //    Title = "Parameters",
            //    Values = chartValues,
            //    DataLabels = true,
            //    LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
            //};
            //ParametriChart.Content = pieSeries;

            ChartValues<ObservableValue> chartValues = new ChartValues<ObservableValue>();
            foreach (Parameter parameter in Parametri)
            {
                chartValues.Add(new ObservableValue(parameter.Utez));
            }

            var piechart = new LiveCharts.Wpf.PieChart();
            // Create a PieSeries object and set its properties
            PieSeries pieSeries = new PieSeries
            {
                Title = "Parameters",
                Values = chartValues,
                DataLabels = true,
                LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.SeriesView.Title, chartPoint.Participation)
            };

            // Add the series to the chart control
            piechart.Series.Add(pieSeries);
            ParametriChart.Content = piechart;

            //chart.Series.Add(series);
            //var window = new Window
            //{
            //    Content = chart,
            //    Width = 800,
            //    Height = 600
            //};
            //window.ShowDialog();

            //var chart = new LiveCharts.Wpf.CartesianChart();

            //var series = new LiveCharts.Wpf.ColumnSeries
            //{
            //    Title = "Alternative",
            //    DataLabels = true,
            //    LabelPoint = point => $"{point.Y}"
            //};

            //foreach (var point in dataPoints)
            //{
            //    series.Values.Add(point);
            //}

            //chart.Series.Add(series);
            //var window = new Window
            //{
            //    Content = chart,
            //    Width = 800,
            //    Height = 600
            //};
            //window.ShowDialog();


            //var series = new LiveCharts.Wpf.ColumnSeries
            //{
            //    Title = "Alternative",
            //    Values = new LiveCharts.ChartValues<int>(dataPoints.Values),
            //    DataLabels = true,
            //    LabelPoint = point => $"{point.Y}"
            //};

            //chart.AxisX.Add(new LiveCharts.Wpf.Axis
            //{
            //    Title = "Črke",
            //    Labels = frekvenca.Keys.Select(c => c.ToString()).ToList()
            //});

            //chart.Series.Add(series);

            //var window = new Window
            //{
            //    Content = chart,
            //    Width = 800,
            //    Height = 600
            //};
            //window.ShowDialog();
        }
    }
}