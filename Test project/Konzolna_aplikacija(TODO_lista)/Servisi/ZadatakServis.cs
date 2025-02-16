using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    public class ZadatakServis: IZadatakServis
    {
        public void UnesiZadatak(Korisnik korisnik, Zadatak zadatak)
        {
            korisnik.dodajZadatak(zadatak);
            var korisnikServis = new KorisnikServis();
            korisnikServis.AzurirajKorisnika(korisnik);
        }
        public double ProsjecnoVrijemeIzvrsenja(List<Zadatak> zadaci)
        {
            // Prosječno trajanje izvršenja završenih zadataka
            var zavrseniZadaci = new List<Zadatak>();
            for(int i=0; i<zadaci.Count; i++)
            {
                if (zadaci[i].status==Status.ZAVRŠEN && zadaci[i].vrijemeZavrsetka!=null && zadaci[i].vrijemePocetka != null)
                {
                    zavrseniZadaci.Add(zadaci[i]);
                }
            }

            TimeSpan ukupnoVrijeme = TimeSpan.Zero;
            for(int i=0; i<zavrseniZadaci.Count ; i++)
            {
                ukupnoVrijeme += zavrseniZadaci[i].vrijemeZavrsetka.Value - zavrseniZadaci[i].vrijemePocetka.Value;
            }

            double prosjecnoVrijemeIzvrsenja = 0;

            if (zavrseniZadaci.Count > 0)
            {
                prosjecnoVrijemeIzvrsenja = ukupnoVrijeme.TotalMinutes / zavrseniZadaci.Count;
            }

            return prosjecnoVrijemeIzvrsenja;
        }

        public List<Zadatak> PretraziZadatke(KriterijPretrage kriterij, object vrijednost)
        {
            var korisnikServis = new KorisnikServis();
            var korisnici = korisnikServis.getKorisnici();
            var sviZadaci = korisnici.SelectMany(k => k.toDoLista).ToList();

            return kriterij switch
            {
                KriterijPretrage.Opis => sviZadaci
                    .Where(z => z.opis != null && z.opis.IndexOf(vrijednost.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList(),

                KriterijPretrage.Status => sviZadaci
                    .Where(z => z.status == (Status)vrijednost)
                    .ToList(),

                KriterijPretrage.Prioritet => sviZadaci
                    .Where(z => z.prioritet == (Prioritet)vrijednost)
                    .ToList(),

                KriterijPretrage.Kategorija => sviZadaci
                    .Where(z => z.kategorija == (Kategorija)vrijednost)
                    .ToList(),

                _ => throw new ArgumentException("Nepoznat kriterij pretrage.")
            };
        }
    }

}
