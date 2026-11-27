using System;
using System.Collections.Generic;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Events;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public event EventHandler<PatientEventArgs>? PatientAdded;

    public int Count
    {
        get { return _patients.Count; }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
        PatientAdded?.Invoke(this, new PatientEventArgs(patient.Id, patient.FullName));
    }

    public Patient? FindById(int id)
    {
        foreach (var patient in _patients)
        {
            if (patient.Id == id)
            {
                return patient;
            }
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        string search = name.ToLower();
        List<Patient> result = new List<Patient>();

        foreach (var patient in _patients)
        {
            if (patient.FirstName.ToLower().Contains(search) || 
                patient.LastName.ToLower().Contains(search))
            {
                result.Add(patient);
            }
        }

        return result.ToArray();
    }

    public Patient? this[int index]
    {
        get
        {
            if (index >= 0 && index < _patients.Count)
            {
                return _patients[index];
            }
            return null;
        }
    }
    
    public bool TryFindById(int id, out Patient patient)
    {
        patient = FindById(id)!;
        return patient != null;
    }
    
    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();
        foreach (var patient in _patients)
        {
            if (patient.BloodType == bloodType)
            {
                result.Add(patient);
            }
        }
        return result.ToArray();
    }
    
    public bool Remove(int id)
    {
        Patient? patient = FindById(id);
        if (patient != null)
        {
            return _patients.Remove(patient);
        }
        return false;
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"\n=== Пацієнти ({_patients.Count}) ===");
        foreach (var patient in _patients)
        {
            Console.WriteLine(patient.ToString());
        }
        Console.WriteLine(new string('=', 30));
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Немає даних для статистики.");
            return;
        }

        int sumAges = 0;
        int adultCount = 0;
        int minIndex = 0;
        int maxIndex = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;
            sumAges += age;

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }

            if (age < _patients[minIndex].Age)
            {
                minIndex = i;
            }

            if (age > _patients[maxIndex].Age)
            {
                maxIndex = i;
            }
        }

        double avgAge = (double)sumAges / _patients.Count;

        Console.WriteLine("\n=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:\t\t{_patients.Count}");
        Console.WriteLine($"Середній вік:\t{avgAge:F1} р.");
        Console.WriteLine($"Наймолодший:\t{_patients[minIndex].FullName} ({_patients[minIndex].Age} р.)");
        Console.WriteLine($"Найстарший:\t{_patients[maxIndex].FullName} ({_patients[maxIndex].Age} р.)");
        Console.WriteLine($"Дорослих:\t{adultCount} з {_patients.Count}");
        Console.WriteLine(new string('=', 30));
    }
}