using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models
{
    public interface ITaskComplexityService
    {
        public double CalculateTaskComplexity(Task task);

        public Task GetMostComplexTask(List<Task> tasks);
    }
}
