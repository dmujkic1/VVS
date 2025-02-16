using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolnaaplikacija_TODO_lista.Klase
{
    public class Izvještaj
    {
        public int UkupnoZadataka { get; set; }
        public int ZavrseniZadaci { get; set; }
        public int AktivniZadaci { get; set; }
        public int CekanjeZadaci { get; set; }
        public int OdlozeniZadaci { get; set; }
        public TimeSpan ProsječnoVrijemeZavršetka { get; set; }
    }
}
