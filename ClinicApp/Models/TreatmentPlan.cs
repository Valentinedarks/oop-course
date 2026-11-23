using ClinicApp.Attributes;
using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class TreatmentPlan : IIdentifiable
{
    private static int _nextId = 1;

    public int Id { get; }

    [MinValue(1, "Patient ID is required.")]
    public int PatientId { get; set; }

    [Required("Diagnosis cannot be empty.")]
    [MaxLength(200, "Diagnosis must not exceed 200 characters.")]
    public string Diagnosis { get; set; } = "";

    [MinValue(1, "Duration must be at least 1 day.")]
    public int DurationDays { get; set; }

    public TreatmentStatus Status { get; private set; }

    public TreatmentPlan()
    {
        Id = _nextId++;
        Status = TreatmentStatus.Planned;
    }

    public bool Activate()
    {
        if (Status == TreatmentStatus.Planned)
        {
            Status = TreatmentStatus.Active;
            return true;
        }
        return false;
    }

    public bool Complete()
    {
        if (Status == TreatmentStatus.Active)
        {
            Status = TreatmentStatus.Completed;
            return true;
        }
        return false;
    }

    public bool Cancel()
    {
        if (Status != TreatmentStatus.Completed && Status != TreatmentStatus.Cancelled)
        {
            Status = TreatmentStatus.Cancelled;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"[{Id}] Пацієнт #{PatientId} | {Diagnosis} | Тривалість: {DurationDays} дн. | Статус: {Status}";
    }
}