using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTMetoda.Model
{
    internal class Sestevek
    {
        public string Ime { get; set; }
        public int Vrednost { get; set; }

        public Sestevek()
        {

        }
        public Sestevek(string ime, int vrednost)
        {
            Ime = ime;
            Vrednost = vrednost;
        }
    }
}
