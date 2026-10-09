using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count
    {
        get { return _count; }
    }

    public AppointmentManager(PatientManager patientManager, DoctorManager doctorManager)
    {
        _patients = patientManager;
        _doctors = doctorManager;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }
        return null;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Ліміт записів досягнуто.");
            return false;
        }

        Patient? p = _patients.FindById(patientId);
        Doctor? d = _doctors.FindById(doctorId);

        if (p == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }
        
        if (d == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        Appointment app = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = app;
        _count++;

        Console.WriteLine($"Запис [{app.Id}] створено: {p.FullName} -> {d.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? app = FindById(id);
        if (app != null)
        {
            return app.Cancel(reason);
        }
        return false;
    }

    public Appointment? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count)
            {
                return _appointments[index];
            }
            return null;
        }
    }
    
    public bool Complete(int id)
    {
        Appointment? app = FindById(id);
        if (app != null)
        {
            return app.Complete();
        }
        return false;
    }

    public void DisplayAppointment(Appointment app)
    {
        Patient? p = _patients.FindById(app.PatientId);
        Doctor? d = _doctors.FindById(app.DoctorId);

        string patientName = (p != null) ? p.FullName : $"Пацієнт #{app.PatientId}";
        string doctorName = (d != null) ? d.FullName : $"Лікар #{app.DoctorId}";

        string baseInfo = $"[{app.Id}] {patientName} -> {doctorName} | {app.ScheduledAt:dd.MM.yyyy HH:mm}-{app.EndsAt:HH:mm} | {app.Status}";
        
        if (app.Notes.Length > 0)
        {
            baseInfo += $" | {app.Notes}";
        }
        
        Console.WriteLine(baseInfo);
    }

    public void DisplayList(Appointment[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }
        
        for (int i = 0; i < list.Length; i++)
        {
            DisplayAppointment(list[i]);
        }
    }
    
    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }
    
    public Appointment[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetUpcoming()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }
}