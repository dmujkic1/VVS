using Konzolna_aplikacija_TODO_lista_.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    public class FiltriranjeZadatakaServis: IFiltriranjeZadatakaServis
    {
        public List<Zadatak> KategorijaFilter(List<Zadatak> zadaci, string kategorijaOpcija) {
            Kategorija kategorijaFilter = kategorijaOpcija == "1" ? Kategorija.LIČNI :
                                                  kategorijaOpcija == "2" ? Kategorija.POSLOVNI :
                                                  Kategorija.OBRAZOVNI;
            var filtriraniZadaci = zadaci.Where(z => z.kategorija == kategorijaFilter).ToList();
            return filtriraniZadaci;
        }

        public List<Zadatak> StatusFilter(List<Zadatak> zadaci, string statusOpcija)
        {
            Status statusFilter = statusOpcija == "1" ? Status.U_ČEKANJU :
                                          statusOpcija == "2" ? Status.U_TOKU :
                                          Status.ZAVRŠEN;
            var filtriraniZadaci = zadaci.Where(z => z.status == statusFilter).ToList();
            return filtriraniZadaci;
        }

        public List<Zadatak> PrioritetFilter(List<Zadatak> zadaci, string prioritetOpcija)
        {
            Prioritet prioritetFilter = prioritetOpcija == "1" ? Prioritet.NIZAK :
                                                prioritetOpcija == "2" ? Prioritet.SREDNJI :
                                                Prioritet.VISOK;
            var filtriraniZadaci = zadaci.Where(z => z.prioritet == prioritetFilter).ToList();
            return filtriraniZadaci;
        }


    }
}