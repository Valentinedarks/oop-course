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

    public int WorkStartHour { get; set; }
    public int WorkEndHour { get; set; }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    public int WorkingHoursPerDay
    {
        get { return WorkEndHour - WorkStartHour; }
    }

    public string WorkSchedule
    {
        get { return $"{WorkStartHour:D2}:00-{WorkEndHour:D2}:00"; }
    }

    public bool IsAvailableNow
    {
        get { return CanAcceptAt(DateTime.Now.Hour); }
    }
    
    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        WorkStartHour = 8;
        WorkEndHour = 17;
    }

    public Doctor(string firstName, string lastName, Speciality speciality) 
        : this(firstName, lastName, speciality, "Невідомо", "0000000000")
    {
    }

    // Делегуємо з Speciality.General[cite: 8]
    public Doctor() 
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return hour >= WorkStartHour && hour < WorkEndHour;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Тел: {Phone} | {WorkSchedule} ({WorkingHoursPerDay} год) | {status}";
    }
}