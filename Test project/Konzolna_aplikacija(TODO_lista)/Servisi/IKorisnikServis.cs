using Konzolna_aplikacija_TODO_lista_.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    public interface IKorisnikServis
    {
        List<Korisnik> getKorisnici();
        void SpremiKorisnike(List<Korisnik> korisnici);
        void RegistrujKorisnik(Korisnik korisnik);
        void ObrisiSveIzJsonFajla();
        void AzurirajKorisnika(Korisnik korisnik);

    }
}
