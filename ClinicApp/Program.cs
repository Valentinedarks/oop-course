namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        // Створення 3–4 лікарів різними конструкторами[cite: 9]
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        // Змінюємо графік після створення[cite: 10]
        d1.WorkStartHour = 8;
        d1.WorkEndHour = 16; 

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія");
        d2.LicenseNumber = "LIC-002";
        d2.Phone = "0442345678";
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
        
        Doctor d4 = new Doctor(); // Лікар за замовчуванням

        // Виведення на екран (спрацьовує ToString, де статус залежить від поточного часу)[cite: 9, 10]
        Console.WriteLine("\n--- Лікарі ---");
        Console.WriteLine(d1);
        Console.WriteLine(d2);
        Console.WriteLine(d3);
        Console.WriteLine(d4);
    }
}