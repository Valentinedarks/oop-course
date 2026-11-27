using System;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }

    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing { get; }
    public AnalyticsManager Analytics { get; }
    public TreatmentPlanManager TreatmentPlans { get; }

    public ClinicLogger Logger { get; }
    public ClinicExporter Exporter { get; }
    public SessionManager Session { get; }
    public PatientPassportWriter Passport { get; }
    public SessionEventTracker Tracker { get; }

    public WaitingQueue<Patient> WaitingRoom { get; }

    public Clinic(string name)
    {
        Name = name;

        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        TreatmentPlans = new TreatmentPlanManager();

        Logger = new ClinicLogger();
        Exporter = new ClinicExporter(this);
        Session = new SessionManager();
        Passport = new PatientPassportWriter(this);
        Tracker = new SessionEventTracker(this);

        WaitingRoom = new WaitingQueue<Patient>();

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Patients.PatientAdded += Logger.OnPatientAdded;
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;

        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;

        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"\n=== Розклад на {date:dd.MM.yyyy} ===");
        Appointment[] appsOnDate = Appointments.GetByDate(date);
        Appointments.DisplayList(appsOnDate);
    }

    public void GenerateReport()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║ Звіт — {Name,-47} ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════╣");
        Console.WriteLine($"║ Пацієнтів:          {Patients.Count,-34} ║");
        Console.WriteLine($"║ Лікарів:            {Doctors.Count,-34} ║");

        Appointment[] upcoming = Appointments.GetUpcoming();
        Console.WriteLine($"║ Майбутніх записів:  {upcoming.Length,-34} ║");

        Console.WriteLine("╠════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Навантаження лікарів (майбутні записи):                ║");

        Doctor[] allDoctors = Doctors.GetAll();

        for (int i = 0; i < allDoctors.Length; i++)
        {
            int loadCount = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == allDoctors[i].Id)
                {
                    loadCount++;
                }
            }
            string docLine = $"  {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {loadCount} записів";
            Console.WriteLine($"║ {docLine,-54} ║");
        }
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
    }
}