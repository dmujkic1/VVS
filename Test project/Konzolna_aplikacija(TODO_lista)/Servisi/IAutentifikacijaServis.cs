using Konzolna_aplikacija_TODO_lista_.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    public interface IAutentifikacijaServis
    {
        public List<Korisnik> getKorisnici();
        Korisnik Prijava(string korisnickoIme, String lozinka);
        public String Enkripcija(String lozinka);
    }
}
