using Konzolna_aplikacija_TODO_lista_.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    internal interface IFiltriranjeZadatakaServis
    {
        public List<Zadatak> KategorijaFilter(List<Zadatak> zadaci, string kategorijaOpcija);
        public List<Zadatak> StatusFilter(List<Zadatak> zadaci, string statusOpcija);
        public List<Zadatak> PrioritetFilter(List<Zadatak> zadaci, string prioritetOpcija);
    }
}
