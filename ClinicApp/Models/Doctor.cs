using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public int Id { get; private set; }

    public string FirstName
    {
        get { return _firstName; }
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get { return _lastName; }
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));
            _lastName = value;
        }
    }

    public Speciality Speciality { get; set; }

    public string LicenseNumber
    {
        get { return _licenseNumber; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));
            }
            _licenseNumber = value;
        }
    }

    public string Phone
    {
        get { return _phone; }
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    public WorkSchedule Schedule { get; set; }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    public bool IsAvailableNow
    {
        get { return Schedule.IsNow; }
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17);
        
        Id = _nextId++;
    }

    public Doctor(string firstName, string lastName, Speciality speciality) 
        : this(firstName, lastName, speciality, "Невідомо", "0000000000")
    {
    }

    public Doctor() 
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}