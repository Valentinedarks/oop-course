using System;
using System.Globalization;

namespace Lab01;

internal class Task7
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine()!);
        decimal[] prices = new decimal[n];
        
        for (int i = 0; i < n; i++)
        {
            prices[i] = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        }
        decimal sum = 0;
        decimal min = decimal.MaxValue;
        decimal max = decimal.MinValue;
        
        foreach (decimal price in prices)
        {
            sum += price;
            if (price < min) min = price;
            if (price > max) max = price;
        }
        
        decimal average = sum / n;
        
        int aboveAverageCount = 0;
        for (int i = 0; i < n; i++)
        {
            if (prices[i] > average)
            {
                aboveAverageCount++;
            }
        }
        int expensiveIndex = -1; 
        int index = 0;
        while (index < n)
        {
            if (prices[index] > 1000m) 
            {
                expensiveIndex = index;
                break; 
            }
            index++;
        }

        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість:       {n}");
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Загальна сума:   {0:F2} грн", sum));
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Середня:         {0:F2} грн", average));
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Мін / Макс:      {0:F2} / {1:F2} грн", min, max));
        Console.WriteLine($"Вище середнього: {aboveAverageCount} з {n}");
        
        if (expensiveIndex != -1)
        {
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Перший > 1000:   #{0} — {1:F2} грн", expensiveIndex + 1, prices[expensiveIndex]));
        }
        else
        {
            Console.WriteLine("Перший > 1000:   немає");
        }
        Console.WriteLine("========================");
    }
}