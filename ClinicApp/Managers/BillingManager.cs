using System;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointmentManager;

    public BillingManager(AppointmentManager appointmentManager)
    {
        _appointmentManager = appointmentManager;
    }

    private IPayable[] FilterUnpaid(Appointment[] source)
    {
        int matchCount = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (!source[i].IsPaid && !source[i].IsCancelled)
            {
                matchCount++;
            }
        }

        IPayable[] result = new IPayable[matchCount];
        int index = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (!source[i].IsPaid && !source[i].IsCancelled)
            {
                result[index++] = source[i];
            }
        }
        return result;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointmentManager.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointmentManager.GetByPatient(patientId));
    }

    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }
        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }
        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] all = _appointmentManager.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].Id == appointmentId)
            {
                if (!all[i].IsPaid && !all[i].IsCancelled)
                {
                    all[i].MarkPaid();
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] is Appointment a)
            {
                Console.WriteLine($"{a} | Сума: {a.GetCost():F2} грн");
            }
            else
            {
                Console.WriteLine($"Запис [{i + 1}] | Сума: {items[i].GetCost():F2} грн");
            }
        }
    }
}