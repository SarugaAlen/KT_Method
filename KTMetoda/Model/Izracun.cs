using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTMetoda.Model
{
    class Izracun
    {
        public string Alternativa { get; set; }

        public List<string> Parametri { get; set; }
        public List<int> Vrednost { get; set; }

        public Izracun(string alternativa, List<string> parametri, List<int> vrednost)
        {
            Alternativa = alternativa;
            Parametri = parametri;
            Vrednost = vrednost;
        }
    }
}
