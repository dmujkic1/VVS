using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Klase
{
    public class Korisnik
    {
        public String korisnickoIme { get; set; }
        public String email { get; set; }
        public String lozinka { get; set; }
        public List <Zadatak> toDoLista { get; set; }
        public List<Podsjetnik> listaPodsjetnika { get; set; }
        public Korisnik(string korisnickoIme, string email, string lozinka,List<Zadatak> toDoLista,List<Podsjetnik> listaPodsjetnika)
        {
            ValidacijaPodataka(korisnickoIme, email, lozinka);
            this.korisnickoIme = korisnickoIme;
            this.email = email;
            this.lozinka = lozinka;
            this.toDoLista = toDoLista;
            this.listaPodsjetnika= listaPodsjetnika;
        }
       
        private void ValidacijaPodataka(String korisnickoIme, String email, String lozinka)
        {
            var emailFormat = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrEmpty(korisnickoIme))
            {
                throw new ArgumentException("Mora biti uneseno korisnicko ime!");
            }
            if(korisnickoIme.Length < 5)
            {
                throw new ArgumentException("Korisnicko ime mora imat minimalno 5 simbola");
            }
            if (!emailFormat.IsMatch(email))
            {
                throw new ArgumentException("Mora biti unesen email u ispravnom formatu!");
            }
            if(string.IsNullOrWhiteSpace(lozinka) || string.IsNullOrEmpty(lozinka))
            {
                throw new ArgumentException("Mora biti unesena lozinka");
            }
            if (lozinka.Length < 5)
            {
                throw new ArgumentException("Lozinka mora imat minimalno 5 simbola");
            }
        }
        public String toString()
        {
            var tekst=($"Korisnicko ime: '{korisnickoIme}', lozinka: '{lozinka}' i email: '{email}");
            return tekst;
        }

        public void dodajZadatak(Zadatak zadatak)
        {
            toDoLista.Add(zadatak);
        }

        public void dodajPodsjetnik(Podsjetnik podsjetnik)
        {
            listaPodsjetnika.Add(podsjetnik);
        }

        public int provjeriRokoveZadataka()
        {
            int brojac = 0;
            foreach(var zadatak in toDoLista)
            {
                if(zadatak.ProvjeriRok())brojac++;
            }
            return brojac;
        }
    }
}