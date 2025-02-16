using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models
{
    public class TaskComplexityService : ITaskComplexityService
    {
        public double CalculateTaskComplexity(Task task)
        {
            // Pretpostavka: 
            // - Prioritet "Visok" = 3, "Srednji" = 2, "Nizak" = 1
            // - Kraći rok = veća kompleksnost
            double priorityFactor = task.Priority.Level == "Visok" ? 3 : (task.Priority.Level == "Srednji" ? 2 : 1);
            double descriptionLengthFactor = task.Description.Length / 100.0; // Duži opis = kompleksniji zadatak
            double deadlineFactor = (task.Deadline - DateTime.Now).TotalDays < 3 ? 2 : 1; // Ako je rok unutar 3 dana = veća kompleksnost

            return priorityFactor * descriptionLengthFactor * deadlineFactor;
        }

        public Task GetMostComplexTask(List<Task> tasks)
        {
            return tasks.OrderByDescending(CalculateTaskComplexity).FirstOrDefault();
        }
    }

}
