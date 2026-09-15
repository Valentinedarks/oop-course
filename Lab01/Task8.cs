using System;
using System.Globalization;

namespace Lab01;

internal class Task8
{
    static void Main()
    {
        double weight = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        double height = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        
        double price = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        int quantity = int.Parse(Console.ReadLine()!);
        int discount = int.Parse(Console.ReadLine()!);
        
        int birthYear = int.Parse(Console.ReadLine()!);
        
        int systolic = int.Parse(Console.ReadLine()!);
        int diastolic = int.Parse(Console.ReadLine()!);
        
        double bmi = CalculateBMI(weight, height);
        string bmiCategory = GetBMICategory(bmi);
        
        double cost = CalculateCost(price, quantity, discount);
        
        int age = 2026 - birthYear;
        string ageCategory = GetAgeCategory(age);
        
        string pressureStatus = GetPressureStatus(systolic, diastolic);

        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "IMT: {0:F2} -> {1}", bmi, bmiCategory));
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Сума: {0:F2} грн", cost));
        Console.WriteLine($"Вік: {age} р., категорія: {ageCategory}");
        Console.WriteLine($"Тиск: {systolic}/{diastolic} - {pressureStatus}");
    }

    
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5) return "недостатня вага";
        if (bmi < 25) return "норма";
        if (bmi < 30) return "надмірна вага";
        return "ожиріння";
    }

    static double CalculateCost(double price, int quantity, int discount)
    {
        return price * quantity * (1 - discount / 100.0);
    }

    static string GetAgeCategory(int age)
    {
        if (age <= 17) return "дитина";
        if (age <= 59) return "дорослий";
        return "пенсіонер";
    }

    static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80) return "норма";
        if (systolic < 130 && diastolic < 80) return "підвищений";
        if (systolic < 140 || diastolic < 90) return "гіпертонія 1 ступеня";
        
        return "гіпертонія 2 ступеня";
    }
}