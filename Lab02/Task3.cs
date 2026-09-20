namespace Lab2
{
    class Task3
    {
        static void Main(string[] args)
        {
            string[] days = { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
            int[] counts = new int[7];
            for (int i = 0; i < 7; i++)
            {
                counts[i] = int.Parse(Console.ReadLine());
            }
            int total = 0;
            int maxIdx = 0;
            int minIdx = 0;
            for (int i = 0; i < 7; i++)
            {
                total += counts[i];
                if (counts[i] > counts[maxIdx])
                {
                    maxIdx = i;
                }
                if (counts[i] < counts[minIdx])
                {
                    minIdx = i;
                }
            }
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{days[i],-11}: {counts[i]} пацієнтів");
            }
            Console.WriteLine($"{"Разом:",-11} {total}");
            Console.WriteLine($"{"Найбільше:",-11} {days[maxIdx]} ({counts[maxIdx]})");
            Console.WriteLine($"{"Найменше:",-11} {days[minIdx]} ({counts[minIdx]})");
        }
    }
}