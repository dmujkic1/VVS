using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest
{   
    public interface IZadatakStub
    {
        List<Zadatak> VratiZadatke();
    }

    public class ZadatakStub : IZadatakStub
    {
        public List<Zadatak> VratiZadatke()
        {
            return new List<Zadatak>
            {
                new Zadatak("Zadatak 1", Kategorija.LIČNI, Status.U_ČEKANJU, Prioritet.NIZAK, null, DateTime.Now.AddDays(1), null),
                new Zadatak("Zadatak 2", Kategorija.POSLOVNI, Status.U_TOKU, Prioritet.SREDNJI, DateTime.Now, DateTime.Now.AddDays(2), null),
                new Zadatak("Zadatak 3", Kategorija.OBRAZOVNI, Status.ZAVRŠEN, Prioritet.VISOK, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-1), DateTime.Now),
                new Zadatak("Zadatak 4", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.SREDNJI, DateTime.Now, DateTime.Now.AddDays(3), DateTime.Now.AddDays(-1)),
                new Zadatak("Zadatak 5", Kategorija.POSLOVNI, Status.U_ČEKANJU, Prioritet.NIZAK, null, DateTime.Now.AddDays(5), null)
            };
        }
    }

    [TestClass]
    public class FiltriranjeZadatakaServisTest
    {
        private FiltriranjeZadatakaServis _filtriranjeZadatakaServis;
        private List<Zadatak> _zadaci;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            _filtriranjeZadatakaServis = new FiltriranjeZadatakaServis();

            // Koristenje Stub-a za inicijalizaciju liste zadataka
            IZadatakStub stub = new ZadatakStub();
            _zadaci = stub.VratiZadatke();
        }

        [TestMethod]
        public void KategorijaFilter_FiltriranjeLicniKategorija_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.KategorijaFilter(_zadaci, "1");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.kategorija == Kategorija.LIČNI));
        }

        [TestMethod]
        public void KategorijaFilter_FiltriranjePoslovniKategorija_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.KategorijaFilter(_zadaci, "2");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.kategorija == Kategorija.POSLOVNI));
        }

        [TestMethod]
        public void KategorijaFilter_FiltriranjeObrazovniKategorija_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.KategorijaFilter(_zadaci, "3");

            // Assert
            Assert.AreEqual(1, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.kategorija == Kategorija.OBRAZOVNI));
        }

        [TestMethod]
        public void StatusFilter_FiltriranjeUCekanjuStatus_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.StatusFilter(_zadaci, "1");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.status == Status.U_ČEKANJU));
        }

        [TestMethod]
        public void StatusFilter_FiltriranjeUTokuStatus_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.StatusFilter(_zadaci, "2");

            // Assert
            Assert.AreEqual(1, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.status == Status.U_TOKU));
        }

        [TestMethod]
        public void StatusFilter_FiltriranjeZavrsenStatus_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.StatusFilter(_zadaci, "3");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.status == Status.ZAVRŠEN));
        }

        [TestMethod]
        public void PrioritetFilter_FiltriranjeNizakPrioritet_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.PrioritetFilter(_zadaci, "1");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.prioritet == Prioritet.NIZAK));
        }

        [TestMethod]
        public void PrioritetFilter_FiltriranjeSrednjiPrioritet_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.PrioritetFilter(_zadaci, "2");

            // Assert
            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.prioritet == Prioritet.SREDNJI));
        }

        [TestMethod]
        public void PrioritetFilter_FiltriranjeVisokPrioritet_VracaIspravneZadatke()
        {
            // Act
            var rezultat = _filtriranjeZadatakaServis.PrioritetFilter(_zadaci, "3");

            // Assert
            Assert.AreEqual(1, rezultat.Count);
            Assert.IsTrue(rezultat.TrueForAll(z => z.prioritet == Prioritet.VISOK));
        }
    }
}