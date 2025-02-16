using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class TDDPrvaFunkcionalnost
    {
        private KorisnikServis korisnikServis;

        [TestInitialize]
        public void SetUp()
        {
            korisnikServis=new KorisnikServis();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerisiStatistikuKorisnika_PraznaLista_BacaIzuzetak()
        {
            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(new List<Korisnik>());
        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerisiStatistikuKorisnika_NullLista_BacaIzuzetak()
        {
            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(null);
        
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_JedanKorisnik()
        {
            var korisnici = new List<Korisnik>
            {
                new Korisnik("kazaz47","akaz@gmail.com","mojalozinka",new List<Zadatak>
                    {
                        new Zadatak("zadataktest1",Kategorija.OBRAZOVNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(1),null),
                        new Zadatak("zadataktest2",Kategorija.LIČNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(2),null)
                    },new List<Podsjetnik>())
            };
            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);

            Assert.IsTrue(rezultat.Contains("kazaz47"));
            Assert.IsTrue(rezultat.Contains("Ukupno zadataka: 2"));
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_DvaKorisnika_VracaKorisnikaSNajviseZadataka()
        {
            var korisnici = new List<Korisnik>
            {
                new Korisnik("kazaz47","akaz@gmail.com","mojalozinka",new List<Zadatak>
                    {
                        new Zadatak("zadataktest1",Kategorija.OBRAZOVNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(1),null),
                        new Zadatak("zadataktest2",Kategorija.LIČNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(2),null)
                    },new List<Podsjetnik>()),
                new Korisnik("test2","test2@gmail.com","mojalozinka2",new List<Zadatak>
                {
                        new Zadatak("zadataktest1",Kategorija.OBRAZOVNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(1),null),
                        new Zadatak("zadataktest2",Kategorija.LIČNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(2),null),
                        new Zadatak("zadataktest3",Kategorija.OBRAZOVNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(1),null),
                },new List<Podsjetnik>())
            };

            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);
            Assert.IsTrue(rezultat.Contains("Korisnik s najviše zadataka: test2"));
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_DvaKorisnika_VracaNeaktivnog()
        {

            var korisnici = new List<Korisnik>
            {
                new Korisnik("kazaz47","akaz@gmail.com","mojalozinka",new List<Zadatak>
                    {
                        new Zadatak("zadataktest1",Kategorija.OBRAZOVNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(1),null),
                        new Zadatak("zadataktest2",Kategorija.LIČNI,Status.U_ČEKANJU,Prioritet.NIZAK,null,DateTime.Now.AddDays(2),null)
                    },new List<Podsjetnik>()),
                new Korisnik("test2","test2@gmail.com","mojalozinka2",new List<Zadatak>(),new List<Podsjetnik>())
            };

            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);
            Assert.IsTrue(rezultat.Contains("Neaktivni korisnici: "));
            Assert.IsTrue(rezultat.Contains("- test2"));
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_ZavrseniZadaci()
        {
            var korisnici = new List<Korisnik>
        {
            new Korisnik("test1", "test1@gmail.com", "lozinka1", new List<Zadatak>
            {
                new Zadatak("zadatak1", Kategorija.LIČNI, Status.ZAVRŠEN, Prioritet.SREDNJI, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1)),
                new Zadatak("zadatak2", Kategorija.OBRAZOVNI, Status.ZAVRŠEN, Prioritet.VISOK, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1))
            }, new List<Podsjetnik>())
        };

            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);

            Assert.IsTrue(rezultat.Contains("Zavrsenih zadataka: 100%"));
            Assert.IsTrue(rezultat.Contains("Ukupno zadataka: 2"));
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_Podsjetnici()
        {
            var korisnici = new List<Korisnik>
        {
            new Korisnik("test2", "test2@gmail.com", "lozinka2", new List<Zadatak>
            {
                new Zadatak("zadatak1", Kategorija.OBRAZOVNI, Status.U_TOKU, Prioritet.NIZAK, DateTime.Now, DateTime.Now.AddDays(2), null)
            }, new List<Podsjetnik>
            {
                new Podsjetnik(DateTime.Now.AddHours(-2), null, true, false),
                new Podsjetnik(DateTime.Now.AddHours(-1), null, true, false)
            })
        };

            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);
            Assert.IsTrue(rezultat.Contains("Ukupno podsjetnika: 2"));
        }

        [TestMethod]
        public void GenerisiStatistikuKorisnika_IzvrseniPodsjetnici()
        {
            var korisnici = new List<Korisnik>
        {
            new Korisnik("test2", "test2@gmail.com", "lozinka2", new List<Zadatak>
            {
                new Zadatak("zadatak1", Kategorija.OBRAZOVNI, Status.U_TOKU, Prioritet.NIZAK, DateTime.Now, DateTime.Now.AddDays(2), null)
            }, new List<Podsjetnik>
            {
                new Podsjetnik(DateTime.Now.AddHours(-2), null, true, false),
                new Podsjetnik(DateTime.Now.AddHours(-1), null, true, false)
            })
        };

            var rezultat = korisnikServis.GenerisiStatistikuKorisnika(korisnici);
            Assert.IsTrue(rezultat.Contains("Ukupno podsjetnika: 2"));
            Assert.IsTrue(rezultat.Contains("Izvrseni podsjetnici: 100%"));
        }
    }
}
