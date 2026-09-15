using System;

namespace Lab01;

internal class Task6
{
    static void Main()
    {
        
        int cardNumber = int.Parse(Console.ReadLine()!);
        int lastDigit = cardNumber % 10;
        string department = lastDigit switch
        {
            0 or 1 => "загальна терапія",
            2 or 3 => "хірургія",
            4 or 5 => "кардіологія",
            6 or 7 => "неврологія",
            8 or 9 => "офтальмологія",
            _ => "невідоме відділення" 
        };
        
        string hasDiscount = (cardNumber % 2 == 0) ? "так" : "ні";
        string needsCheckup = (cardNumber % 3 == 0) ? "так" : "ні";
        
        Console.WriteLine($"Відділення: {department}");
        Console.WriteLine($"Пільгова: {hasDiscount}");
        Console.WriteLine($"Огляд: {needsCheckup}");
    }
}