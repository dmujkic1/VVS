using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;
namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;

public class PriorityService : IPriorityService
{
    private List<Priority> priorities = new List<Priority>
{
    new Priority { Id = 1, Level = "Visok" },
    new Priority { Id = 2, Level = "Srednji" },
    new Priority { Id = 3, Level = "Nizak" }
};

    public void DisplayPriorities()
    {
        foreach (var priority in priorities)
        {
            Console.WriteLine($"{priority.Id}: {priority.Level}");
        }
    }

    public Priority GetPriorityById(int id)
    {
        return priorities.FirstOrDefault(p => p.Id == id);
    }

    public string GetPriorityLevel(int id)
    {
        var priority = priorities.FirstOrDefault(p => p.Id == id);
        return priority != null ? priority.Level : null;
    }
    
    public int ComparePriorityLevels(string level1, string level2)
{
    // Definiramo hijerarhiju prioriteta ("Visok" > "Srednji" > "Nizak")
    var priorityOrder = new Dictionary<string, int>
    {
        { "Visok", 3 },
        { "Srednji", 2 },
        { "Nizak", 1 }
    };

    if (!priorityOrder.ContainsKey(level1) || !priorityOrder.ContainsKey(level2))
    {
        Console.WriteLine("Jedan ili oba nivoa prioriteta nisu validni.");
        return 0; // Indikacija da poređenje nije uspješno jer nedostaje validan nivo
    }

    int priorityValue1 = priorityOrder[level1];
    int priorityValue2 = priorityOrder[level2];

    return priorityValue1.CompareTo(priorityValue2);
}


}

