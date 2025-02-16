using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using Konzolnaaplikacija_TODO_lista.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Konzolnaaplikacija_TODO_lista.Servisi
{
    public class AnalitikaServis : IAnalitikaServis
    {
        public Izvještaj GenerisiIzvještaj(List<Zadatak> zadaci)
        {
            int ukupno = zadaci.Count;
            int zavrseni = 0;
            int aktivni = 0;
            int cekanje = 0;
            int odlozeni = 0;
            TimeSpan prosjecnoVrijeme = TimeSpan.Zero;

            for (int i = 0; i < zadaci.Count; i++)
            {
                if (zadaci[i].status == Status.ZAVRŠEN) zavrseni++;
                else if (zadaci[i].status == Status.U_TOKU) aktivni++;
                else if (zadaci[i].status == Status.U_ČEKANJU) cekanje++;
                else if (zadaci[i].status == Status.ODLOŽEN) odlozeni++;
            }

            if (zavrseni > 0)
            {
                prosjecnoVrijeme = TimeSpan.FromMinutes(
                    zadaci.Where(z => z.status == Status.ZAVRŠEN)
                          .Average(z => (z.vrijemeZavrsetka.Value - z.vrijemePocetka.Value).TotalMinutes)
                );
            }

            return new Izvještaj
            {
                UkupnoZadataka = ukupno,
                ZavrseniZadaci = zavrseni,
                AktivniZadaci = aktivni,
                CekanjeZadaci = cekanje,
                OdlozeniZadaci = odlozeni,
                ProsječnoVrijemeZavršetka = prosjecnoVrijeme
            };
        }


    }
}