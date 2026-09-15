using System;
using System.Globalization;

namespace Lab01;

internal class Task1
{
    static void Main()
    {
        double weight = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        double height = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        
        double imt = weight / (height * height);
        
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "IMT: {0:F2}", imt));
    }
    
}