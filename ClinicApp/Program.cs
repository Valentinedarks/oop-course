using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using ClinicApp;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Comparators;
using ClinicApp.Utils;

class Program
{
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), BloodType.APositive,
            "0501234567"));
        clinic.Patients.Add(
            new Patient("Олена", "Коваль", new DateTime(1993, 8, 24), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(
            new Patient("Максим", "Бойко", new DateTime(2010, 2, 10), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Валентин", "Руснак", new DateTime(2008, 10, 15), BloodType.OPositive,
            "0933456789"));

        clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567")
            { Schedule = new WorkSchedule(8, 16) });
        clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678")
            { Schedule = new WorkSchedule(9, 18) });
        clinic.Doctors.Add(new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789")
            { Schedule = new WorkSchedule(8, 17) });

        clinic.Appointments.Book(1, 1, DateTime.Now.AddDays(1).AddHours(2), 30);
        clinic.Appointments.BookUrgent(2, 2, DateTime.Now.AddDays(2).AddHours(3), "біль у грудях", 30);
        clinic.Appointments.BookSpecialist(3, 3, DateTime.Now.AddDays(3).AddHours(4), "кардіологія", 45);

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба",
            true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт",
            false));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Гемоглобін", 145, "г/л", "120-160", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30,
            "1 раз на добу вранці"));

        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10,
            "3 рази на день"));
        clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-15), "M54.5", "Люмбаго", false));

        clinic.MedicalRecords.Add(new LabResult(3, 3, DateTime.Today.AddDays(-2), "Глюкоза", 5.1, "ммоль/л", "3.3-5.5",
            true));
        
        if (clinic.Session.Exists())
        {
            Console.Write("Знайдено збережену сесію. Завантажити? (y/n): ");
            string? loadChoice = Console.ReadLine();
            if (loadChoice != null && loadChoice.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                int loaded = clinic.Session.Load(clinic);
                Console.WriteLine($"Завантажено {loaded} пацієнтів.");
                clinic.Logger.LogInfo($"Завантажено сесію ({loaded} нових пацієнтів).");
            }
        }

        Console.WriteLine("\n== new vs override ==");
        UrgentAppointment urgentApp = new UrgentAppointment(1, 1, DateTime.Today.AddDays(1).AddHours(10), "тест");
        Appointment baseApp = urgentApp;

        Console.WriteLine($"Через Appointment:       {baseApp.GetDescription()} | пріоритет {baseApp.GetPriority()}");
        Console.WriteLine(
            $"Через UrgentAppointment: {urgentApp.GetDescription()} | пріоритет {urgentApp.GetPriority()}");

        while (true)
        {
            Console.WriteLine("+----------------------------------------------+");
            Console.WriteLine("|               МЕДИЧНА КЛІНІКА                |");
            Console.WriteLine("+----------------------------------------------+");
            Console.WriteLine("| 1. Пацієнти         - реєстрація, пошук      |");
            Console.WriteLine("| 2. Лікарі           - персонал, розклад      |");
            Console.WriteLine("| 3. Записи           - прийоми, скасування    |");
            Console.WriteLine("| 4. Медична картка   - діагнози, рецепти      |");
            Console.WriteLine("| 5. Рахунки          - оплата, борги          |");
            Console.WriteLine("| 6. Черга            - очікування, прийом     |");
            Console.WriteLine("| 7. Звіт             - загальна статистика    |");
            Console.WriteLine("| 8. Аналітика        - звіти та рейтинги      |");
            Console.WriteLine("| 9. Плани лікування  - рефлексія, атрибути    |");
            Console.WriteLine("| 10. Файли           - журнал подій           |");
            Console.WriteLine("| 0. Вийти                                     |");
            Console.WriteLine("+----------------------------------------------+");
            Console.Write("Виберіть опцію: ");

            string? choice = Console.ReadLine();

            if (choice == "1") PatientMenu(clinic);
            else if (choice == "2") DoctorMenu(clinic);
            else if (choice == "3") AppointmentMenu(clinic);
            else if (choice == "4") MedicalRecordsMenu(clinic);
            else if (choice == "5") BillingMenu(clinic);
            else if (choice == "6") WaitingRoomMenu(clinic);
            else if (choice == "7") clinic.GenerateReport();
            else if (choice == "8") AnalyticsMenu(clinic);
            else if (choice == "9") TreatmentPlansMenu(clinic);
            else if (choice == "10") FilesMenu(clinic);
            else if (choice == "0")
            {
                Console.Write("Зберегти сесію? (y/n): ");
                string? saveChoice = Console.ReadLine();
                if (saveChoice != null && saveChoice.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    clinic.Session.Save(clinic);
                    Console.WriteLine("Сесію збережено.");
                    clinic.Logger.LogInfo("Сесію збережено перед виходом.");
                }
                
                Console.WriteLine("Роботу завершено. Навседобре!");
                break;
            }
        }

        static void FilesMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Файли --------------------");
                Console.WriteLine("1. Останні рядки лога");
                Console.WriteLine("2. Очистити лог");
                Console.WriteLine("3. Експортувати всі звіти");
                Console.WriteLine("4. Імпорт пацієнтів з CSV");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Скільки останніх рядків показати (N): ");
                    if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
                    {
                        Console.WriteLine("Некоректне число.");
                        continue;
                    }

                    string[] lines = clinic.Logger.GetLastLines(n);
                    if (lines.Length == 0)
                    {
                        Console.WriteLine("Лог порожній або файл ще не створено.");
                    }
                    else
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            Console.WriteLine(lines[i]);
                        }
                    }
                }
                else if (choice == "2")
                {
                    clinic.Logger.Clear();
                    Console.WriteLine("Лог очищено.");
                }
                else if (choice == "3")
                {
                    string[] paths = clinic.Exporter.ExportAll();
                    Console.WriteLine("Звіти експортовано:");
                    for (int i = 0; i < paths.Length; i++)
                    {
                        Console.WriteLine($"  {paths[i]}");
                    }
                    clinic.Logger.LogInfo("Експортовано всі звіти у файли.");
                }
                else if (choice == "4")
                {
                    Console.Write("Шлях до CSV-файлу: ");
                    string filePath = Console.ReadLine() ?? "";
                    CsvImporter importer = new CsvImporter();
                    ImportResult result = importer.ImportPatients(clinic, filePath);
                    result.Print();
                    clinic.Logger.LogInfo($"Імпорт CSV ({filePath}): імпортовано {result.Imported}, пропущено {result.Skipped}.");
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

        static void TreatmentPlansMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Плани лікування --------------------");
                Console.WriteLine("1. Показати всі плани");
                Console.WriteLine("2. Додати план лікування");
                Console.WriteLine("3. Плани пацієнта");
                Console.WriteLine("4. Активувати план");
                Console.WriteLine("5. Завершити план");
                Console.WriteLine("6. Скасувати план");
                Console.WriteLine("7. Інформація про тип TreatmentPlan");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    TreatmentPlan[] plans = clinic.TreatmentPlans.GetAll();
                    if (plans.Length == 0)
                    {
                        Console.WriteLine("Планів лікування немає.");
                    }
                    else
                    {
                        for (int i = 0; i < plans.Length; i++)
                        {
                            Console.WriteLine(plans[i]);
                        }
                    }
                }
                else if (choice == "2")
                {
                    TreatmentPlan plan = FormBuilder.Build<TreatmentPlan>();
                    if (clinic.TreatmentPlans.Add(plan))
                    {
                        Console.WriteLine($"План лікування [{plan.Id}] додано.");
                        clinic.Logger.LogInfo($"Додано план лікування #{plan.Id} для пацієнта #{plan.PatientId}.");
                    }
                }
                else if (choice == "3")
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Некоректний формат ID.");
                        continue;
                    }

                    TreatmentPlan[] plans = clinic.TreatmentPlans.GetByPatient(pId);
                    if (plans.Length == 0)
                    {
                        Console.WriteLine("Планів для цього пацієнта не знайдено.");
                    }
                    else
                    {
                        for (int i = 0; i < plans.Length; i++)
                        {
                            Console.WriteLine(plans[i]);
                        }
                    }
                }
                else if (choice == "4")
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Некоректний формат ID.");
                        continue;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(id);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Activate())
                    {
                        Console.WriteLine($"План [{id}] активовано.");
                    }
                    else
                    {
                        Console.WriteLine("Неможливо активувати план (активувати можна лише план у статусі Planned).");
                    }
                }
                else if (choice == "5")
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Некоректний формат ID.");
                        continue;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(id);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Complete())
                    {
                        Console.WriteLine($"План [{id}] завершено.");
                    }
                    else
                    {
                        Console.WriteLine("Неможливо завершити план (завершити можна лише активний план).");
                    }
                }
                else if (choice == "6")
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Некоректний формат ID.");
                        continue;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(id);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Cancel())
                    {
                        Console.WriteLine($"План [{id}] скасовано.");
                    }
                    else
                    {
                        Console.WriteLine("Неможливо скасувати план (він уже завершений або скасований).");
                    }
                }
                else if (choice == "7")
                {
                    ModelValidator.PrintInfo(typeof(TreatmentPlan));
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

        static List<DoctorStats> CollectDoctorStats(Clinic clinic)
        {
            List<DoctorStats> list = new List<DoctorStats>();
            foreach (DoctorStats stat in clinic.Analytics.ComputeDoctorStats())
            {
                list.Add(stat);
            }
            return list;
        }

        static List<PatientStats> CollectPatientStats(Clinic clinic)
        {
            List<PatientStats> list = new List<PatientStats>();
            foreach (PatientStats stat in clinic.Analytics.ComputePatientStats())
            {
                list.Add(stat);
            }
            return list;
        }

        static void AnalyticsMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Аналітика --------------------");
                Console.WriteLine("1. Лікарі за навантаженням");
                Console.WriteLine("2. Лікарі за виручкою");
                Console.WriteLine("3. Лікарі за іменем");
                Console.WriteLine("4. Пацієнти за кількістю візитів");
                Console.WriteLine("5. Пацієнти за витратами");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort();
                    foreach (DoctorStats s in stats) Console.WriteLine(s);
                }
                else if (choice == "2")
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort(new DoctorStatsByRevenue());
                    foreach (DoctorStats s in stats) Console.WriteLine(s);
                }
                else if (choice == "3")
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort(new DoctorStatsByName());
                    foreach (DoctorStats s in stats) Console.WriteLine(s);
                }
                else if (choice == "4")
                {
                    List<PatientStats> stats = CollectPatientStats(clinic);
                    stats.Sort();
                    foreach (PatientStats s in stats) Console.WriteLine(s);
                }
                else if (choice == "5")
                {
                    List<PatientStats> stats = CollectPatientStats(clinic);
                    stats.Sort(new PatientStatsBySpent());
                    foreach (PatientStats s in stats) Console.WriteLine(s);
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

        static void WaitingRoomMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Черга --------------------");
                Console.WriteLine("1. Додати пацієнта до черги");
                Console.WriteLine("2. Прийняти першого");
                Console.WriteLine("3. Хто перший?");
                Console.WriteLine("4. Переглянути всю чергу");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Введіть ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Patient? patient = clinic.Patients.FindById(pId);
                        if (patient != null)
                        {
                            clinic.WaitingRoom.Enqueue(patient);
                            Console.WriteLine($"Пацієнта {patient.FullName} додано до черги.");
                        }
                        else
                        {
                            Console.WriteLine("Пацієнта не знайдено.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некоректний формат ID.");
                    }
                }
                else if (choice == "2")
                {
                    try
                    {
                        Patient first = clinic.WaitingRoom.Dequeue();
                        Console.WriteLine($"Прийнято: {first.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                        clinic.Logger.LogWarning($"Черга: {ex.Message}");
                    }
                }
                else if (choice == "3")
                {
                    try
                    {
                        Patient first = clinic.WaitingRoom.Peek();
                        Console.WriteLine($"Перший у черзі: {first.FullName}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                        clinic.Logger.LogWarning($"Черга: {ex.Message}");
                    }
                }
                else if (choice == "4")
                {
                    Patient[] queueArr = clinic.WaitingRoom.ToArray();
                    if (queueArr.Length == 0)
                    {
                        Console.WriteLine("Черга порожня.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== Поточний стан черги ===");
                        for (int i = 0; i < queueArr.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {queueArr[i].FullName}");
                        }
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

        static void BillingMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Рахунки --------------------");
                Console.WriteLine("1. Борги пацієнта");
                Console.WriteLine("2. Всі неоплачені записи");
                Console.WriteLine("3. Оплатити запис");
                Console.WriteLine("4. Загальний борг клініки");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        continue;
                    }

                    Console.WriteLine($"Неоплачені записи пацієнта #{pId}:");
                    IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(pId);
                    clinic.Billing.DisplayUnpaid(unpaid);

                    decimal debt = clinic.Billing.GetPatientDebt(pId);
                    Console.WriteLine($"Борг: {debt:F2} грн");
                }
                else if (choice == "2")
                {
                    IPayable[] unpaid = clinic.Billing.GetAllUnpaid();
                    clinic.Billing.DisplayUnpaid(unpaid);
                }
                else if (choice == "3")
                {
                    Console.Write("ID запису для оплати: ");
                    if (!int.TryParse(Console.ReadLine(), out int appId))
                    {
                        continue;
                    }

                    if (clinic.Billing.PayAppointment(appId))
                    {
                        Console.WriteLine($"Запис [{appId}] оплачено.");
                        clinic.Logger.LogInfo($"Оплачено запис #{appId}.");
                    }
                    else
                    {
                        Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                    }
                }
                else if (choice == "4")
                {
                    decimal total = clinic.Billing.GetTotalDebt();
                    Console.WriteLine($"Загальний борг по клініці: {total:F2} грн");
                }
                else if (choice == "0")
                {
                    break;
                }
            }
        }

        static void DoctorMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню: Лікарі ---");
                Console.WriteLine("1. Показати всіх лікарів");
                Console.WriteLine("2. Знайти лікаря за спеціальністю");
                Console.WriteLine("3. Видалити лікаря за ID");
                Console.WriteLine("4. Статистика лікарів");
                Console.WriteLine("5. Вільні години лікаря");
                Console.WriteLine("6. Додати нового лікаря (базовий запис)");
                Console.WriteLine("0. Назад до головного меню");
                Console.Write("Виберіть опцію: ");

                string? choice = Console.ReadLine();

                if (choice == "1") clinic.Doctors.DisplayAll();
                else if (choice == "2")
                {
                    Console.Write("Введіть спеціальність англійською (наприклад Cardiology, Surgery): ");
                    string specInput = Console.ReadLine()!;

                    if (Enum.TryParse<Speciality>(specInput, true, out Speciality spec) &&
                        Enum.IsDefined(typeof(Speciality), spec))
                    {
                        Doctor[] found = clinic.Doctors.FindBySpeciality(spec);

                        if (found.Length == 0) Console.WriteLine("Лікарів такої спеціальності не знайдено.");
                        else
                        {
                            Console.WriteLine($"\nЗнайдено ({found.Length}):");
                            for (int i = 0; i < found.Length; i++) Console.WriteLine(found[i].ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невідома спеціальність. Спробуйте ще раз.");
                    }
                }
                else if (choice == "3")
                {
                    Console.Write("Введіть ID лікаря для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                        Console.WriteLine(clinic.Doctors.Remove(id)
                            ? "Лікаря успішно видалено."
                            : "Лікаря з таким ID не знайдено.");
                    else Console.WriteLine("Некоректний формат ID.");
                }
                else if (choice == "4") clinic.Doctors.DisplayStats();
                else if (choice == "5")
                {
                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        continue;
                    }

                    Doctor? doc = clinic.Doctors.FindById(dId);
                    if (doc == null)
                    {
                        Console.WriteLine("Лікаря не знайдено.");
                        continue;
                    }

                    ISchedulable schedulable = doc;

                    Console.Write("Дата (dd.MM.yyyy): ");
                    string dateStr = Console.ReadLine()!;
                    if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime date))
                    {
                        continue;
                    }

                    Console.Write("Скільки слотів: ");
                    if (!int.TryParse(Console.ReadLine(), out int slotCount))
                    {
                        continue;
                    }

                    try
                    {
                        DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
                        for (int i = 0; i < slots.Length; i++)
                        {
                            Console.WriteLine(slots[i].ToString("HH:mm"));
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "6")
                {
                    try
                    {
                        Console.Write("Введіть ім'я: ");
                        string firstName = Console.ReadLine()!;
                        Console.Write("Введіть прізвище: ");
                        string lastName = Console.ReadLine()!;

                        Console.Write(
                            "Спеціальність (General, Cardiology, Neurology, Pediatrics, Surgery, Orthopedics, Dermatology, Emergency): ");
                        string specInput = Console.ReadLine()!;

                        if (Enum.TryParse<Speciality>(specInput, true, out Speciality spec) &&
                            Enum.IsDefined(typeof(Speciality), spec))
                        {
                            Console.Write("Номер ліцензії: ");
                            string license = Console.ReadLine()!;
                            Console.Write("Введіть телефон (10 цифр): ");
                            string phone = Console.ReadLine()!;

                            Doctor newDoctor = new Doctor(firstName, lastName, spec, license, phone);

                            Console.Write("Початок роботи (година, 0-23): ");
                            int startHour = 8;
                            if (int.TryParse(Console.ReadLine(), out int s)) startHour = s;

                            Console.Write("Кінець роботи (година, 1-24): ");
                            int endHour = 17;
                            if (int.TryParse(Console.ReadLine(), out int e)) endHour = e;

                            newDoctor.Schedule = new WorkSchedule(startHour, endHour);

                            clinic.Doctors.Add(newDoctor);
                            clinic.Logger.LogInfo($"Додано лікаря #{newDoctor.Id}: {newDoctor.FullName}");
                        }
                        else
                        {
                            Console.WriteLine("Помилка: невідома спеціальність. Лікаря не додано.");
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "0") break;
                else Console.WriteLine("Невідома команда.");
            }
        }

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
                Console.WriteLine("8. Скасувати всі записи пацієнта");
                Console.WriteLine("9. За типом прийому");
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
                    try
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

                            if (clinic.Appointments.Book(pId, dId, dt, duration))
                            {
                                clinic.Logger.LogInfo($"Запис створено: пацієнт #{pId} -> лікар #{dId}");
                            }
                        }
                        else Console.WriteLine("Некоректний формат дати або часу.");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "3")
                {
                    Console.Write("Введіть ID запису для його завершення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Appointments.Complete(id))
                        {
                            Console.WriteLine($"Запис [{id}] успішно переведено у статус Completed.");
                            clinic.Logger.LogInfo($"Запис #{id} завершено.");
                        }
                        else
                        {
                            Console.WriteLine("Помилка: запис не знайдено або він вже не в статусі Scheduled.");
                        }
                    }
                }
                else if (choice == "4")
                {
                    Console.Write("Введіть ID запису для скасування: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.Write("Причина скасування (можна залишити порожнім): ");
                        string reason = Console.ReadLine()!;
                        if (clinic.Appointments.Cancel(id, reason))
                        {
                            Console.WriteLine($"Запис [{id}] скасовано.");
                            clinic.Logger.LogInfo($"Запис #{id} скасовано.");
                        }
                        else
                        {
                            Console.WriteLine("Помилка: запис не знайдено або він вже не в статусі Scheduled.");
                        }
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
                else if (choice == "8")
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        continue;
                    }

                    Console.Write("Причина (Enter - без причини): ");
                    string reason = Console.ReadLine()!;

                    Appointment[] patientApps = clinic.Appointments.GetByPatient(pId);
                    int count = AppointmentManager.CancelAll(patientApps, reason);
                    Console.WriteLine($"Скасовано записів: {count}");
                    if (count > 0)
                    {
                        clinic.Logger.LogInfo($"Скасовано {count} записів пацієнта #{pId}.");
                    }
                }
                else if (choice == "9")
                {
                    Console.WriteLine("\n1. Термінові");
                    Console.WriteLine("2. Консультації спеціаліста");
                    Console.WriteLine("3. Ззвичайні");
                    Console.Write("Оберіть: ");

                    string? subChoice = Console.ReadLine();
                    if (subChoice == "1")
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetUrgent());
                    }
                    else if (subChoice == "2")
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetSpecialist());
                    }
                    else if (subChoice == "3")
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetRegular());
                    }
                    else
                    {
                        Console.WriteLine("Невідома опція.");
                    }
                }
                else if (choice == "0") break;
                else Console.WriteLine("Невідома команда.");
            }
        }

        static void MedicalRecordsMenu(Clinic clinic)
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню: Медична картка ---");
                Console.WriteLine("1. Картка пацієнта (зведення)");
                Console.WriteLine("2. Усі записи пацієнта");
                Console.WriteLine("3. Додати діагноз");
                Console.WriteLine("4. Додати аналіз");
                Console.WriteLine("5. Додати рецепт");
                Console.WriteLine("6. Записи лікаря");
                Console.WriteLine("0. Назад до головного меню");
                Console.Write("Виберіть опцію: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Введіть ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Patients.FindById(pId) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
                        continue;
                    }

                    clinic.MedicalRecords.DisplayPatientSummary(pId);
                }
                else if (choice == "2")
                {
                    Console.Write("Введіть ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Patients.FindById(pId) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
                        continue;
                    }

                    MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(pId);
                    clinic.MedicalRecords.DisplayList(records);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("\n[Довідка] Список пацієнтів:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("[Довідка] Список лікарів:");
                    clinic.Doctors.DisplayAll();

                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Patients.FindById(pId) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Doctors.FindById(dId) == null)
                    {
                        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("Код діагнозу (напр. J06.9): ");
                    string code = Console.ReadLine()!;

                    Console.Write("Опис: ");
                    string desc = Console.ReadLine()!;

                    Console.Write("Хронічне? (1=так, 0=ні): ");
                    string chronicInput = Console.ReadLine()!;
                    bool isChronic = chronicInput == "1";

                    try
                    {
                        Diagnosis diagnosis = new Diagnosis(pId, dId, DateTime.Today, code, desc, isChronic);
                        clinic.MedicalRecords.Add(diagnosis);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "4")
                {
                    Console.WriteLine("\n[Довідка] Список пацієнтів:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("[Довідка] Список лікарів:");
                    clinic.Doctors.DisplayAll();

                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Patients.FindById(pId) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Doctors.FindById(dId) == null)
                    {
                        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("Назва аналізу: ");
                    string testName = Console.ReadLine()!;

                    Console.Write("Значення (число): ");
                    string valStr = Console.ReadLine()!;
                    if (!double.TryParse(valStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    Console.Write("Одиниці виміру: ");
                    string unit = Console.ReadLine()!;

                    Console.Write("Норма (напр. 4.0–9.0): ");
                    string range = Console.ReadLine()!;

                    Console.Write("В нормі? (1=так, 0=ні): ");
                    string normalInput = Console.ReadLine()!;
                    bool isNormal = normalInput == "1";

                    try
                    {
                        LabResult labResult = new LabResult(pId, dId, DateTime.Today, testName, value, unit, range,
                            isNormal);
                        clinic.MedicalRecords.Add(labResult);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "5")
                {
                    Console.WriteLine("\n[Довідка] Список пацієнтів:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("[Довідка] Список лікарів:");
                    clinic.Doctors.DisplayAll();

                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Patients.FindById(pId) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Doctors.FindById(dId) == null)
                    {
                        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
                        continue;
                    }

                    Console.Write("Препарат: ");
                    string medName = Console.ReadLine()!;

                    Console.Write("Дозування (10 мг): ");
                    string dosage = Console.ReadLine()!;

                    Console.Write("Кількість днів: ");
                    if (!int.TryParse(Console.ReadLine(), out int days))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    Console.Write("Інструкція (Enter — пропустити): ");
                    string instructions = Console.ReadLine()!;

                    try
                    {
                        Prescription rx = new Prescription(pId, dId, DateTime.Today, medName, dosage, days,
                            instructions);
                        clinic.MedicalRecords.Add(rx);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                }
                else if (choice == "6")
                {
                    Console.Write("Введіть ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }

                    if (clinic.Doctors.FindById(dId) == null)
                    {
                        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
                        continue;
                    }

                    MedicalRecord[] records = clinic.MedicalRecords.GetByDoctor(dId);
                    clinic.MedicalRecords.DisplayList(records);
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
                    try
                    {
                        Console.Write("Введіть ім'я: ");
                        string firstName = Console.ReadLine()!;
                        Console.Write("Введіть прізвище: ");
                        string lastName = Console.ReadLine()!;

                        Console.Write("Введіть дату народження (наприклад, 15.10.2008): ");
                        DateTime dob = DateTime.Now;
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime parsedDate))
                        {
                            dob = parsedDate;
                        }

                        Console.Write("Група крові (Unknown, APositive, ANegative...): ");
                        string bloodInput = Console.ReadLine()!;
                        BloodType bloodType = BloodType.Unknown;
                        if (Enum.TryParse<BloodType>(bloodInput, true, out BloodType bt) &&
                            Enum.IsDefined(typeof(BloodType), bt))
                        {
                            bloodType = bt;
                        }

                        Console.Write("Введіть телефон (10 цифр): ");
                        string phone = Console.ReadLine()!;

                        Patient newPatient = new Patient(firstName, lastName, dob, bloodType, phone);
                        clinic.Patients.Add(newPatient);
                        clinic.Logger.LogInfo($"Додано пацієнта #{newPatient.Id}: {newPatient.FullName}");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                        clinic.Logger.LogWarning($"Помилка введення: {e.Message}");
                    }
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
                        Console.WriteLine(clinic.Patients.Remove(id)
                            ? "Пацієнта успішно видалено."
                            : "Пацієнта з таким ID не знайдено.");
                    else Console.WriteLine("Некоректний формат ID.");
                }
                else if (choice == "5") clinic.Patients.DisplayStats();
                else if (choice == "0") break;
                else Console.WriteLine("Невідома команда.");
            }
        }
    }
}