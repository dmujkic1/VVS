using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Klase
{
    public class Zadatak
    {
        public String opis {  get; set; }
        public Kategorija kategorija {  get; set; }
        public Status status { get; set; }
        public Prioritet prioritet { get; set; }
        public DateTime? vrijemePocetka { get; set; } //moze bit null dok neko ga ne zapocne
        public DateTime rokZavrsetka { get; set; }
        public DateTime? vrijemeZavrsetka { get; set; } //moze bit null dok ne oznaci da je zavrsen

        public Zadatak(string opis, Kategorija kategorija, Status status, Prioritet prioritet, DateTime? vrijemePocetka, DateTime rokZavrsetka, DateTime? vrijemeZavrsetka)
        {
            validacijaPodataka(opis, rokZavrsetka,status);
            this.opis = opis;
            this.kategorija = kategorija;
            this.status = status;
            this.prioritet = prioritet;
            this.vrijemePocetka = vrijemePocetka;
            this.rokZavrsetka = rokZavrsetka;
            this.vrijemeZavrsetka = vrijemeZavrsetka;
        }

        public void ZapocniZadatak()
        {
            if (status == Status.U_ČEKANJU)
            {
                this.status = Status.U_TOKU;
                vrijemePocetka = DateTime.Now;
            }else if(status==Status.U_TOKU)
            {
                throw new ArgumentException("Zadatak je već započet");
            }else if (status == Status.ZAVRŠEN)
            {
                throw new ArgumentException("Zadatak je već završen");
            }else
            {
                Console.WriteLine("Zadatak je bio odložen ali je sada aktivan");
                this.status = Status.U_TOKU;
                vrijemePocetka = DateTime.Now;
            }
        }

        public void ZavrsiZadatak()
        {
            if (status == Status.U_TOKU)
            {
                this.status = Status.ZAVRŠEN;
                vrijemeZavrsetka = DateTime.Now;
            }else if (status == Status.ZAVRŠEN)
            {
                throw new Exception("Zadatak je već prije završen");
            }else if (status == Status.U_ČEKANJU)
            {
                throw new Exception("Zadatak nije bio započet");
            }else
            {
                throw new Exception("Zadatak je odložen, započnite ga opet"); //kad je odlozen
            }
        }

        
        private void validacijaPodataka(String opis, DateTime rokZavrsetka,Status status)
        {
            if (String.IsNullOrWhiteSpace(opis)) throw new ArgumentException("Opis ne smije biti prazan!");
            if (rokZavrsetka < DateTime.Now && status==Status.U_ČEKANJU) throw new ArgumentException("Rok završetka mora biti u budućnosti!");
            //kad se tek pravi tad je u cekanju i tad se samo treba vrsit validacija kad se tek praviinstanca
        }

        public Boolean ProvjeriRok()
        {
            var ima_promjene = false;
            if(status!=Status.ZAVRŠEN && status!=Status.ODLOŽEN && DateTime.Now>rokZavrsetka)
            {
                ima_promjene = true;
                status = Status.ODLOŽEN;
                //Console.WriteLine($"Zadatak '{opis}' je automatski postavljen na ODLOŽEN jer je prošao rok završetka!");
            }
            return ima_promjene;
        }
    }
}
