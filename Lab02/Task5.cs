namespace Lab2
{
    class Task5
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            int[,] matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] rowValues = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                }
            }

            int[] mainDiag = new int[n];
            int[] secDiag = new int[n];

            int mainSum = 0;
            int secSum = 0;

            for (int i = 0; i < n; i++)
            {
                mainDiag[i] = matrix[i, i];
                mainSum += matrix[i, i];

                int secCol = n - 1 - i;
                secDiag[i] = matrix[i, secCol];
                secSum += matrix[i, secCol];
            }

            Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiag)} (сума = {mainSum})");
            Console.WriteLine($"Побічна діагональ: {string.Join(", ", secDiag)} (сума = {secSum})");
        }
    }
}