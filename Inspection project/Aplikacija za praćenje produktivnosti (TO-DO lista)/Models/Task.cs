using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public Priority Priority { get; set; } // Visok, Srednji, Nizak
        public string Status { get; set; } // U toku, Završeno
        public Category Category { get; set; } 

        public Task(int id, string name, string description, DateTime deadline, Priority priority, string status, Category category) 
        {
            Id = id;
            Name = name;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            Status = status;
            Category = category;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Naziv: {Name}, Kategorija: {Category.Name}, Opis: {Description}, Rok: {Deadline.ToShortDateString()}, Prioritet: {Priority.Level}, Status: {Status}";
    
        }

    }
}
