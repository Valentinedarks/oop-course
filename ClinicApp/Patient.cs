namespace ClinicApp;

public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    // Використовуємо enum[cite: 11]
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
    

    public bool IsAdult
    {
        get { return Age >= 18; }
    }

    // Змінено тип параметра bloodType[cite: 11]
    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
    }

    // Делегуємо з BloodType.Unknown[cite: 11]
    public Patient(string firstName, string lastName)
        : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000")
    {
    }

    public Patient()
        : this("Невідомий", "Пацієнт")
    {
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
        {
            return "дитина";
        }
        else if (Age < 60)
        {
            return "дорослий";
        }
        else
        {
            return "літній";
        }
    }

    public override string ToString()
    {
        return
            $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Кров: {ClinicFormatter.FormatBloodType(BloodType)} | Тел: {ClinicFormatter.FormatPhone(Phone)}";
    }
}