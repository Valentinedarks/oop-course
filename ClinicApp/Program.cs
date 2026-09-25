namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();

        // Тестові дані пацієнтів
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), "B-", "0672345678"));

        // Тестові дані лікарів
        doctorManager.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567") { WorkStartHour = 8, WorkEndHour = 16 });
        doctorManager.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678") { WorkStartHour = 9, WorkEndHour = 18 });
        doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789") { WorkStartHour = 8, WorkEndHour = 17 });

        // Головне меню
        while (true)
        {
            Console.WriteLine("\n=== Головне меню ===");
            Console.WriteLine("1. Керування пацієнтами");
            Console.WriteLine("2. Керування лікарями");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1") PatientMenu(patientManager);
            else if (choice == "2") DoctorMenu(doctorManager);
            else if (choice == "0") break;
            else Console.WriteLine("Невідома команда.");
        }
    }

    // Метод для пацієнтів залишається з попереднього кроку
    static void PatientMenu(PatientManager manager)
    {
        // ... (Тут код з попереднього кроку) ...
        // Щоб не дублювати гігантський шматок коду, просто встав сюди попередній PatientMenu
        Console.WriteLine("Перехід у меню пацієнтів (код з попереднього завдання)");
    }

    // Нове меню для лікарів[cite: 13]
    static void DoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню Лікарі ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Знайти за спеціальністю");
            Console.WriteLine("3. Видалити за ID");
            Console.WriteLine("4. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                manager.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Введіть спеціальність: ");
                string spec = Console.ReadLine()!;
                Doctor[] found = manager.FindBySpeciality(spec);
                
                if (found.Length == 0) Console.WriteLine("Нікого не знайдено.");
                else
                {
                    Console.WriteLine($"\nЗнайдено ({found.Length}):");
                    for (int i = 0; i < found.Length; i++)
                        Console.WriteLine(found[i].ToString());
                }
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ID для видалення: ");
                // Використовуємо int.TryParse для безпечного парсингу[cite: 14]
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool success = manager.Remove(id);
                    Console.WriteLine(success ? "Лікаря видалено." : "Не знайдено.");
                }
                else
                {
                    Console.WriteLine("Некоректний формат ID.");
                }
            }
            else if (choice == "4")
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