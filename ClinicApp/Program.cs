namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();

        // --- Тестові дані пацієнтів ---
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), "B-", "0672345678"));
        patientManager.Add(new Patient("Валентин", "Руснак", new DateTime(2008, 10, 15), "O+", "0933456789")); // Тест для категорії "дитина/дорослий"
        patientManager.Add(new Patient("Марія", "Ткач"));

        // --- Тестові дані лікарів ---
        doctorManager.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567") { WorkStartHour = 8, WorkEndHour = 16 });
        doctorManager.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678") { WorkStartHour = 9, WorkEndHour = 18 });
        doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789") { WorkStartHour = 8, WorkEndHour = 17 });

        // --- Демонстрація Задачі 5 (Appointment) ---
        Console.WriteLine("\n=== Демонстрація Задачі 5 (Прийоми) ===");
        Appointment a1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        Appointment a2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment a3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine(a1);
        Console.WriteLine(a2);
        Console.WriteLine(a3);

        a1.Cancel("Пацієнт не зміг прийти");
        a2.Complete();

        Console.WriteLine("\n// Після Cancel та Complete:");
        Console.WriteLine(a1);
        Console.WriteLine(a2);
        Console.WriteLine("=======================================\n");

        // --- Головне меню ---
        while (true)
        {
            Console.WriteLine("\n=== Головне меню ===");
            Console.WriteLine("1. Керування пацієнтами");
            Console.WriteLine("2. Керування лікарями");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                PatientMenu(patientManager);
            }
            else if (choice == "2")
            {
                DoctorMenu(doctorManager);
            }
            else if (choice == "0")
            {
                Console.WriteLine("Роботу завершено.");
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
    }

    // --- Локальний метод для меню Пацієнтів ---
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
            Console.WriteLine("0. Назад до головного меню");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                manager.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Введіть ім'я: ");
                string firstName = Console.ReadLine()!;
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
                else
                {
                    Console.WriteLine("Некоректний формат ID.");
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

    // --- Локальний метод для меню Лікарів ---
    static void DoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню Лікарі ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Знайти за спеціальністю");
            Console.WriteLine("3. Видалити за ID");
            Console.WriteLine("4. Статистика");
            Console.WriteLine("0. Назад до головного меню");
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
            else if (choice == "3")
            {
                Console.Write("Введіть ID для видалення: ");
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