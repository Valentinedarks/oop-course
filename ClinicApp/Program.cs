namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        PatientManager manager = new PatientManager();

        // Початкові дані для тестування
        manager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
        manager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), "B-", "0672345678"));
        manager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 2, 10), "O+", "0933456789"));
        manager.Add(new Patient("Марія", "Ткач"));

        // Виклик підменю
        PatientMenu(manager);
    }

    // Окремий локальний метод для підменю пацієнтів[cite: 12]
    static void PatientMenu(PatientManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню Пацієнти ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                manager.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Введіть ім'я: ");
                string firstName = Console.ReadLine()!; // ! гарантує, що не null[cite: 2]
                Console.Write("Введіть прізвище: ");
                string lastName = Console.ReadLine()!;
                
                manager.Add(new Patient(firstName, lastName));
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ім'я або прізвище для пошуку: ");
                string query = Console.ReadLine()!;
                Patient[] found = manager.FindByName(query);
                
                if (found.Length == 0)
                {
                    Console.WriteLine("Нікого не знайдено.");
                }
                else
                {
                    Console.WriteLine($"\nЗнайдено ({found.Length}):");
                    for (int i = 0; i < found.Length; i++)
                    {
                        Console.WriteLine(found[i].ToString());
                    }
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool success = manager.Remove(id);
                    Console.WriteLine(success ? "Пацієнта видалено." : "Пацієнта з таким ID не знайдено.");
                }
            }
            else if (choice == "5")
            {
                manager.DisplayStats();
            }
            else if (choice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
    }
}