using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено.");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0 || string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            int lineNumber = i + 1;

            try
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length != 5)
                {
                    string msg = parts.Length == 1
                        ? "Рядок має 1 поле замість 5."
                        : $"Рядок має {parts.Length} полів замість 5.";
                    result.AddError(lineNumber, msg);
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                DateTime dob = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                if (!Enum.IsDefined(typeof(BloodType), bloodType))
                {
                    throw new ArgumentException("Невідома група крові.");
                }

                string phone = parts[4].Trim();

                Patient patient = new Patient(firstName, lastName, dob, bloodType, phone);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception ex)
            {
                result.AddError(lineNumber, ex.Message);
            }
        }

        return result;
    }
}