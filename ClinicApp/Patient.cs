namespace ClinicApp;

public class Patient
{
    // Статичне поле, спільне для всіх об'єктів класу[cite: 8]
    private static int _nextId = 1;

    // Властивість лише для читання[cite: 7]
    public int Id { get; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    // Обчислювана властивість (лише get)[cite: 7]
    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    // Обчислення повних років[cite: 8]
    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            // Якщо день народження у цьому році ще не настав, віднімаємо 1 рік[cite: 8]
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    // Обчислювана властивість перевірки повноліття[cite: 7]
    public bool IsAdult
    {
        get { return Age >= 18; }
    }

    // 1. Повний конструктор (призначає Id)
    public Patient(string firstName, string lastName, DateTime dob, string bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = ""; // порожній рядок, якщо невідомий[cite: 7]
    }

    // 2. Конструктор з базовими даними (делегує повному через this)[cite: 7, 8]
    public Patient(string firstName, string lastName) 
        : this(firstName, lastName, new DateTime(2000, 1, 1), "Невідомо", "0000000000")
    {
    }

    // 3. Конструктор за замовчуванням (делегує базовому)[cite: 7]
    public Patient() 
        : this("Невідомий", "Пацієнт")
    {
    }

    // Метод класифікації віку через каскад if[cite: 8]
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

    // Перевизначення методу ToString() для красивого виводу[cite: 7]
    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {Age} ({GetAgeCategory()}) | Кров: {BloodType} | Тел: {Phone}";
    }
}