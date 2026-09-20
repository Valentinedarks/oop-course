using System.Globalization;

namespace Lab2
{
    class Task6
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            int n = int.Parse(Console.ReadLine());

            int[][] costs = new int[n][];

            for (int i = 0; i < n; i++)
            {
                int k = int.Parse(Console.ReadLine());
                costs[i] = new int[k]; 

                for (int j = 0; j < k; j++)
                {
                    costs[i][j] = int.Parse(Console.ReadLine());
                }
            }

            int maxIncome = -1;
            int maxDoctorIndex = 0;

            for (int i = 0; i < n; i++)
            {
                int k = costs[i].Length;
                int sum = 0;
                
                for (int j = 0; j < k; j++)
                {
                    sum += costs[i][j];
                }

                if (sum > maxIncome)
                {
                    maxIncome = sum;
                    maxDoctorIndex = i;
                }

                double average = 0;
                if (k > 0)
                {
                    average = (double)sum / k;
                }

                string word = "прийомів";
                if (k % 10 == 1 && k % 100 != 11) word = "прийом";
                else if (k % 10 >= 2 && k % 10 <= 4 && (k % 100 < 10 || k % 100 >= 20)) word = "прийоми";

                Console.WriteLine($"Лікар {i + 1}: {k} {word}, сума={sum} грн, середня={average:F2} грн");
            }

            Console.WriteLine($"Найбільший дохід: Лікар {maxDoctorIndex + 1} ({maxIncome} грн)");
        }
    }
}