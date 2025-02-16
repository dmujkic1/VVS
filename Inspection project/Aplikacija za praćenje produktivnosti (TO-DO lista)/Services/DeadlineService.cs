using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;
namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;


public class DeadlineService : IDeadlineService
{
    public TimeSpan CalculateTimeUntilDeadline(Task task)
    {
        return task.Deadline - DateTime.Now;
    }

    public List<Task> GetTasksNearDeadline(List<Task> tasks, int days)
    {
        DateTime today = DateTime.Now.Date;

        return tasks
            .Where(t => (t.Deadline.Date - today).TotalDays <= days && (t.Deadline.Date - today).TotalDays >= 0 && t.Status != "Završeno")
            .ToList();
    }


    public List<Task> GetOverdueTasks(List<Task> tasks)
    {
        return tasks.Where(t => t.Deadline < DateTime.Now && t.Status != "Završeno").ToList();
    }
}
