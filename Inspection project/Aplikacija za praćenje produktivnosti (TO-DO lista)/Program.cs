using System;
using System.Collections.Generic;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Models;
using Aplikacija_za_praćenje_produktivnosti__TO_DO_lista_.Services;

class Program
{
    static void Main(string[] args)
    {
        TaskService taskService = new TaskService();
        PriorityService priorityService = new PriorityService();
        DeadlineService deadlineService = new DeadlineService();
        TaskComplexityService complexityService = new TaskComplexityService();

        bool running = true;

        // Kreiranje osnovnih kategorija
        var categories = new Category[]
        {
            new Category(1, "Posao"),
            new Category(2, "Privatno"),
            new Category(3, "Škola")
        };

        // Kreiranje korisnika
        User user = new User(1, "student");

        while (running)
        {
            Console.WriteLine("\n--- Aplikacija za praćenje produktivnosti (TO-DO lista) ---");
            Console.WriteLine("1. Dodaj novi zadatak");
            Console.WriteLine("2. Ažuriraj status zadatka");
            Console.WriteLine("3. Obriši zadatak");
            Console.WriteLine("4. Prikaži sve zadatke");
            Console.WriteLine("5. Sortiraj zadatke po prioritetu");
            Console.WriteLine("6. Pretraži zadatke");
            Console.WriteLine("7. Prikaži zadatke blizu roka");
            Console.WriteLine("8. Prikaži najkompleksniji zadatak");
            Console.WriteLine("9. Prikaži zadatke određene kategorije");
            Console.WriteLine("10. Izlaz");
            Console.Write("Izaberite opciju: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": // Dodaj novi zadatak
                    Console.Write("Unesite naziv zadatka: ");
                    string name = Console.ReadLine();
                    Console.Write("Unesite opis zadatka: ");
                    string description = Console.ReadLine();
                    /*Console.Write("Unesite rok zadatka (yyyy-mm-dd): ");
                    DateTime deadline = DateTime.Parse(Console.ReadLine());*/
                    DateTime deadline = DateTime.Now;
                    bool validDate = false;

                    while (!validDate)
                    {
                        Console.Write("Unesite rok zadatka (dd-MM-yyyy): ");
                        string input = Console.ReadLine();

                        // Pokušaj parsiranja unosa
                        if (DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out deadline))
                        {
                            // Provjera da rok nije u prošlosti
                            if (deadline >= DateTime.Now)
                            {
                                validDate = true;
                            }
                            else
                            {
                                Console.WriteLine("Greška: Rok zadatka ne može biti u prošlosti. Pokušajte ponovo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Neispravan format datuma. Molimo unesite datum u formatu dd-MM-yyyy.");
                        }
                    }



                    // Prikaz i odabir prioriteta
                    Console.WriteLine("Izaberite prioritet zadatka: ");
                    priorityService.DisplayPriorities();
                    int priorityId = int.Parse(Console.ReadLine());
                    Priority selectedPriority = priorityService.GetPriorityById(priorityId);

                    Console.WriteLine("Izaberite kategoriju zadatka: ");
                    for (int i = 0; i < categories.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories[i].Name}");
                    }
                    int categoryChoice = int.Parse(Console.ReadLine());
                    if (categoryChoice <= 0 || categoryChoice > 3)
                    {
                        Console.WriteLine("Odaberite jedno od ponudjenih kategorija zadataka!");
                        break;
                    }
                    Category selectedCategory = categories[categoryChoice - 1];

                    string status = "U toku";

                    taskService.AddTask(user, name, description, deadline, selectedPriority, status, selectedCategory);
                    Console.WriteLine("Zadatak uspješno dodan.");
                    break;

                case "2": // Ažuriraj status zadatka
                    Console.Write("Unesite ID zadatka za ažuriranje: ");
                    int updateId = int.Parse(Console.ReadLine());
                    Console.Write("Unesite novi status (Završeno/U toku): ");
                    status = Console.ReadLine();

                    taskService.UpdateTaskStatus(updateId, status);
                    break;

                case "3": // Obriši zadatak
                    Console.Write("Unesite ID zadatka za brisanje: ");
                    int deleteId = int.Parse(Console.ReadLine());

                    taskService.DeleteTask(deleteId);
                    break;

                case "4": // Prikaži sve zadatke
                    taskService.DisplayTasks();
                    break;

                case "5": // Sortiraj zadatke po prioritetu
                    taskService.SortTasksByPriority();
                    break;

                case "6": // Pretraži zadatke
                    Console.Write("Unesite ključnu riječ za pretragu: ");
                    string keyword = Console.ReadLine();
                    taskService.SearchTasks(keyword);
                    break;

                case "7": // Prikaži zadatke blizu roka
                    Console.Write("Unesite broj dana unutar kojih želite vidjeti zadatke blizu roka: ");
                    int days = int.Parse(Console.ReadLine());
                    var nearDeadlineTasks = deadlineService.GetTasksNearDeadline(taskService.GetTasks(), days);
                    if (nearDeadlineTasks.Count == 0) Console.WriteLine("Nema zadataka unutar odabranog broja dana!");
                    else
                    {
                        Console.WriteLine("\nZadaci blizu roka:");
                        foreach (var task in nearDeadlineTasks)
                        {
                            Console.WriteLine($"{task.Id}: {task.Name}, Rok: {task.Deadline}, Status: {task.Status}");
                        }
                    }

                    break;
            
                case "8": // Prikaži najkompleksniji zadatak
                    var mostComplexTask = complexityService.GetMostComplexTask(taskService.GetTasks());
                    if (mostComplexTask != null)
                    {
                        double complexity = complexityService.CalculateTaskComplexity(mostComplexTask);
                        Console.WriteLine($"\nNajkompleksniji zadatak: {mostComplexTask.Name}, Kompleksnost: {complexity}");
                    }
                    else
                    {
                        Console.WriteLine("Nema dostupnih zadataka.");
                    }
                    break;

                case "9":
                    Console.WriteLine("Unesite kategoriju za koju želite prikazati zadatke (1 za posao, 2 za privatno 3 za skola):");
                    int izborKategorije = int.Parse(Console.ReadLine());
                    taskService.SortTasksByCategory(izborKategorije);
                    break;

                case "10": // Izlaz
                    running = false;
                    Console.WriteLine("Zatvaranje aplikacije...");
                    break;

                default:
                    Console.WriteLine("Nepoznata opcija. Molimo pokušajte ponovo.");
                    break;
            }
        }
    }
}
