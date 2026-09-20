using System;

namespace Lab2
{
    class Task8
    {
        static void Main(string[] args)
        {
            int d = int.Parse(Console.ReadLine());
            int w = int.Parse(Console.ReadLine());
            int[,,] patients = new int[d, w, 2];
            int[] deptTotals = new int[d];
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        patients[i, j, k] = int.Parse(Console.ReadLine());
                    }
                }
            }
            
            for (int i = 0; i < d; i++)
            {
                Console.WriteLine($"Відділення {i + 1}:");
                int currentDeptTotal = 0;
                for (int j = 0; j < w; j++)
                {
                    int morning = patients[i, j, 0];
                    int evening = patients[i, j, 1];
                    int weekTotal = morning + evening;
                    
                    currentDeptTotal += weekTotal;

                    Console.WriteLine($"    Тиждень {j + 1}: ранок {morning}, вечір {evening} -> разом {weekTotal}");
                }

                deptTotals[i] = currentDeptTotal;
                Console.WriteLine($"    Разом: {currentDeptTotal} пацієнтів");
            }
            int maxIdx = 0;
            for (int i = 1; i < d; i++)
            {
                if (deptTotals[i] > deptTotals[maxIdx])
                {
                    maxIdx = i;
                }
            }
            Console.WriteLine($"Найзавантаженіше: Відділення {maxIdx + 1} ({deptTotals[maxIdx]} пацієнтів)");
        }
    }
}