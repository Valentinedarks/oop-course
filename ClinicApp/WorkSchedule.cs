using System;

namespace ClinicApp;

public struct WorkSchedule
{
    // Властивості лише для читання
    public int Start { get; }
    public int End { get; }

    // Обчислювані властивості[cite: 15]
    public int HoursPerDay
    {
        get { return End - Start; }
    }

    public string Display
    {
        get { return $"{Start:D2}:00-{End:D2}:00"; } // Форматування з доповненням нулем
    }

    public bool IsNow
    {
        get { return Contains(DateTime.Now.Hour); } // Використання існуючого методу[cite: 16]
    }

    // Конструктор[cite: 15]
    public WorkSchedule(int start, int end)
    {
        Start = start;
        End = end;
    }

    // Метод перевірки діапазону
    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    // Перевизначення ToString[cite: 15]
    public override string ToString()
    {
        return Display + " (" + HoursPerDay + " год)";
    }
}