using System;

namespace Lab2
{
    class Task4
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int m = int.Parse(Console.ReadLine());
            int[,] matrix = new int[n, m];
            int maxAppointments = -1;
            int maxRow = 0;
            int maxCol = 0;

            for (int i = 0; i < n; i++)
            {
                string[] rowValues = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                    if (matrix[i, j] > maxAppointments)
                    {
                        maxAppointments = matrix[i, j];
                        maxRow = i;
                        maxCol = j;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                int doctorSum = 0;
                for (int j = 0; j < m; j++)
                {
                    doctorSum += matrix[i, j];
                }
                Console.WriteLine($"Лікар {i + 1}: {doctorSum} прийомів");
            }

            int[] daysSums = new int[m];
            for (int j = 0; j < m; j++)
            {
                int daySum = 0;
                for (int i = 0; i < n; i++)
                {
                    daySum += matrix[i, j];
                }
                daysSums[j] = daySum;
            }
            Console.WriteLine($"По днях: {string.Join(", ", daysSums)}");
            Console.WriteLine($"Максимум: {maxAppointments} (Лікар {maxRow + 1}, День {maxCol + 1})");
        }
    }
}