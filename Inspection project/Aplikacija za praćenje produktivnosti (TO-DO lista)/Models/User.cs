using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<Task> UserTasks { get; set; }

        public User(int id, string username)
        {
            Id = id;
            Username = username;
            UserTasks = new List<Task>();
        }

        public void AddTask(Task task)
        {
            UserTasks.Add(task);
            Console.WriteLine($"Zadatak '{task.Name}' dodan korisniku {Username}.");
        }

        public void DisplayTasks()
        {
            if (UserTasks.Count == 0)
            {
                Console.WriteLine($"{Username} nema zadataka.");
                return;
            }
            foreach (var task in UserTasks)
            {
                Console.WriteLine(task);
            }
        }
    }
}
