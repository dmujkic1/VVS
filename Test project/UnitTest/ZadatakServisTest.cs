using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using Moq;
using Newtonsoft.Json;

namespace UnitTest
{
    public interface StubInterface
    {
        Korisnik korisnik();
    }

    public class Stub : StubInterface
    {
        public Korisnik korisnik()
        {
            return new Korisnik("Korisnik1", "test@example.com", "password1", new List<Zadatak>
        {
            new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.NIZAK, null, DateTime.Now.AddDays(3), null),
            new Zadatak("Zadatak 2", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.VISOK, null, DateTime.Now.AddDays(5), DateTime.Now.AddDays(6)),
            new Zadatak("Zadatak 3", Kategorija.OBRAZOVNI, Status.U_TOKU, Prioritet.SREDNJI, null, DateTime.Now.AddDays(7), null)
        },
            new List<Podsjetnik>());
        }
    }

    [TestClass]
    public class ZadatakServisTest
    {
        private ZadatakServis zadatakServis;
        private KorisnikServis korisnikServis;
        private Stub stub;
        private string _testFilePath;

        [TestInitialize]
        public void Setup()
        {
            // Inicijalizacija servisa i stubova
            zadatakServis = new ZadatakServis();
            korisnikServis = new KorisnikServis();
            stub = new Stub();

            // Privremeni JSON fajl za testove
            _testFilePath = Path.Combine(Path.GetTempPath(), "korisnici_test.json");
            File.WriteAllText(_testFilePath, "[]");

            // Postavljanje putanje fajla u instanci KorisnikServis
            typeof(KorisnikServis).GetField("putanjafilea",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(korisnikServis, _testFilePath);

            // Dodavanje korisnika iz stuba u JSON fajl
            var korisnik = stub.korisnik();
            korisnikServis.RegistrujKorisnik(korisnik);
        }
        [TestCleanup]
        public void Cleanup()
        {
            // Brisanje privremenog fajla nakon izvršavanja testova
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void ProsjecnoVrijemeIzvrsenjaTest_VracaTacno()
        {
            var zadaci = new List<Zadatak>
            {
                new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.NIZAK, DateTime.Now.AddHours(-1), DateTime.Now, DateTime.Now.AddHours(1)),
                new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.NIZAK, DateTime.Now.AddHours(-1), DateTime.Now, DateTime.Now.AddHours(1)),
                new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.NIZAK, DateTime.Now.AddHours(-1), DateTime.Now, DateTime.Now.AddHours(1))
            };
            var prosjecnoVrijemeIzvrsenja = zadatakServis.ProsjecnoVrijemeIzvrsenja(zadaci);
            Assert.AreEqual(120, Math.Round(prosjecnoVrijemeIzvrsenja));
        }

        [TestMethod]
        public void ProsjecnoVrijemeIzvrsenja_PraznaLista_VracaNula()
        {
            var zadaci = new List<Zadatak>();

            var prosjecnoVrijeme = zadatakServis.ProsjecnoVrijemeIzvrsenja(zadaci);

            Assert.AreEqual(0, prosjecnoVrijeme);
        }

        [TestMethod]
        public void ProsjecnoVrijemeIzvrsenja_NemaZavrsenih_VracaNula()
        {
            var zadaci = new List<Zadatak>
            {
                new Zadatak("Opis1", Kategorija.POSLOVNI, Status.U_ČEKANJU, Prioritet.VISOK, DateTime.Now, DateTime.Now.AddDays(1), null),
                new Zadatak("Opis2", Kategorija.LIČNI, Status.U_TOKU, Prioritet.SREDNJI, DateTime.Now, DateTime.Now.AddDays(1), null)
            };
            var prosjecnoVrijeme = zadatakServis.ProsjecnoVrijemeIzvrsenja(zadaci);

            Assert.AreEqual(0, prosjecnoVrijeme);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UnesiZadatak_ValidanZadatak_KorisnikNepostojeci_BazaIzuzetak()
        {
            var korisnik = new Korisnik("MujkaTriceps", "dmujkic1@etf.unsa.ba", "mojasifra", new List<Zadatak>(), new List<Podsjetnik>());
            var zadatak = new Zadatak(
                "Opis",
                Kategorija.LIČNI,
                Status.U_ČEKANJU,
                Prioritet.SREDNJI,
                null,
                DateTime.Now.AddDays(1),
                null
            );

            zadatakServis.UnesiZadatak(korisnik, zadatak);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnesiZadatak_RokZavrsetkaZadatkaIstekao_BacaIzuzetak()
        {
            var korisnik = stub.korisnik();
            var zadatak = new Zadatak("Prošlogodišnji zadatak", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.SREDNJI, null, DateTime.Now.AddDays(-1), null);

            // Poziv funkcije koja treba da baci izuzetak jer je rok prošao
            zadatakServis.UnesiZadatak(korisnik, zadatak);
        }

        [TestMethod]
        public void UnesiZadatak_DodavanjeDvaZadatka_PravilnoDodano()
        {
            // Kreiraj korisnika sa početnim podacima
            var korisnik = stub.korisnik();

            // Kreiraj zadatak koji želimo da dodamo korisniku
            var noviZadatak1 = new Zadatak(
                "Novi zadatak1",
                Kategorija.POSLOVNI,
                Status.U_ČEKANJU,
                Prioritet.SREDNJI,
                null,
                DateTime.Now.AddDays(5),
                null
            );
            var noviZadatak2 = new Zadatak(
                "Novi zadatak2",
                Kategorija.OBRAZOVNI,
                Status.U_ČEKANJU,
                Prioritet.VISOK,
                null,
                DateTime.Now.AddDays(2),
                null
            );

            // Dodajemo zadatak korisniku
            zadatakServis.UnesiZadatak(korisnik, noviZadatak1);
            zadatakServis.UnesiZadatak(korisnik, noviZadatak2);

            // Proveravamo da li je zadatak zaista dodat korisniku
            Assert.IsTrue(korisnik.toDoLista.Contains(noviZadatak1));
            Assert.IsTrue(korisnik.toDoLista.Contains(noviZadatak2));

            // Takođe provjeravamo da li je zadatak zaista dodat u listu
            Assert.AreEqual(korisnik.toDoLista.Count, 5);
            Assert.AreEqual(korisnik.toDoLista[0].opis, "Zadatak 1");
            Assert.AreEqual(korisnik.toDoLista[1].opis, "Zadatak 2");
        }

        [TestMethod]
        public void PretraziZadatke_ByOpis_ReturnsMatchingZadaci()
        {
            // Act
            var rezultat = zadatakServis.PretraziZadatke(KriterijPretrage.Opis, "Zadatak 1");

            // Assert
            Assert.AreEqual(1, rezultat.Count);
            Assert.AreEqual("Zadatak 1", rezultat[0].opis);
        }

        [TestMethod]
        public void PretraziZadatke_ByStatus_ReturnsMatchingZadaci()
        {
            // Act
            var rezultat = zadatakServis.PretraziZadatke(KriterijPretrage.Status, Status.ZAVRŠEN);

            // Assert
            Assert.AreEqual(1, rezultat.Count);
            Assert.AreEqual("Zadatak 2", rezultat[0].opis);
        }

        [TestMethod]
        public void PretraziZadatke_ByPrioritet_ReturnsMatchingZadaci()
        {
            // Act
            var rezultat = zadatakServis.PretraziZadatke(KriterijPretrage.Prioritet, Prioritet.SREDNJI);

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.AreEqual("Zadatak 3", rezultat[0].opis, "Naziv zadatka ne odgovara.");
        }

        [TestMethod]
        public void PretraziZadatke_ByKategorija_ReturnsMatchingZadaci()
        {
            // Act
            var rezultat = zadatakServis.PretraziZadatke(KriterijPretrage.Kategorija, Kategorija.LIČNI);

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.AreEqual("Zadatak 1", rezultat[0].opis);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PretraziZadatke_InvalidKriterij_ThrowsException()
        {
            // Act
            zadatakServis.PretraziZadatke((KriterijPretrage)999, null);
        }
    }
}