using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolnaaplikacija_TODO_lista.Klase;
using Newtonsoft.Json;

namespace UnitTest
{
    [TestClass]
    public class PodsjetnikTest
    {
        private Zadatak zadatak;
        private DateTime vrijemeSlanja;

        [TestInitialize]
        public void SetUp()
        {
            vrijemeSlanja = DateTime.Now.AddMinutes(10);
            zadatak = new Zadatak("Zadataktest opis", Kategorija.OBRAZOVNI, Status.U_ČEKANJU, Prioritet.VISOK, null, vrijemeSlanja, null);
        }

        [TestMethod]
        public void Konstruktor_ValidniUnos_Neizvrsen_NapraviInstancu() //validno i vrijeme slanja i zadatak
        {
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, false, true);
            Assert.AreEqual(vrijemeSlanja, podsjetnik.vrijemeSlanja);
            Assert.AreEqual(zadatak, podsjetnik.zadatak);
            Assert.IsFalse(podsjetnik.izvrsen);
        }

        [TestMethod]
        public void Konstruktor_ValidniUnos_Izvrsen_NapraviInstancu() //validno i vrijeme slanja i zadatak ali je izvrsen
        {
            var vrijemeSlanja = DateTime.Now.AddMinutes(-10);
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, true, false);
            Assert.AreEqual(vrijemeSlanja, podsjetnik.vrijemeSlanja);
            Assert.AreEqual(zadatak, podsjetnik.zadatak);
            Assert.IsTrue(podsjetnik.izvrsen);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Konstruktor_NevalidnoVrijemeSlanja_BacaIzuzetak()
        {
            var vrijemeSlanja= DateTime.Now.AddMinutes(-10);
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, false, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Konstruktor_NevalidniStatusZadatka_BacaIzuzetak()
        {
            zadatak.status = Status.ZAVRŠEN;
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, false, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Konstruktor_NevalidniNullZadatak_BacaIzuzetak()
        {
            var podsjetnik = new Podsjetnik(vrijemeSlanja, null, false, true);
        }

        [TestMethod]
        public void Konstruktor_Deserializacija_JSON_NapraviInstancu_BezValidacije()
        {
            string json = @"
            {
             ""vrijemeSlanja"": ""2024-10-24T15:30:00"",
            ""zadatak"": {
            ""opis"": ""Test Zadatak"",
            ""kategorija"": ""OBRAZOVNI"",
            ""status"": ""U_ČEKANJU"",
            ""prioritet"": ""VISOK"",
            ""vrijemePocetka"": ""2024-11-24T14:30:00"",
            ""rokZavrsetka"": ""2024-11-25T16:00:00"",
            ""vrijemeZavrsetka"": null
             },
          ""izvrsen"": false
             }";

            var podsjetnik = JsonConvert.DeserializeObject<Podsjetnik>(json);

            Assert.IsNotNull(podsjetnik);
            Assert.AreEqual(DateTime.Parse("2024-10-24T15:30:00"), podsjetnik.vrijemeSlanja);
            Assert.IsNotNull(podsjetnik.zadatak);
            Assert.AreEqual("Test Zadatak", podsjetnik.zadatak.opis);
            Assert.IsFalse(podsjetnik.izvrsen);
        }

        [TestMethod]
        public void IspisPodsjetnika_NijeIsteklo_VrijemeSlanja_NijeIzvrsen()
        {
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, false, false);
            Assert.IsFalse(podsjetnik.ispisPodsjetnika());
        }

        [TestMethod]
        [DataRow(true, false, false)]  // Isteklo vrijeme slanja, izvršeno false, ispis true
        [DataRow(false, false, true)]  // Isteklo vrijeme slanja, izvršeno false, ispis false
        public void IspisPodsjetnika_IstekloVrijemeSlanja(bool izvršen, bool očekivanoRezultat, bool očekivanaVrijednost)
        {
            var vrijemeSlanja = DateTime.Now.AddMinutes(-10);
            var podsjetnik = new Podsjetnik(vrijemeSlanja, zadatak, izvršen, false);

            // Provjera ispisivanja
            Assert.AreEqual(očekivanaVrijednost, podsjetnik.ispisPodsjetnika());
        }
    }
}