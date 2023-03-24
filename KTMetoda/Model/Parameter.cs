using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTMetoda.Model
{
    public class Parameter : INotifyPropertyChanged
    {
        private string ime;
        private int utez;

        public string Ime {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged("Ime");
            }
        }
        public int Utez {
            get { return utez; }
            set
            {
                utez = value;
                OnPropertyChanged("Utez");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Parameter()
        {
        }
        public Parameter(string ime, int utez)
        {
            Ime = ime;
            Utez = utez;
        }

    }
}
