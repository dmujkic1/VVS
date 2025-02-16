using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolnaaplikacija_TODO_lista.Klase;

namespace UnitTest
{

    [TestClass]
    public class ZadatakTest
    {

        [TestMethod]
        [DataRow("", "2024-12-01", typeof(ArgumentException))] // Prazan opis
        [DataRow("Opis", "2020-01-01", typeof(ArgumentException))] // Rok u prošlosti
        [DataRow("Opis", "2024-12-01", null)] // Ispravno
        public void Konstruktor_ValidacijaPodataka_ViseScenarija(string opis, string rokZavrsetkaStr, Type expectedException)
        {
            DateTime rokZavrsetka = DateTime.Parse(rokZavrsetkaStr);

            if (expectedException != null)
            {
                var exception = Assert.ThrowsException<ArgumentException>(() =>
                {
                    var zadatak = new Zadatak(opis, Kategorija.POSLOVNI, Status.U_ČEKANJU, Prioritet.NIZAK, null, rokZavrsetka, null);
                });

                Assert.IsInstanceOfType(exception, expectedException);
            }
            else
            {
                
                var zadatak = new Zadatak(opis, Kategorija.POSLOVNI, Status.U_ČEKANJU, Prioritet.SREDNJI, null, rokZavrsetka, null);
                Assert.IsNotNull(zadatak);
            }
        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                yield return new object[] { "Opis", Status.U_ČEKANJU, Status.U_TOKU, null, true }; // U čekanju - pokreće zadatak
                yield return new object[] { "Opis", Status.ODLOŽEN, Status.U_TOKU, null, true }; //bio odlozen pa se pokrene
                yield return new object[] { "Opis", Status.U_TOKU, Status.U_TOKU, typeof(ArgumentException), false }; // U toku - izuzetak
                yield return new object[] { "Opis", Status.ZAVRŠEN, Status.ZAVRŠEN, typeof(ArgumentException), false }; // Završen - izuzetak
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestData), DynamicDataSourceType.Property)]
        public void ZapocniZadatak_RazlicitiScenariji(string opis, Status trenutniStatus, Status ocekivaniStatus, Type expectedException, bool expectSuccess)
        {
            var zadatak = new Zadatak(
                opis,
                Kategorija.LIČNI,
                trenutniStatus,
                Prioritet.VISOK,
                null,
                DateTime.Now.AddDays(1),
                null
            );

            if (expectedException != null)
            {
                // Ako se očekuje izuzetak 
                var exception = Assert.ThrowsException<ArgumentException>(() =>
                {
                    zadatak.ZapocniZadatak();
                });
                Assert.IsInstanceOfType(exception, expectedException);
            }
            else
            {
                // Ako se ne očekuje izuzetak provjera statusa
                zadatak.ZapocniZadatak();
                Assert.AreEqual(ocekivaniStatus, zadatak.status);
                Assert.IsNotNull(zadatak.vrijemePocetka);
            }
        }

        [TestMethod]
        [DataRow(Status.U_TOKU, Status.ZAVRŠEN, true)]  //validno za ovaj sto je zapocet
        [DataRow(Status.U_ČEKANJU, Status.U_ČEKANJU, false)]  // nije validno za ovaj sto je u cekanju
        [DataRow(Status.ZAVRŠEN, Status.ZAVRŠEN, false)]  // nije validno za ovaj sto je zavrsen
        public void ZavrsiZadatak_RazlicitiScenariji(Status inicijalniStatus, Status expectedStatus, bool shouldComplete)
        {
            // Arrange
            var zadatak = new Zadatak(
                "Opis",
                Kategorija.POSLOVNI,
                inicijalniStatus,
                Prioritet.VISOK,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                null
            );

            if (shouldComplete)
            {
                zadatak.ZavrsiZadatak();
                Assert.AreEqual(expectedStatus, zadatak.status);
                Assert.IsNotNull(zadatak.vrijemeZavrsetka);
            }
            else
            {
                Assert.ThrowsException<Exception>(() => zadatak.ZavrsiZadatak());
            }
        }

        [TestMethod]
        public void ProvjeriRok_IstekaoRok_PostavljaStatusOdlOzen()
        {
            var zadatak = new Zadatak(
                "Opis",
                Kategorija.POSLOVNI,
                Status.U_ČEKANJU,
                Prioritet.NIZAK,
                null,
                DateTime.Now.AddHours(1),
                null
            );

            zadatak.rokZavrsetka=zadatak.rokZavrsetka.AddDays(-3); //ovo je ovdje postavljeno jer se gore poziva konstruktor koji vrsi validaciju
            //za rok, medjutim ova se funkcija poziva nekad poslije nakon sto je vec kreiran prije zadatak

            var promjena = zadatak.ProvjeriRok();

            Assert.IsTrue(promjena);
            Assert.AreEqual(Status.ODLOŽEN, zadatak.status);
        }

        [TestMethod]
        public void ProvjeriRok_NijeIstekaoRok_NemaPromjena()
        {
            var zadatak = new Zadatak(
                "Opis",
                Kategorija.LIČNI,
                Status.U_ČEKANJU,
                Prioritet.SREDNJI,
                null,
                DateTime.Now.AddDays(1),
                null
            );

            var promjena = zadatak.ProvjeriRok();

            Assert.IsFalse(promjena);
            Assert.AreEqual(Status.U_ČEKANJU, zadatak.status);
        }

    }
}

