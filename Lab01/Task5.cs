using System;

namespace Lab01;

internal class Task5
{
    static void Main()
    {
        int day = int.Parse(Console.ReadLine()!);
        string result = day switch
        {
            1 => "Понеділок, 08:00–18:00",
            2 => "Вівторок, 08:00–18:00",
            3 => "Середа, 09:00–17:00",
            4 => "Четвер, 08:00–18:00",
            5 => "П'ятниця, 08:00–16:00",
            6 => "Субота, 09:00–14:00",
            7 => "Неділя – вихідний",
            _ => "невідомий день" 
        };
        Console.WriteLine($"День: {result}");
    }
}