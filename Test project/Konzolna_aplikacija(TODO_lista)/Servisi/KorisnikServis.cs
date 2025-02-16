using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Newtonsoft.Json;

namespace Konzolna_aplikacija_TODO_lista_.Servisi
{
    public class KorisnikServis: IKorisnikServis
    {
        private readonly string putanjafilea = Path.Combine(Environment.CurrentDirectory, "Podaci", "korisnici.json");

        public List<Korisnik> getKorisnici()
        {
            try
            {
                if (File.Exists(putanjafilea))
                {
                    var json = File.ReadAllText(putanjafilea);
                    return JsonConvert.DeserializeObject<List<Korisnik>>(json) ?? new List<Korisnik>();
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Greška pri deserijalizaciji JSON-a: {ex.Message}");
                return new List<Korisnik>(); // U slučaju greške, kreiraj praznu listu
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Greška pri čitanju fajla: {ex.Message}");
                return new List<Korisnik>(); // U slučaju greške, kreiraj praznu listu
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nepredviđena greška: {ex.Message}");
                return new List<Korisnik>(); // U slučaju nepredviđene greške, kreiraj praznu listu
            }
            return new List<Korisnik>();
        }

        public void SpremiKorisnike(List<Korisnik> korisnici)
        {
            try
            {
                var json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
                File.WriteAllText(putanjafilea, json);
            }catch(Exception ex)
            {
                Console.WriteLine("Greska pri upisu u fajl: ", ex);
            }
        }

        private String Enkripcija(String lozinka)
        {
            //lozinka = lozinka.Trim();
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
            return Convert.ToBase64String(bytes);
        }

        public void RegistrujKorisnik(Korisnik korisnik)
        {
            var korisnici=getKorisnici();
            korisnik.lozinka = Enkripcija(korisnik.lozinka);
            korisnici.Add(korisnik);
            SpremiKorisnike(korisnici);
        }

        public void ObrisiSveIzJsonFajla()
        {
            File.WriteAllText(putanjafilea, "[]");
        }

        public void AzurirajKorisnika(Korisnik korisnik)
        {
            var korisnici = getKorisnici();
            var starikorisnik= korisnici.FirstOrDefault(u => u.korisnickoIme == korisnik.korisnickoIme);
            if (starikorisnik != null)
            {
                starikorisnik.listaPodsjetnika = korisnik.listaPodsjetnika;
                starikorisnik.toDoLista = korisnik.toDoLista;

                SpremiKorisnike(korisnici);
            }else
            {
               throw new Exception("Ne postoji taj korisnik");
            }
        }

        public void VecPostojiKorisnik(String korisnickoImetemp)
        {
            var korisnici = getKorisnici();
            foreach(var korisnik in korisnici)
            {
                if (korisnik.korisnickoIme == korisnickoImetemp)
                    throw new ArgumentException("Vec postoji korisnik s tim korisnickim imenom!");

            }
        }

        public string GenerisiStatistikuKorisnika(List<Korisnik> korisnici)
        {
            if (korisnici == null || korisnici.Count == 0)
                throw new ArgumentException("Lista korisnika ne može biti null ili prazna.");

            var detaljiPoKorisniku = new Dictionary<string, (int ukupnoZadataka, double zavrseniZadaci, int brojPodsjetnika, double izvrseniPodsjetnici)>();
            string korisnikSaNajviseZadataka = "";
            int maxZadataka = 0;
            var neaktivniKorisnici = new List<string>();

            for (int i = 0; i < korisnici.Count; i++)
            {
                var korisnik = korisnici[i];
                int ukupnoZadataka = korisnik.toDoLista.Count, zavrseniZadaci = 0;
                for (int j = 0; j < korisnik.toDoLista.Count; j++)
                {
                    if (korisnik.toDoLista[j].status == Status.ZAVRŠEN)
                        zavrseniZadaci++;
                }

                int ukupnoPodsjetnika = korisnik.listaPodsjetnika.Count, zavrseniPodsjetnici = 0;
                for (int j = 0; j < korisnik.listaPodsjetnika.Count; j++)
                {
                    if (korisnik.listaPodsjetnika[j].izvrsen)
                        zavrseniPodsjetnici++;
                }

                double zavrseniProcenat = 0, zavrseniProcenatPodsjetnik = 0;
                if (ukupnoZadataka > 0)
                    zavrseniProcenat = Math.Round((double)zavrseniZadaci / ukupnoZadataka * 100, 2);
                else
                    zavrseniProcenat = 0;

                if (ukupnoPodsjetnika > 0)
                    zavrseniProcenatPodsjetnik = Math.Round((double)zavrseniPodsjetnici / ukupnoPodsjetnika * 100, 2);
                else
                    zavrseniProcenatPodsjetnik = 0;

                detaljiPoKorisniku[korisnik.korisnickoIme] = (ukupnoZadataka, zavrseniProcenat, ukupnoPodsjetnika, zavrseniProcenatPodsjetnik);

                if (ukupnoZadataka > maxZadataka)
                {
                    maxZadataka = ukupnoZadataka;
                    korisnikSaNajviseZadataka = korisnik.korisnickoIme;
                }

                if (ukupnoZadataka == 0 && ukupnoPodsjetnika == 0)
                    neaktivniKorisnici.Add(korisnik.korisnickoIme);
            }

            return generisiRezultatStatistike(detaljiPoKorisniku, korisnikSaNajviseZadataka, neaktivniKorisnici);
        }

        private String generisiRezultatStatistike(Dictionary<string, (int ukupnoZadataka, double zavrseniZadaci, int brojPodsjetnika, double izvrseniPodsjetnici)> detaljiPoKorisniku, string korisnikSaNajviseZadataka, List<string> neaktivniKorisnici) {
            var rezultat = new StringBuilder();
            foreach (var korisnik in detaljiPoKorisniku)
                rezultat.AppendLine($"{korisnik.Key}:" +
                    $"\n  Ukupno zadataka: {korisnik.Value.ukupnoZadataka}" +
                    $"\n  Zavrsenih zadataka: {korisnik.Value.zavrseniZadaci}%" +
                    $"\n  Ukupno podsjetnika: {korisnik.Value.brojPodsjetnika}" +
                    $"\n  Izvrseni podsjetnici: {korisnik.Value.izvrseniPodsjetnici}%");

            rezultat.AppendLine($"Korisnik s najviše zadataka: {korisnikSaNajviseZadataka}");

            if (neaktivniKorisnici.Count > 0)
            {
                rezultat.AppendLine("Neaktivni korisnici:");
                rezultat.AppendLine(string.Join("\n", neaktivniKorisnici.Select(n => $"- {n}")));
            }
            return rezultat.ToString();
        }

    }
}
