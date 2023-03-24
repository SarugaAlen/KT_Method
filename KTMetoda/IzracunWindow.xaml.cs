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
    /// Interaction logic for IzracunWindow.xaml
    /// </summary>
    /// 
    public partial class IzracunWindow : Window
    {
        public ObservableCollection<string> Alternative;
        public ObservableCollection<Dictionary<string, int>> Parametri;

        public IzracunWindow(ObservableCollection<string> Alternative, ObservableCollection<Dictionary<string, int>> Parametri)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            this.Parametri = Parametri;

            //DataGridUtezi.ItemsSource = Parametri;
            var parametriColumn = new DataGridTemplateColumn
            {
                Header = "Parametri",
                CellTemplate = new DataTemplate(typeof(ListBox))
            };

            //var itemTemplate = new DataTemplate();

            //var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            //stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //var keyTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            //keyTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Key"));
            //stackPanelFactory.AppendChild(keyTextBlockFactory);

            //var valueTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            //valueTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Value"));
            //stackPanelFactory.AppendChild(valueTextBlockFactory);

            //itemTemplate.VisualTree = stackPanelFactory;

            //var listBoxFactory = new FrameworkElementFactory(typeof(ListBox));
            //listBoxFactory.SetValue(ListBox.ItemTemplateProperty, itemTemplate);
            //listBoxFactory.SetBinding(ListBox.ItemsSourceProperty, new Binding());

            //parametriColumn.CellTemplate = new DataTemplate();
            //parametriColumn.CellTemplate.VisualTree = listBoxFactory;

            //parametriColumn.SetValue(DataGrid.ItemsSourceProperty, Parametri);

            //myDataGrid.Columns.Insert(0, parametriColumn);

            foreach (string alternative in Alternative)
            {
                var column = new DataGridTextColumn
                {
                    Header = alternative,
                    Binding = new Binding(string.Format("[{0}]", alternative))
                };
                myDataGrid.Columns.Add(column);
            }
        }
    }
}
