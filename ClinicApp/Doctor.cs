namespace ClinicApp;

public class Doctor
{
    // Лічильник та автопризначений ID
    private static int _nextId = 1;
    public int Id { get; }

    // Основні поля[cite: 9]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }

    // Робочий графік (години від 0 до 23)[cite: 9, 10]
    public int WorkStartHour { get; set; }
    public int WorkEndHour { get; set; }

    // Обчислювані властивості[cite: 9]
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
        // Форматування годин з доповненням нулем (наприклад, 08:00) за допомогою D2[cite: 10]
        get { return $"{WorkStartHour:D2}:00-{WorkEndHour:D2}:00"; }
    }

    public bool IsAvailableNow
    {
        // Поточна година отримується через DateTime.Now і перевіряється методом CanAcceptAt[cite: 10]
        get { return CanAcceptAt(DateTime.Now.Hour); }
    }

    // Повний конструктор, який також виставляє графік 8–17[cite: 9, 10]
    public Doctor(string firstName, string lastName, string speciality, string licenseNumber, string phone)
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

    // Ланцюжок викликів: делегування повному конструктору[cite: 9, 10]
    public Doctor(string firstName, string lastName, string speciality) 
        : this(firstName, lastName, speciality, "Невідомо", "0000000000")
    {
    }

    // Значення за замовчуванням[cite: 9]
    public Doctor() 
        : this("Невідомий", "Лікар", "Загальна практика")
    {
    }

    // Метод перевірки доступності у вказану годину[cite: 9]
    public bool CanAcceptAt(int hour)
    {
        return hour >= WorkStartHour && hour < WorkEndHour;
    }

    // Перевизначення виводу[cite: 9]
    public override string ToString()
    {
        // Формування рядка з використанням готових властивостей[cite: 10]
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Тел: {Phone} | {WorkSchedule} ({WorkingHoursPerDay} год) | {status}";
    }
}