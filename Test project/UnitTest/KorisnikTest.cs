using Konzolna_aplikacija_TODO_lista_.Klase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTest
{
    [TestClass]
    public class KorisnikTest
    {
        [TestMethod]
        public void Konstruktor_ValidniPodaci_ObjekatKreiran()
        {
            var toDoLista = new List<Zadatak>();
            var listaPodsjetnika = new List<Podsjetnik>();
            var korisnik = new Korisnik("TestUser", "test@example.com", "pass123", toDoLista, listaPodsjetnika);

            Assert.AreEqual("TestUser", korisnik.korisnickoIme);
            Assert.AreEqual("test@example.com", korisnik.email);
            Assert.AreEqual("pass123", korisnik.lozinka);
            Assert.AreEqual(toDoLista, korisnik.toDoLista);
            Assert.AreEqual(listaPodsjetnika, korisnik.listaPodsjetnika);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Konstruktor_NepotpuniPodaci_ThrowsException()
        {
            new Korisnik("", "test@example.com", "pass123", new List<Zadatak>(), new List<Podsjetnik>());
        }

        [TestMethod]
        public void DodajZadatak_ValidanZadatak_ZadatakDodan()
        {
            var korisnik = new Korisnik("TestUser", "test@example.com", "pass123", new List<Zadatak>(), new List<Podsjetnik>());
            var zadatak = new Zadatak("Test Zadatak", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.SREDNJI, null, DateTime.Now.AddDays(1), null);

            korisnik.dodajZadatak(zadatak);

            Assert.AreEqual(1, korisnik.toDoLista.Count);
            Assert.AreEqual(zadatak, korisnik.toDoLista[0]);
        }

        [TestMethod]
        public void DodajPodsjetnik_ValidanPodsjetnik_PodsjetnikDodan()
        {
            var korisnik = new Korisnik("TestUser", "test@example.com", "pass123", new List<Zadatak>(), new List<Podsjetnik>());
            var zadatak = new Zadatak("Test Zadatak", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.SREDNJI, null, DateTime.Now.AddDays(1), null);
            var podsjetnik = new Podsjetnik(DateTime.Now.AddHours(1), zadatak, false, true);

            korisnik.dodajPodsjetnik(podsjetnik);

            Assert.AreEqual(1, korisnik.listaPodsjetnika.Count);
            Assert.AreEqual(podsjetnik, korisnik.listaPodsjetnika[0]);
        }

        [TestMethod]
        public void ProvjeriRokoveZadataka_ZadaciSPrekoracenimRokom_BrojacPovecava()
        {
            var zadatak1 = new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.SREDNJI, null, DateTime.Now.AddDays(1), null);
            var zadatak2 = new Zadatak("Zadatak 2", Kategorija.LIČNI, Status.U_TOKU, Prioritet.VISOK, null, DateTime.Now.AddDays(1), null);

            zadatak1.rokZavrsetka = DateTime.Now.AddDays(-1);

            var korisnik = new Korisnik("TestUser", "test@example.com", "pass123", new List<Zadatak> { zadatak1, zadatak2 }, new List<Podsjetnik>());

            var brojPrekoracenih = korisnik.provjeriRokoveZadataka();

            Assert.AreEqual(1, brojPrekoracenih);
            Assert.AreEqual(Status.ODLOŽEN, zadatak1.status);
            Assert.AreEqual(Status.U_TOKU, zadatak2.status);
        }


        [TestMethod]
        public void ToString_VracanjeStringa_IspravanFormat()
        {
            var korisnik = new Korisnik("TestUser", "test@example.com", "pass123", new List<Zadatak>(), new List<Podsjetnik>());
            var rezultat = korisnik.toString();

            Assert.AreEqual("Korisnicko ime: 'TestUser', lozinka: 'pass123' i email: 'test@example.com", rezultat);
        }

        [TestMethod]
        [DataRow("", "test@example.com", "password", "Mora biti uneseno korisnicko ime!")]
        [DataRow("abc", "test@example.com", "password", "Korisnicko ime mora imat minimalno 5 simbola")]
        [DataRow("ValidUser", "invalid-email", "password", "Mora biti unesen email u ispravnom formatu!")]
        [DataRow("ValidUser", "test@example.com", "", "Mora biti unesena lozinka")]
        [DataRow("ValidUser", "test@example.com", "1234", "Lozinka mora imat minimalno 5 simbola")]
        [DataRow("ValidUser", "valid.email@example.com", "ValidPass", null)] // Validan slucaj - nema izuzetka
        public void ValidacijaPodataka_TestRazliciteSituacije(string korisnickoIme, string email, string lozinka, string ocekivanaPoruka)
        {
            // Arrange
            var korisnik = new Korisnik("ValidUser", "valid@example.com", "ValidPass", new List<Zadatak>(), new List<Podsjetnik>()); // valid korisnik za instancu

            try
            {
                // Act
                typeof(Korisnik)
                    .GetMethod("ValidacijaPodataka", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(korisnik, new object[] { korisnickoIme, email, lozinka });

                Assert.IsNull(ocekivanaPoruka, "Očekivana je greška, ali nije došlo do izuzetka.");
            }
            catch (TargetInvocationException ex)
            {
                // Assert
                if (ex.InnerException is ArgumentException argEx)
                {
                    Assert.AreEqual(ocekivanaPoruka, argEx.Message, "Poruka izuzetka nije ispravna.");
                }
                else
                {
                    Assert.Fail($"Došlo je do neočekivanog izuzetka: {ex.InnerException?.Message}");
                }
            }
        }

    }
}