using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services
{
     public interface IPriorityService
    {
        public void DisplayPriorities();
        public Priority GetPriorityById(int id);
        public int ComparePriorityLevels(string level1, string level2);
    }
}
