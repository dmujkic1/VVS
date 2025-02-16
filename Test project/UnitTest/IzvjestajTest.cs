using Konzolnaaplikacija_TODO_lista.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{

    public interface IProsjecnoVrijemeZavrsetka
    {
        TimeSpan IzracunajProsjecnoVrijeme();
    }


    public class ProsječnoVrijemeStub : IProsjecnoVrijemeZavrsetka
    {
        public TimeSpan IzracunajProsjecnoVrijeme()
        {

            return TimeSpan.FromMinutes(50);
        }
    }

    [TestClass]
    public class IzvjestajTests
    {
        [TestMethod]
        [DataRow(10, 4, 3, 2, 1)]
        [DataRow(0, 0, 0, 0, 0)]
        public void izvjestaj(int ukupnoZadataka, int zavrseniZadaci, int aktivniZadaci, int cekanjeZadaci, int odlozeniZadaci)
        {


            TimeSpan prosjecnoVrijemeZavršetka = TimeSpan.FromMinutes(45);


            var izvještaj = new Izvještaj
            {
                UkupnoZadataka = ukupnoZadataka,
                ZavrseniZadaci = zavrseniZadaci,
                AktivniZadaci = aktivniZadaci,
                CekanjeZadaci = cekanjeZadaci,
                OdlozeniZadaci = odlozeniZadaci,
                ProsječnoVrijemeZavršetka = prosjecnoVrijemeZavršetka
            };


            Assert.AreEqual(ukupnoZadataka, izvještaj.UkupnoZadataka);
            Assert.AreEqual(zavrseniZadaci, izvještaj.ZavrseniZadaci);
            Assert.AreEqual(aktivniZadaci, izvještaj.AktivniZadaci);
            Assert.AreEqual(cekanjeZadaci, izvještaj.CekanjeZadaci);
            Assert.AreEqual(odlozeniZadaci, izvještaj.OdlozeniZadaci);
            Assert.AreEqual(prosjecnoVrijemeZavršetka, izvještaj.ProsječnoVrijemeZavršetka);
        }



        [TestMethod]
        public void izvjestajStub()
        {

            var stub = new ProsječnoVrijemeStub();
            var izvještaj = new Izvještaj();


            izvještaj.ProsječnoVrijemeZavršetka = stub.IzracunajProsjecnoVrijeme();


            Assert.AreEqual(TimeSpan.FromMinutes(50), izvještaj.ProsječnoVrijemeZavršetka);
        }
    }
}