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
    public partial class IzracunWindow : Window
    {
        public ObservableCollection<string> Alternative;
        public ObservableCollection<Dictionary<string, int>> Parametri;

        public IzracunWindow(ObservableCollection<string> Alternative, ObservableCollection<Dictionary<string, int>> Parametri)
        {
            InitializeComponent();
            this.Alternative = Alternative;
            this.Parametri = Parametri;
        }
    }
}
