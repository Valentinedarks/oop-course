using System;

namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        // Створюємо єдиний об'єкт клініки-оркестратора
        Clinic clinic = new Clinic("Медична Клініка");

        // --- Ініціалізація базових тестових даних ---
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 2, 10), "O+", "0933456789"));
        clinic.Patients.Add(new Patient("Валентин", "Руснак", new DateTime(2008, 10, 15), "O+", "0933456789"));

        clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567") { WorkStartHour = 8, WorkEndHour = 16 });
        clinic.Doctors.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678") { WorkStartHour = 9, WorkEndHour = 18 });
        clinic.Doctors.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789") { WorkStartHour = 8, WorkEndHour = 17 });

        clinic.Appointments.Book(1, 1, DateTime.Now.AddDays(1).AddHours(2), 30);
        clinic.Appointments.Book(2, 2, DateTime.Now.AddDays(2).AddHours(3), 45);

        // --- Головний цикл програми ---
        while (true)
        {
            Console.WriteLine($"\n=== {clinic.Name.ToUpper()}: ГОЛОВНЕ МЕНЮ ===");
            Console.WriteLine("1. Керування пацієнтами (статичний масив)");
            Console.WriteLine("2. Керування лікарями");
            Console.WriteLine("3. Керування записами на прийом");
            Console.WriteLine("4. Розклад на конкретну дату");
            Console.WriteLine("5. Згенерувати звіт клініки");
            Console.WriteLine("6. Тест GrowablePatientManager (Задача 8)");
            Console.WriteLine("0. Вихід з програми");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1") PatientMenu(clinic);
            else if (choice == "2") DoctorMenu(clinic);
            else if (choice == "3") AppointmentMenu(clinic);
            else if (choice == "4")
            {
                Console.Write("Введіть дату (наприклад, 10.05.2026): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    clinic.DisplaySchedule(date);
                }
                else
                {
                    Console.WriteLine("Некоректний формат дати.");
                }
            }
            else if (choice == "5")
            {
                clinic.GenerateReport();
            }
            else if (choice == "6")
            {
                TestGrowableArray();
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
    // ТЕСТ ЗАДАЧІ 8: ДИНАМІЧНИЙ МАСИВ
    // ==========================================
    static void TestGrowableArray()
    {
        Console.WriteLine("\n=== Тест GrowablePatientManager ===");
        GrowablePatientManager dynamicManager = new GrowablePatientManager();
        
        Console.WriteLine("Додаємо пацієнтів одного за одним ...");
        // У циклі додаємо 20 пацієнтів, щоб побачити кілька розширень (4 -> 8 -> 16 -> 32)
        for (int i = 1; i <= 20; i++)
        {
            dynamicManager.Add(new Patient($"Тест", $"Пацієнт{i}"));
        }

        Console.WriteLine("\nТест пошуку:");
        Patient? p10 = dynamicManager.FindById(10);
        Console.WriteLine($"FindById(10) -> {(p10 != null ? p10.FullName : "не знайдено")}");

        Patient? p99 = dynamicManager.FindById(99);
        Console.WriteLine($"FindById(99) -> {(p99 != null ? p99.FullName : "не знайдено")}");

        Console.WriteLine("\nПорівняння:");
        Console.WriteLine("PatientManager:         100 місць (фіксовано)");
        Console.WriteLine($"GrowablePatientManager: {dynamicManager.Capacity} місця (зросте при потребі)");
    }

    // ==========================================
    // ПІДМЕНЮ: ПАЦІЄНТИ
    // ==========================================
    static void PatientMenu(Clinic clinic)
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

            if (choice == "1") clinic.Patients.DisplayAll();
            else if (choice == "2")
            {
                Console.Write("Введіть ім'я: "); string firstName = Console.ReadLine()!;
                Console.Write("Введіть прізвище: "); string lastName = Console.ReadLine()!;
                clinic.Patients.Add(new Patient(firstName, lastName));
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ім'я або прізвище для пошуку: ");
                string query = Console.ReadLine()!;
                Patient[] found = clinic.Patients.FindByName(query);
                
                if (found.Length == 0) Console.WriteLine("Пацієнтів не знайдено.");
                else
                {
                    Console.WriteLine($"\nЗнайдено ({found.Length}):");
                    for (int i = 0; i < found.Length; i++) Console.WriteLine(found[i].ToString());
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID пацієнта для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                    Console.WriteLine(clinic.Patients.Remove(id) ? "Пацієнта успішно видалено." : "Пацієнта з таким ID не знайдено.");
                else Console.WriteLine("Некоректний формат ID.");
            }
            else if (choice == "5") clinic.Patients.DisplayStats();
            else if (choice == "0") break;
            else Console.WriteLine("Невідома команда.");
        }
    }

    // ==========================================
    // ПІДМЕНЮ: ЛІКАРІ
    // ==========================================
    static void DoctorMenu(Clinic clinic)
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

            if (choice == "1") clinic.Doctors.DisplayAll();
            else if (choice == "2")
            {
                Console.Write("Введіть спеціальність для пошуку: ");
                string spec = Console.ReadLine()!;
                Doctor[] found = clinic.Doctors.FindBySpeciality(spec);
                
                if (found.Length == 0) Console.WriteLine("Лікарів такої спеціальності не знайдено.");
                else
                {
                    Console.WriteLine($"\nЗнайдено ({found.Length}):");
                    for (int i = 0; i < found.Length; i++) Console.WriteLine(found[i].ToString());
                }
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ID лікаря для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                    Console.WriteLine(clinic.Doctors.Remove(id) ? "Лікаря успішно видалено." : "Лікаря з таким ID не знайдено.");
                else Console.WriteLine("Некоректний формат ID.");
            }
            else if (choice == "4") clinic.Doctors.DisplayStats();
            else if (choice == "5")
            {
                Console.Write("Введіть ім'я: "); string firstName = Console.ReadLine()!;
                Console.Write("Введіть прізвище: "); string lastName = Console.ReadLine()!;
                Console.Write("Спеціальність: "); string spec = Console.ReadLine()!;
                clinic.Doctors.Add(new Doctor(firstName, lastName, spec));
            }
            else if (choice == "0") break;
            else Console.WriteLine("Невідома команда.");
        }
    }

    // ==========================================
    // ПІДМЕНЮ: ЗАПИСИ НА ПРИЙОМ
    // ==========================================
    static void AppointmentMenu(Clinic clinic)
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
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
            }
            else if (choice == "2")
            {
                Console.WriteLine("\n[Довідка] Список пацієнтів:");
                clinic.Patients.DisplayAll();
                Console.WriteLine("[Довідка] Список лікарів:");
                clinic.Doctors.DisplayAll();

                Console.Write("Введіть ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pId)) continue;
                
                Console.Write("Введіть ID лікаря: ");
                if (!int.TryParse(Console.ReadLine(), out int dId)) continue;

                Console.Write("Введіть дату та час (наприклад, 10.05.2026 14:00): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                {
                    Console.Write("Тривалість у хвилинах (натисніть Enter для 30 хв): ");
                    string durationStr = Console.ReadLine()!;
                    int duration = 30;
                    if (!string.IsNullOrWhiteSpace(durationStr)) int.TryParse(durationStr, out duration);
                    
                    clinic.Appointments.Book(pId, dId, dt, duration);
                }
                else Console.WriteLine("Некоректний формат дати або часу.");
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ID запису для його завершення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                    Console.WriteLine(clinic.Appointments.Complete(id) ? $"Запис [{id}] успішно переведено у статус Completed." : "Помилка: запис не знайдено або він вже не в статусі Scheduled.");
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.Write("Причина скасування (можна залишити порожнім): ");
                    string reason = Console.ReadLine()!;
                    Console.WriteLine(clinic.Appointments.Cancel(id, reason) ? $"Запис [{id}] скасовано." : "Помилка: запис не знайдено або він вже не в статусі Scheduled.");
                }
            }
            else if (choice == "5")
            {
                Console.Write("Введіть ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int pId))
                {
                    Appointment[] patientApps = clinic.Appointments.GetByPatient(pId);
                    Console.WriteLine($"\n--- Записи пацієнта #{pId} ---");
                    clinic.Appointments.DisplayList(patientApps);
                }
            }
            else if (choice == "0") break;
            else Console.WriteLine("Невідома команда.");
        }
    }
}