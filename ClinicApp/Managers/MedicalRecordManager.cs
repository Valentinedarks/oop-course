using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count => _count;

    public void Add(MedicalRecord record)
    {
        if (_count < MaxRecords)
        {
            _records[_count] = record;
            _count++;
            Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
        }
        else
        {
            Console.WriteLine("Ліміт медичних записів досягнуто.");
        }
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId) matchCount++;
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index++] = _records[i];
            }
        }
        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId) matchCount++;
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index++] = _records[i];
            }
        }
        return result;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Медичних записів не знайдено.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i].ToString());
        }
    }

    public void DisplayList(MedicalRecord[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < list.Length; i++)
        {
            Console.WriteLine(list[i].ToString());
        }
    }

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count)
            {
                return _records[index];
            }
            return null;
        }
    }
}