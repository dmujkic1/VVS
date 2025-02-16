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
    public class AutentifikacijaServis: IAutentifikacijaServis
    {
        private string putanjafilea = Path.Combine(Environment.CurrentDirectory, "Podaci", "korisnici.json");
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
        public Korisnik Prijava(string korisnickoIme, string lozinka)
        {
            if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                return null;

            var korisnici = getKorisnici();

            Korisnik korisnik = null;

            // Use a loop to find the user instead of LINQ
            foreach (var u in korisnici)
            {
                if (u.korisnickoIme == korisnickoIme)
                {
                    korisnik = u;
                    break;
                }
            }
    
            // Check if the user was found and validate the password
            if (korisnik != null)
            {
                if (korisnik.lozinka == Enkripcija(lozinka))
                {
                    return korisnik; // Successful login
                }
                else
                {
                    return null; // Incorrect password
                }
            }
            else
            {
                return null; // User not found
            }
        }

        public String Enkripcija(String lozinka)
        {
            //lozinka = lozinka.Trim();
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
            return Convert.ToBase64String(bytes);
        }
    }
}
