using System;
namespace Lab01;

internal class Task4
{
    static void Main()
    
    {
        int systolic = int.Parse(Console.ReadLine()!);
        int diastolic = int.Parse(Console.ReadLine()!);
        
        string category = "";
        
        if (systolic < 120 && diastolic < 80)
        {
            category = "норма";
        }
        else if (systolic < 130 && diastolic < 80)
        {
            category = "підвищений";
        }
        else if (systolic < 140 || diastolic < 90)
        {
            category = "гіпертонія 1 ступеня";
        }
        else
        {
            category = "гіпертонія 2 ступеня";
        }
        Console.WriteLine($"Тиск: {systolic}/{diastolic} - {category}");
    }
}