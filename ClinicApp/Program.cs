using System;

namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        // 1. Ініціалізація менеджерів
        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();
        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

        // 2. Наповнення тестовими даними (щоб не вводити все вручну при кожному запуску)
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), "B-", "0672345678"));
        patientManager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 2, 10), "O+", "0933456789"));

        doctorManager.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567") { WorkStartHour = 8, WorkEndHour = 16 });
        doctorManager.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678") { WorkStartHour = 9, WorkEndHour = 18 });
        doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789") { WorkStartHour = 8, WorkEndHour = 17 });

        // Додаємо тестові записи на прийом
        appointmentManager.Book(1, 1, DateTime.Now.AddDays(1).AddHours(2), 30);
        appointmentManager.Book(2, 2, DateTime.Now.AddDays(2).AddHours(3), 45);

        // 3. Головний цикл програми
        while (true)
        {
            Console.WriteLine("\n===========================");
            Console.WriteLine("=== КЛІНІКА: ГОЛОВНЕ МЕНЮ ===");
            Console.WriteLine("===========================");
            Console.WriteLine("1. Керування пацієнтами");
            Console.WriteLine("2. Керування лікарями");
            Console.WriteLine("3. Керування записами на прийом");
            Console.WriteLine("0. Вихід з програми");
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
            else if (choice == "3")
            {
                AppointmentMenu(appointmentManager, patientManager, doctorManager);
            }
            else if (choice == "0")
            {
                Console.WriteLine("Роботу завершено. Навседобре!");
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда. Спробуйте ще раз.");
            }
        }
    }

    // ==========================================
    // ПІДМЕНЮ: ПАЦІЄНТИ
    // ==========================================
    static void PatientMenu(PatientManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню: Пацієнти ---");
            Console.WriteLine("1. Показати всіх пацієнтів");
            Console.WriteLine("2. Додати нового пацієнта");
            Console.WriteLine("3. Знайти пацієнта за ім'ям або прізвищем");
            Console.WriteLine("4. Видалити пацієнта за ID");
            Console.WriteLine("5. Статистика пацієнтів");
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
                    Console.WriteLine("Пацієнтів не знайдено.");
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
                Console.Write("Введіть ID пацієнта для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool success = manager.Remove(id);
                    Console.WriteLine(success ? "Пацієнта успішно видалено." : "Пацієнта з таким ID не знайдено.");
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

    // ==========================================
    // ПІДМЕНЮ: ЛІКАРІ
    // ==========================================
    static void DoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню: Лікарі ---");
            Console.WriteLine("1. Показати всіх лікарів");
            Console.WriteLine("2. Знайти лікаря за спеціальністю");
            Console.WriteLine("3. Видалити лікаря за ID");
            Console.WriteLine("4. Статистика лікарів");
            Console.WriteLine("5. Додати нового лікаря (базовий запис)");
            Console.WriteLine("0. Назад до головного меню");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                manager.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Введіть спеціальність для пошуку: ");
                string spec = Console.ReadLine()!;
                Doctor[] found = manager.FindBySpeciality(spec);
                
                if (found.Length == 0)
                {
                    Console.WriteLine("Лікарів такої спеціальності не знайдено.");
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
                Console.Write("Введіть ID лікаря для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool success = manager.Remove(id);
                    Console.WriteLine(success ? "Лікаря успішно видалено." : "Лікаря з таким ID не знайдено.");
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
            else if (choice == "5")
            {
                Console.Write("Введіть ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Введіть прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Спеціальність: ");
                string spec = Console.ReadLine()!;
                
                manager.Add(new Doctor(firstName, lastName, spec));
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

    // ==========================================
    // ПІДМЕНЮ: ЗАПИСИ НА ПРИЙОМ
    // ==========================================
    static void AppointmentMenu(AppointmentManager appManager, PatientManager pManager, DoctorManager dManager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню: Записи на прийом ---");
            Console.WriteLine("1. Показати майбутні записи");
            Console.WriteLine("2. Створити новий запис");
            Console.WriteLine("3. Завершити прийом (Completed)");
            Console.WriteLine("4. Скасувати прийом (Cancelled)");
            Console.WriteLine("5. Знайти всі записи конкретного пацієнта");
            Console.WriteLine("0. Назад до головного меню");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\n--- Майбутні записи ---");
                appManager.DisplayList(appManager.GetUpcoming());
            }
            else if (choice == "2")
            {
                // Підказка користувачу: виводимо доступні ID
                Console.WriteLine("\n[Довідка] Список пацієнтів:");
                pManager.DisplayAll();
                Console.WriteLine("[Довідка] Список лікарів:");
                dManager.DisplayAll();

                Console.Write("Введіть ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pId))
                {
                    Console.WriteLine("Некоректний ID пацієнта.");
                    continue;
                }
                
                Console.Write("Введіть ID лікаря: ");
                if (!int.TryParse(Console.ReadLine(), out int dId))
                {
                    Console.WriteLine("Некоректний ID лікаря.");
                    continue;
                }

                Console.Write("Введіть дату та час (наприклад, 10.05.2026 14:00): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                {
                    Console.Write("Тривалість у хвилинах (натисніть Enter для 30 хв): ");
                    string durationStr = Console.ReadLine()!;
                    int duration = 30;
                    if (!string.IsNullOrWhiteSpace(durationStr))
                    {
                        int.TryParse(durationStr, out duration);
                    }

                    appManager.Book(pId, dId, dt, duration);
                }
                else
                {
                    Console.WriteLine("Некоректний формат дати або часу.");
                }
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ID запису для його завершення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool success = appManager.Complete(id);
                    Console.WriteLine(success ? $"Запис [{id}] успішно переведено у статус Completed." : "Помилка: запис не знайдено або він вже не в статусі Scheduled.");
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.Write("Причина скасування (можна залишити порожнім): ");
                    string reason = Console.ReadLine()!;
                    bool success = appManager.Cancel(id, reason);
                    Console.WriteLine(success ? $"Запис [{id}] скасовано." : "Помилка: запис не знайдено або він вже не в статусі Scheduled.");
                }
            }
            else if (choice == "5")
            {
                Console.Write("Введіть ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int pId))
                {
                    Appointment[] patientApps = appManager.GetByPatient(pId);
                    Console.WriteLine($"\n--- Записи пацієнта #{pId} ---");
                    appManager.DisplayList(patientApps);
                }
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