using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTMetoda.Model
{
    internal class Alternativa
    {
        public string Naziv { get; set; }

        public Alternativa()
        {
        }

        public Alternativa(string naziv)
        {
            Naziv = naziv;
        }
    }
}
