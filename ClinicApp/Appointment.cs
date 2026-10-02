namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }

    // Використовуємо enum замість string[cite: 5]
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt
    {
        get { return ScheduledAt.AddMinutes(DurationMinutes); }
    }

    public bool IsUpcoming
    {
        get { return ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled; }
    }

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        
        Status = AppointmentStatus.Scheduled; // Зміна на enum[cite: 5]
        Notes = ""; 
    }

    public bool Cancel(string reason = "")
    {
        if (Status == AppointmentStatus.Scheduled) // Зміна на enum[cite: 5]
        {
            Status = AppointmentStatus.Cancelled; // Зміна на enum[cite: 5]
            Notes = reason;
            return true;
        }
        return false;
    }

    public bool Complete()
    {
        if (Status == AppointmentStatus.Scheduled) // Зміна на enum[cite: 5]
        {
            Status = AppointmentStatus.Completed; // Зміна на enum[cite: 5]
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        string baseInfo = $"[{Id}] Пацієнт #{PatientId} -> Лікар #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";
        
        if (Notes.Length > 0)
        {
            return baseInfo + $" | {Notes}";
        }
        
        return baseInfo;
    }
}