namespace ClinicApp;

public class Doctor
{
    private static int _nextId = 1;
    public int Id { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Speciality Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }

    // Нове поле замість WorkStartHour та WorkEndHour[cite: 15]
    public WorkSchedule Schedule { get; set; }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    // Звернення до властивості структури[cite: 16]
    public bool IsAvailableNow
    {
        get { return Schedule.IsNow; }
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        
        // Ініціалізація структури[cite: 15]
        Schedule = new WorkSchedule(8, 17);
    }

    public Doctor(string firstName, string lastName, Speciality speciality) 
        : this(firstName, lastName, speciality, "Невідомо", "0000000000")
    {
    }

    public Doctor() 
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    // Делегування методу структури[cite: 16]
    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        // Schedule автоматично викличе свій ToString(), який повертає "08:00-17:00 (9 год)"[cite: 15]
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Тел: {Phone} | {Schedule} | {status}";
    }
}