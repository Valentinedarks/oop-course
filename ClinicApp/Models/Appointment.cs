using System;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Appointment : IPayable
{
    private static int _nextId = 1;

    private int _durationMinutes;
    private bool _isPaid = false;

    public int Id { get; private set; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }

    public int DurationMinutes
    {
        get { return _durationMinutes; }
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public bool IsPaid
    {
        get { return _isPaid; }
    }

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
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        
        Status = AppointmentStatus.Scheduled;
        Notes = ""; 
        
        Id = _nextId++;
    }

    public decimal GetCost()
    {
        return DurationMinutes * 10m;
    }

    public void MarkPaid()
    {
        if (Status != AppointmentStatus.Cancelled)
        {
            _isPaid = true;
        }
    }

    public bool Cancel(string reason = "")
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Cancelled;
            Notes = reason;
            return true;
        }
        return false;
    }

    public bool Complete()
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Completed;
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