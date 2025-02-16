using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;


namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models

{
    public class TaskService : ITaskService
    {
        private List<Task> tasks = new List<Task>();
        private int nextId = 1;

        // Kreiranje novog zadatka ideee
        public void AddTask(User user, string name, string description, DateTime deadline, Priority priority, string status, Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException("Naziv i opis zadatka ne smiju biti prazni.");
                }

                Task newTask = new Task(nextId, name, description, deadline, priority, status, category);
                tasks.Add(newTask);
                user.AddTask(newTask);
                nextId++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        // Ažuriranje statusa zadatka
        public void UpdateTaskStatus(int taskId, string status)
        {
            try
            {
                Task task = tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                {
                    throw new Exception("Zadatak nije pronađen.");
                }
                task.Status = status;
                Console.WriteLine("Status zadatka ažuriran.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        // Brisanje zadatka
        public void DeleteTask(int taskId)
        {
            try
            {
                Task task = tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                {
                    throw new Exception("Zadatak nije pronađen.");
                }
                tasks.Remove(task);
                Console.WriteLine("Zadatak uspješno obrisan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        // Prikaz svih zadataka
        public void DisplayTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Nema dodanih zadataka.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        // Sortiranje zadataka po prioritetu (Visok -> Nizak)
        public void SortTasksByPriority()
        {
            try
            {
                var priorityOrder = new Dictionary<string, int>
        {
            { "Visok", 3 },
            { "Srednji", 2 },
            { "Nizak", 1 }
        };

                // Sortiranje po nivou prioriteta koristeći definiranu hijerarhiju
                var sortedTasks = tasks
                    .OrderByDescending(t => priorityOrder.ContainsKey(t.Priority.Level) ? priorityOrder[t.Priority.Level] : 0)
                    .ToList();

                int br = 0;
                foreach (var task in sortedTasks)
                {
                    br++;
                    Console.WriteLine($"({br}) \"{task.Name}\" - Prioritet: {task.Priority.Level}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri sortiranju: {ex.Message}");
            }
        }

        public void SortTasksByCategory(int izbor)
        {
            try
            {
                var filteredList = new List<Task>();
                if(izbor == 1)
                {
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].Category.Name == "Posao") { filteredList.Add(tasks[i]); }
                    }
                }else if(izbor == 2)
                {
                    for(int i = 0;i < tasks.Count; i++)
                    {
                        if (tasks[i].Category.Name == "Privatno")
                        {
                            filteredList.Add(tasks[i]);
                        }
                    }
                }else if (izbor == 3)
                {
                    for(int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].Category.Name == "Škola")
                        {
                            filteredList.Add(tasks[i]);
                        }
                    }
                }else
                {
                    throw new ArgumentException("Niste ispravno izabrali kategoriju!");
                }
                if(filteredList.Count == 0) { throw new Exception("Nema dodanih zadataka!"); }
                foreach (var task in filteredList) {
                    Console.WriteLine(task);
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine($"Greska pri filtriranju: {ex.Message}");
            }
           
        }


        // Pretraga zadataka po ključnim riječima
        public void SearchTasks(string keyword)
        {
            try
            {
                var foundTasks = tasks.Where(t => t.Name.Contains(keyword)).ToList();
                if (foundTasks.Count == 0)
                {
                    Console.WriteLine("Nema zadataka koji odgovaraju pretrazi.");
                    return;
                }
                foreach (var task in foundTasks)
                {
                    Console.WriteLine(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri pretrazi: {ex.Message}");
            }
            
        }

        public List<Task> GetTasks()
        {
            return tasks;
        }
            

        
    }
}
