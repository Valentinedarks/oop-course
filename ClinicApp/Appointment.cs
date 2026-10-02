namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    // Властивості лише для читання (встановлюються один раз у конструкторі)[cite: 15]
    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }

    // Змінювати статус та примітки можна лише зсередини класу[cite: 16]
    public string Status { get; private set; }
    public string Notes { get; private set; }

    // Обчислювані властивості[cite: 15]
    public DateTime EndsAt
    {
        get { return ScheduledAt.AddMinutes(DurationMinutes); }
    }

    public bool IsUpcoming
    {
        get { return ScheduledAt > DateTime.Now && Status == "Scheduled"; }
    }

    // Конструктор з параметром за замовчуванням для тривалості (30 хвилин)[cite: 15, 16]
    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        
        // Початковий стан кінцевого автомата[cite: 15]
        Status = "Scheduled";
        Notes = ""; // Ініціалізація порожнім рядком (ne nullable)[cite: 16]
    }

    // Метод скасування запису[cite: 15]
    // reason - необов'язковий параметр із порожнім рядком за замовчуванням[cite: 16]
    public bool Cancel(string reason = "")
    {
        if (Status == "Scheduled")
        {
            Status = "Cancelled";
            Notes = reason;
            return true;
        }
        return false;
    }

    // Метод завершення прийому[cite: 15]
    public bool Complete()
    {
        if (Status == "Scheduled")
        {
            Status = "Completed";
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        // Форматування дати та часу (напр., 09.05.2026 10:00-10:30)[cite: 15]
        string baseInfo = $"[{Id}] Пацієнт #{PatientId} -> Лікар #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";
        
        // Додаємо примітки до виводу лише якщо вони існують[cite: 16]
        if (Notes.Length > 0)
        {
            return baseInfo + $" | {Notes}";
        }
        
        return baseInfo;
    }
}