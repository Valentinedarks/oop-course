namespace ClinicApp;

public class Clinic
{
    public string Name { get; }

    // Публічні властивості-менеджери є get-only, ініціалізуються в конструкторі
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        
        // Порядок створення важливий: AppointmentManager потребує готових PatientManager та DoctorManager[cite: 20]
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");[cite: 19]
        // Делегування: беремо записи на дату і виводимо наявним методом[cite: 20]
        Appointment[] appsOnDate = Appointments.GetByDate(date);
        Appointments.DisplayList(appsOnDate);
    }

    public void GenerateReport()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");[cite: 20]
        Console.WriteLine($"║ Звіт — {Name,-47} ║");[cite: 19]
        Console.WriteLine("╠════════════════════════════════════════════════════════╣");
        Console.WriteLine($"║ Пацієнтів:          {Patients.Count,-34} ║");[cite: 19]
        Console.WriteLine($"║ Лікарів:            {Doctors.Count,-34} ║");[cite: 19]
        
        // Беремо майбутні записи один раз[cite: 20]
        Appointment[] upcoming = Appointments.GetUpcoming();
        Console.WriteLine($"║ Майбутніх записів:  {upcoming.Length,-34} ║");[cite: 19]
        
        Console.WriteLine("╠════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Навантаження лікарів (майбутні записи):                ║");[cite: 19]
        
        // Беремо всіх лікарів[cite: 20]
        Doctor[] allDoctors = Doctors.GetAll();
        
        // Вкладений цикл для підрахунку навантаження без LINQ[cite: 20]
        for (int i = 0; i < allDoctors.Length; i++)
        {
            int loadCount = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == allDoctors[i].Id)
                {
                    loadCount++;
                }
            }
            string docLine = $"  {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {loadCount} записів";
            Console.WriteLine($"║ {docLine,-54} ║");[cite: 19, 20]
        }
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");[cite: 20]
    }
}