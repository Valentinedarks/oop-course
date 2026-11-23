using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    private string EnsureReportDirectory()
    {
        string dir = Path.Combine(_baseDir, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    public string ExportPatients()
    {
        string dir = EnsureReportDirectory();
        string path = Path.Combine(dir, "patients.txt");
        Patient[] patients = _clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Пацієнти - звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine("---------------------------");

        for (int i = 0; i < patients.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {patients[i]}");
        }

        writer.WriteLine("---------------------------");
        writer.WriteLine($"Всього: {patients.Length}");

        return path;
    }

    public string ExportAppointments()
    {
        string dir = EnsureReportDirectory();
        string path = Path.Combine(dir, "appointments.txt");
        Appointment[] appointments = _clinic.Appointments.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Записи на прийом - звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine("---------------------------");

        for (int i = 0; i < appointments.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {appointments[i]}");
        }

        writer.WriteLine("---------------------------");
        writer.WriteLine($"Всього: {appointments.Length}");

        return path;
    }

    public string ExportBilling()
    {
        string dir = EnsureReportDirectory();
        string path = Path.Combine(dir, "billing.txt");
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        decimal totalDebt = _clinic.Billing.GetTotalDebt();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Рахунки та борги - звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine("---------------------------");

        for (int i = 0; i < unpaid.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
        }

        writer.WriteLine("---------------------------");
        writer.WriteLine($"Всього: {unpaid.Length}");
        writer.WriteLine($"Загальний борг: {totalDebt:F2} грн");

        return path;
    }

    public string ExportTreatmentPlans()
    {
        string dir = EnsureReportDirectory();
        string path = Path.Combine(dir, "treatment_plans.txt");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Плани лікування - звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine("---------------------------");

        for (int i = 0; i < plans.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {plans[i]}");
        }

        writer.WriteLine("---------------------------");
        writer.WriteLine($"Всього: {plans.Length}");

        return path;
    }

    public string[] ExportAll()
    {
        return new string[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }
}