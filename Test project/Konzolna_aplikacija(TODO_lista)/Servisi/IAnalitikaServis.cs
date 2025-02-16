using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konzolna_aplikacija_TODO_lista_.Klase;
using Konzolnaaplikacija_TODO_lista.Klase;

namespace Konzolnaaplikacija_TODO_lista.Servisi
{
    public interface IAnalitikaServis
    {

        Izvještaj GenerisiIzvještaj( List<Zadatak> zadaci);



    }
}