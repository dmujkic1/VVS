using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konzolna_aplikacija_TODO_lista_.Servisi;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Newtonsoft.Json;
using System.Reflection;

namespace UnitTest
{
    [TestClass]
    public class AutentifikacijaServisTest
    {
        private AutentifikacijaServis _servis;
        private string _tempFilePath;

        [TestInitialize]
        public void Setup()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _servis = new AutentifikacijaServis();
            PostaviPrivremenuPutanju(_servis, _tempFilePath);
        }
    
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        private void PostaviPrivremenuPutanju(AutentifikacijaServis servis, string novaPutanja)
        {
            var field = typeof(AutentifikacijaServis).GetField("putanjafilea", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(servis, novaPutanja);
            }
        }

        private Korisnik KreirajValidnogKorisnika()
        {
            return new Korisnik(
                korisnickoIme: "validUser",
                email: "valid.email@example.com",
                lozinka: _servis.Enkripcija("password1"),
                toDoLista: new List<Zadatak>(),
                listaPodsjetnika: new List<Podsjetnik>()
            );
        }

        public interface IStubKorisnik
        {
            Korisnik GetKorisnik();
        }

        public class StubKorisnik : IStubKorisnik
        {
            public Korisnik GetKorisnik()
            {
                var _servis = new AutentifikacijaServis();
                return new Korisnik("validUser","email@email.com", _servis.Enkripcija("hashedPassword"), new List<Zadatak>(), new List<Podsjetnik>());
            }
        }




        [TestMethod]
        [DataRow("{ 'korisnici': [ 'ime': 'testkorisnik' ] }")]
        [DataRow("{ 'korisnici': [{ 'ime': 'testkorisnik' }] }]")]
        [DataRow("[{\"nonexistentProperty\": 123}]")] 
        public void GetKorisnici_JsonSerializationException_Exception_VracaPraznuListu(string invalidJson)
        {
            File.WriteAllText(_tempFilePath, invalidJson);

            var result = _servis.getKorisnici();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetKorisnici_IOException_VracaPraznuListu()
        {
            using (var stream = new FileStream(_tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                var result = _servis.getKorisnici();

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        public static IEnumerable<object[]> TestCredentials
        {
            get
            {
                yield return new object[] { "validUser", "password1", true };
                yield return new object[] { "validUser", "wrongPassword", false };
                yield return new object[] { "invalidUser", "password1", false };
                yield return new object[] { "validUser", "password123", false };
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestCredentials), DynamicDataSourceType.Property)] 
        public void Prijava_RazniPokusaji(string korisnickoIme, string lozinka, bool shouldReturnUser)
        {
            var korisnici = new List<Korisnik> { KreirajValidnogKorisnika() };
            File.WriteAllText(_tempFilePath, JsonConvert.SerializeObject(korisnici));

            var result = _servis.Prijava(korisnickoIme, lozinka);

            if (shouldReturnUser)
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(korisnickoIme, result.korisnickoIme);
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void Prijava_StubKorisnik()
        {
            StubKorisnik stubKorisnik = new StubKorisnik();
            var korisnik = stubKorisnik.GetKorisnik();

            var korisnici = new List<Korisnik> { korisnik};
            File.WriteAllText(_tempFilePath, JsonConvert.SerializeObject(korisnici));

            var prijavaResult = _servis.Prijava("validUser", "hashedPassword");

            Assert.IsNotNull(prijavaResult);
            Assert.AreEqual("validUser", prijavaResult.korisnickoIme);
        }

        [TestMethod]
        public void Prijava_ValidniPodaci_VracaKorisnika()
        {
            var korisnici = new List<Korisnik> { KreirajValidnogKorisnika() };
            File.WriteAllText(_tempFilePath, JsonConvert.SerializeObject(korisnici));

            var result = _servis.Prijava("validUser", "password1");

            Assert.IsNotNull(result);
            Assert.AreEqual("validUser", result.korisnickoIme);
        }

        [TestMethod]
        public void Prijava_NevalidnoKorisnickoIme_VracaNull()
        {
            var korisnici = new List<Korisnik> { KreirajValidnogKorisnika() };
            File.WriteAllText(_tempFilePath, JsonConvert.SerializeObject(korisnici));

            var result = _servis.Prijava("invalidUser", "password1");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Prijava_NevalidnaSifra_VracaNull()
        {
            var korisnici = new List<Korisnik> { KreirajValidnogKorisnika() };
            File.WriteAllText(_tempFilePath, JsonConvert.SerializeObject(korisnici));

            var result = _servis.Prijava("validUser", "wrongPassword");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Enkripcija_ValidnaSifra_VracaEnkriptovaniString()
        {
            var password = "password1";

            var result = _servis.Enkripcija(password);

            Assert.IsNotNull(result);
            Assert.AreEqual(_servis.Enkripcija(password), result);
        }
    }
}
