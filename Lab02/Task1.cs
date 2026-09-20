using System.Globalization;
namespace Lab2
{
    class Task1
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            int n = int.Parse(Console.ReadLine());
            double[] weights = new double[n];
            
            for (int i = 0; i < n; i++)
            {
                weights[i] = double.Parse(Console.ReadLine());
            }
            
            double min = weights[0];
            double max = weights[0];
            double sum = 0;

            foreach (double weight in weights)
            {
                sum += weight; 
                if (weight < min) min = weight; 
                if (weight > max) max = weight; 
            }
            double average = sum / n;
            int aboveAverageCount = 0;
            foreach (double weight in weights)
            {
                if (weight > average)
                {
                    aboveAverageCount++;
                }
            }
            Console.WriteLine($"Кількість: {n} / Середня вага: {average:F1} кг / Мін / Макс: {min:F1} / {max:F1} кг / Вище середнього: {aboveAverageCount} з {n}");
        }
    }
}