using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Konzolna_aplikacija_TODO_lista_.Klase
{
    public class Podsjetnik
    {
        public DateTime vrijemeSlanja {  get; set; }
        public Zadatak zadatak { get; set; }
        public Boolean izvrsen { get; set; }
        public Podsjetnik(DateTime vrijemeSlanja, Zadatak zadatak, bool izvrsen,bool validate)
        {
            if(validate)
            ValidacijaPodataka(vrijemeSlanja, zadatak);
            this.vrijemeSlanja = vrijemeSlanja;
            this.zadatak = zadatak;
            this.izvrsen = izvrsen;
        }

        [JsonConstructor]
        public Podsjetnik(DateTime vrijemeSlanja, Zadatak zadatak, bool izvrsen)
        {
            // Konstruktor za deserializaciju JSON-a - preskače validaciju
            this.vrijemeSlanja = vrijemeSlanja;
            this.zadatak = zadatak;
            this.izvrsen = izvrsen;
        }

        private Boolean ValidacijaPodataka(DateTime vrijemeSlanja, Zadatak zadatak)
        {
            if (izvrsen == false)
            {
                if (zadatak == null)
                {
                    throw new ArgumentException("Podsjetnik mora biti vezan za neki zadatak!");
                }
                if (zadatak.status == Status.ZAVRŠEN)
                {
                    throw new ArgumentException("Ne možeš postaviti podsjetnik na završenom zadatku!");
                }
                if (vrijemeSlanja < DateTime.Now)
                {
                    throw new ArgumentException("Vrijeme slanja podsjetnika mora bit u buducnosti!");
                }
            }
            return true;
        }


        public Boolean ispisPodsjetnika()
        {
            var imaPromjene = false;
            if (vrijemeSlanja < DateTime.Now && izvrsen==false)
            {
                this.izvrsen = true;
                Console.WriteLine($"Vrijeme je da zavrsis zadatak: '{zadatak.opis}'!");
                imaPromjene = true;
            }
            return imaPromjene;
        }
    }
}
