using System;
using System.Globalization;

namespace Lab01;

internal class Task2
{
    static void Main()
    {
        double price = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        
        int quantity = int.Parse(Console.ReadLine()!);
        int discount = int.Parse(Console.ReadLine()!);
        
        double total = price * quantity * (1 - discount / 100.0);
        
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Сума: {0:F2} грн", total));
    }
}