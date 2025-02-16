using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services
{
    public interface ITaskService
    {
        void AddTask(User user, string name, string description, DateTime deadline, Priority priority, string status, Category category);
        void UpdateTaskStatus(int taskId, string status);
        void DeleteTask(int taskId);
        void DisplayTasks();
        void SortTasksByPriority();
        void SearchTasks(string keyword);

    }
}
