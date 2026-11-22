using System;
using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < doctors.Length; i++)
        {
            Doctor doctor = doctors[i];
            int count = 0;
            decimal revenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++)
            {
                Appointment app = appointments[j];
                if (app.DoctorId == doctor.Id)
                {
                    count++;
                    revenue += app.GetCost();
                    if (app.ScheduledAt > lastDate)
                    {
                        lastDate = app.ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(doctor.Id, doctor.FullName, count, revenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = patients[i];
            int count = 0;
            decimal spent = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++)
            {
                Appointment app = appointments[j];
                if (app.PatientId == patient.Id)
                {
                    count++;
                    spent += app.GetCost();
                    if (app.ScheduledAt > lastDate)
                    {
                        lastDate = app.ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, count, spent, lastDate);
        }
    }
}