using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models
{
    public interface IDeadlineService
    {
        public TimeSpan CalculateTimeUntilDeadline(Task task);

        public List<Task> GetTasksNearDeadline(List<Task> tasks, int days);

        public List<Task> GetOverdueTasks(List<Task> tasks);
        
    }
}
