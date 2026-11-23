using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine("[PATIENTS]");

        for (int i = 0; i < patients.Length; i++)
        {
            Patient p = patients[i];
            writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
        }

        writer.WriteLine("[END]");
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath))
        {
            return 0;
        }

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string currentSection = "";
        int addedCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                currentSection = line;
                continue;
            }

            if (currentSection == "[PATIENTS]")
            {
                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5)
                    {
                        continue;
                    }

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    DateTime dob = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                    if (!Enum.IsDefined(typeof(BloodType), bloodType))
                    {
                        continue;
                    }

                    string phone = parts[4].Trim();

                    Patient[] existing = clinic.Patients.GetAll();
                    bool alreadyExists = false;
                    for (int j = 0; j < existing.Length; j++)
                    {
                        if (string.Equals(existing[j].FirstName, firstName, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(existing[j].LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                            existing[j].DateOfBirth.Date == dob.Date)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists)
                    {
                        continue;
                    }

                    clinic.Patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
                    addedCount++;
                }
                catch
                {
                }
            }
        }

        return addedCount;
    }
}