using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    private void WriteEntry(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }

    public void LogInfo(string message)
    {
        WriteEntry("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteEntry("WARN", message);
    }

    public void LogError(string message)
    {
        WriteEntry("ERROR", message);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
        => LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
        => LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} -> лікар #{e.DoctorId}");

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        string note = string.IsNullOrWhiteSpace(e.Notes) ? "без примітки" : e.Notes;
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {note}");

        Directory.CreateDirectory("alerts");
        string alertPath = Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");
        string alertLine = $"[{DateTime.Now:HH:mm:ss}] Запис #{e.AppointmentId} | Пацієнт #{e.PatientId} -> Лікар #{e.DoctorId} | {note}{Environment.NewLine}";
        File.AppendAllText(alertPath, alertLine, Encoding.UTF8);
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
        => LogInfo($"Запис #{e.AppointmentId} скасовано");

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
        => LogInfo($"Запис #{e.AppointmentId} завершено");

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
        => LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
        => LogInfo($"План лікування #{e.PlanId} завершено (пацієнт #{e.PatientId}: {e.Diagnosis})");

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0)
        {
            return new string[0];
        }

        string[] allLines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skip = Math.Max(0, allLines.Length - n);
        int count = allLines.Length - skip;

        string[] result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = allLines[skip + i];
        }

        return result;
    }

    public void Clear()
    {
        File.WriteAllText(_logPath, "", Encoding.UTF8);
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }
}