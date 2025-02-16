using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolnaaplikacija_TODO_lista.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class AnalitikaServisTest
    {

        [TestMethod]
        public void generisiIzvjestaj()
        {

            var analitikaServis = new AnalitikaServis();
            var zadaci = new List<Zadatak>
            {
                new Zadatak("Zadatak 1", Kategorija.OBRAZOVNI, Status.ZAVRŠEN,   Prioritet.NIZAK, DateTime.Now.AddHours(-5),DateTime.Now.AddHours(5), DateTime.Now.AddHours(-3)),
                new Zadatak("Zadatak 2", Kategorija.OBRAZOVNI, Status.U_TOKU,    Prioritet.VISOK, DateTime.Now.AddHours(-2),DateTime.Now.AddHours(5), DateTime.Now),
                new Zadatak("Zadatak 3", Kategorija.OBRAZOVNI, Status.U_ČEKANJU, Prioritet.VISOK, DateTime.Now.AddHours(-4),DateTime.Now.AddHours(5), DateTime.Now.AddHours(-2) ),
                new Zadatak("Zadatak 4", Kategorija.OBRAZOVNI, Status.ODLOŽEN,   Prioritet.NIZAK, DateTime.Now.AddHours(-3),DateTime.Now.AddHours(5), DateTime.Now.AddHours(-1))
            };


            var expectedUkupno = 4;
            var expectedZavrseni = 1;
            var expectedAktivni = 1;
            var expectedCekanje = 1;
            var expectedOdlozeni = 1;
            var expectedProsjecnoVrijeme = TimeSpan.FromHours(2).TotalSeconds;


            var izvještaj = analitikaServis.GenerisiIzvještaj(zadaci);



            Assert.AreEqual(expectedUkupno, izvještaj.UkupnoZadataka);
            Assert.AreEqual(expectedZavrseni, izvještaj.ZavrseniZadaci);
            Assert.AreEqual(expectedAktivni, izvještaj.AktivniZadaci);
            Assert.AreEqual(expectedCekanje, izvještaj.CekanjeZadaci);
            Assert.AreEqual(expectedOdlozeni, izvještaj.OdlozeniZadaci);
            Assert.AreEqual(expectedProsjecnoVrijeme, Math.Round((izvještaj.ProsječnoVrijemeZavršetka.TotalSeconds)));
        }

        [TestMethod]
        public void generisiIzvjestaj_SveNule()
        {

            var analitikaServis = new AnalitikaServis();
            var zadaci = new List<Zadatak>();


            var izvještaj = analitikaServis.GenerisiIzvještaj(zadaci);


            Assert.AreEqual(0, izvještaj.UkupnoZadataka);
            Assert.AreEqual(0, izvještaj.ZavrseniZadaci);
            Assert.AreEqual(0, izvještaj.AktivniZadaci);
            Assert.AreEqual(0, izvještaj.CekanjeZadaci);
            Assert.AreEqual(0, izvještaj.OdlozeniZadaci);
            Assert.AreEqual(TimeSpan.Zero, izvještaj.ProsječnoVrijemeZavršetka);
        }


    }
}