using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KTMetoda.Model
{
    class Izracun : INotifyPropertyChanged
    {
        string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    OnPropertyChanged(nameof(Ime));
                }
            }
        }

        ObservableCollection<Parameter> parametri;
        public ObservableCollection<Parameter> Parametri
        {
            get { return parametri; }
            set
            {
                if (parametri != value)
                {
                    parametri = value;
                    OnPropertyChanged(nameof(Parametri));
                }
            }
        }

        int[] vnosi;
        public int[] Vnosi
        {
            get { return vnosi; }
            set
            {
                if (vnosi != value)
                {
                    vnosi = value;
                    OnPropertyChanged(nameof(Vnosi));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Izracun()
        {
        }
        public Izracun(string ime, ObservableCollection<Parameter> parametri, int[] vnosi)
        {
            Ime = ime;
            Parametri = parametri;
            Vnosi = vnosi;
        }
    }
}
