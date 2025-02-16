using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public interface SpyInterface
    {
        Korisnik korisnik(int broj);
    }

    public class Spy : SpyInterface
    {
        private Dictionary<int, Korisnik> korisnici;

        public Spy()
        {
            korisnici = new Dictionary<int, Korisnik>
        {
            { 0, new Korisnik("test1korisnik", "test1@gmail.com", "lozinka1", new List<Zadatak>(), new List<Podsjetnik>()) },
            { 1, new Korisnik("test2korisnik", "test2@gmail.com", "lozinka2", new List<Zadatak>(), new List<Podsjetnik>()) },
            { 2, new Korisnik("testUser1", "test1@gmail.com", "lozinka1", new List<Zadatak>(), new List<Podsjetnik>()) },
            { 3, new Korisnik("newUser", "new@gmail.com", "lozinka", new List<Zadatak>(), new List<Podsjetnik>()) }
        };
        }

        public Korisnik korisnik(int broj)
        {
            return korisnici.ContainsKey(broj) ? korisnici[broj] : new Korisnik("nepostojeci", "nepostojeci@gmail.com", "lozinka", new List<Zadatak>(), new List<Podsjetnik>());
        }
    }

    [TestClass]
    public class KorisnikServisTest
    {


        private KorisnikServis _korisnikServis;
        private string _testFilePath;
        private Spy spy;

        [TestInitialize]
        public void Setup()
        {
            spy = new Spy();
            //privremeni fajl za testove
            _testFilePath = Path.Combine(Path.GetTempPath(), "korisnici_test.json");
            File.WriteAllText(_testFilePath, "[]");
            _korisnikServis = new KorisnikServis();

            // privremeni fajl kao putanju fajla u instanci KorisnikServis
            typeof(KorisnikServis).GetField("putanjafilea",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_korisnikServis, _testFilePath);
        }

        [TestMethod]
        public void GetKorisnici_PostojiFile_VracaListuKorisnika()
        {
            var expectedKorisnici = new List<Korisnik>
            {
                spy.korisnik(0),spy.korisnik(1)
            };
            var json = JsonConvert.SerializeObject(expectedKorisnici);
            File.WriteAllText(_testFilePath, json);

            var result = _korisnikServis.getKorisnici();

            Assert.AreEqual(expectedKorisnici.Count, result.Count);
            Assert.AreEqual(expectedKorisnici[0].korisnickoIme, result[0].korisnickoIme);
            Assert.AreEqual(expectedKorisnici[1].korisnickoIme, result[1].korisnickoIme);
        }

        [TestMethod] 
        public void GetKorisnici_FileNePostoji_VracaPraznuListu()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }

            var result = _korisnikServis.getKorisnici();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetKorisnici_NeispravanJSon_VracaPraznuListu()
        {
            var invalidJson = "["; // Nedostaje znak ]
            File.WriteAllText(_testFilePath, invalidJson);

            var result = _korisnikServis.getKorisnici();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetKorisnici_GreskaPrilikomCitanja_VracaPraznuListu()
        {
            // kreiranje fajla i zakljucavanje
            using (var fileStream = new FileStream(_testFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                // Pokušaj čitanja dok je fajl zaključan
                var result = _korisnikServis.getKorisnici();
                Assert.AreEqual(0, result.Count); // Metoda treba vratiti praznu listu
            }
        }

        [TestMethod]
        public void GetKorisnici_NepredvidjenaGreska_VracaPraznuListu()
        {
            var incompatibleEncoding = Encoding.Unicode; // Koristimo Unicode (UTF-16)
            File.WriteAllText(_testFilePath, "[]", incompatibleEncoding);

            var result = _korisnikServis.getKorisnici();
            Assert.AreEqual(0, result.Count); 
        }


        [TestMethod]
        public void SpremiKorisnike_UspjesnoSpasava()
        {
            var korisnici = new List<Korisnik>
            {
                spy.korisnik(2)
            };

            _korisnikServis.SpremiKorisnike(korisnici);

            var fileContent = File.ReadAllText(_testFilePath);
            Assert.IsTrue(fileContent.Contains("testUser1"));
        }

        [TestMethod]
        public void SpremiKorisnike_FajlZakljucan_NeSpasavaKorisnika()
        {
            using (var fileStream = new FileStream(_testFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                var korisnici = new List<Korisnik> { spy.korisnik(0) };
                // Pokušaj čitanja dok je fajl zaključan
                _korisnikServis.SpremiKorisnike(korisnici);
            }
            var sadrzaj = File.ReadAllText(_testFilePath);
            Assert.IsFalse(sadrzaj.Contains("test1korisnik"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VecPostojiKorisnik_KorisnikVecPostoji_BazaIzuzetak()
        {
            // Arrange
            var existingUser = "testUser1";
            var korisnici = new List<Korisnik>
            {
                spy.korisnik(2)
            };
            var json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(_testFilePath, json); // Ručno unosimo korisnika u fajl

            _korisnikServis.VecPostojiKorisnik(existingUser);
        }

        [TestMethod]
        public void VecPostojiKorisnik_KorisnikNePostoji_ProlaziBezIzuzetka()
        {
            var nepostojeci = "nonexistent";
            var korisnici = new List<Korisnik>
            {
                spy.korisnik(2)
            };
            var json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(_testFilePath, json); // Ručno unosimo korisnika u fajl

            _korisnikServis.VecPostojiKorisnik(nepostojeci);
        }

        [TestMethod]
        public void RegistrujKorisnik_DodajeKorisnikaUListu()
        {
            var korisnik = spy.korisnik(3);

            var sadrzajPrije = File.ReadAllText(_testFilePath);
            var korisniciPrije = JsonConvert.DeserializeObject<List<Korisnik>>(sadrzajPrije);

            _korisnikServis.RegistrujKorisnik(korisnik);

            var sadrzajNakon = File.ReadAllText(_testFilePath);
            var korisniciNakon = JsonConvert.DeserializeObject<List<Korisnik>>(sadrzajNakon);

            // provjera jel porastao za 1 
            Assert.AreEqual(korisniciPrije.Count + 1, korisniciNakon.Count);
            Assert.IsTrue(korisniciNakon.Any(k => k.korisnickoIme == korisnik.korisnickoIme));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AzurirajKorisnika_KorisnikNePostoji_BacaIzuzetak()
        {
            var korisnik = spy.korisnik(5);

            _korisnikServis.AzurirajKorisnika(korisnik);
        }

        [TestMethod]
        public void AzurirajKorisnika_KorisnikPostoji_AzuriraPodatke()
        {
            var korisnici = new List<Korisnik>
            {
                spy.korisnik(2) //testUser1 
            };
            var json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(_testFilePath, json);

            //mijenjanje liste
            korisnici[0].listaPodsjetnika = new List<Podsjetnik> { new Podsjetnik(DateTime.Now.AddDays(1), null, false) };
            _korisnikServis.AzurirajKorisnika(spy.korisnik(2));

            var sadrzajNakon = File.ReadAllText(_testFilePath);
            var korisniciNakon = JsonConvert.DeserializeObject<List<Korisnik>>(sadrzajNakon);
            Assert.IsTrue(korisniciNakon.Any(korisnik => korisnik.listaPodsjetnika.Any()));
        }

        [TestMethod]
        public void TestObrisiSveIzJsonFajla_BriseSve()
        {
            var korisnici = new List<Korisnik>
            {
                spy.korisnik(2)
            };
            var json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(_testFilePath, json);

            _korisnikServis.ObrisiSveIzJsonFajla();

            var fileContent = File.ReadAllText(_testFilePath);
            Assert.AreEqual("[]", fileContent);
        }

        [TestMethod]
        public void Enkripcija_ValidnaLozinka_VracaEnkriptovanuVrijednost() //privatna funkcija koja se poziva iz registruj
        {
            var korisnik = spy.korisnik(2);
            _korisnikServis.RegistrujKorisnik(korisnik);

            var sadrzaj = File.ReadAllText(_testFilePath);
            var korisnici= JsonConvert.DeserializeObject<List<Korisnik>>(sadrzaj);
            var spaseniKorisnik=korisnici.FirstOrDefault(k => k.korisnickoIme == "testUser1");

            Assert.IsNotNull(spaseniKorisnik);
            Assert.AreNotEqual("lozinka1", spaseniKorisnik.lozinka);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // ciscenje nakon svakog testa
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }



    }
}
