namespace ClinicApp;

public class PatientManager
{
    // Три приватні поля за схемою Лаби 02
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count
    {
        get { return _count; }
    }

    public void Add(Patient patient)
    {
        if (_count < MaxPatients)
        {
            _patients[_count] = patient;
            _count++;
            Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
        }
        else
        {
            Console.WriteLine("Ліміт пацієнтів досягнуто. Неможливо додати нового.");
        }
    }

    public Patient? FindById(int id)
    {
        // Лінійний пошук по перших _count елементах[cite: 12]
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        // Двопрохідний патерн: спочатку рахуємо, потім заповнюємо[cite: 12]
        string search = name.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) || 
                _patients[i].LastName.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Patient[] result = new Patient[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) || 
                _patients[i].LastName.ToLower().Contains(search))
            {
                result[index] = _patients[i];
                index++;
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int indexToRemove = -1;

        // Шукаємо індекс елемента[cite: 12]
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1)
        {
            return false;
        }

        // Зсуваємо всі наступні елементи на одну позицію ліворуч[cite: 12]
        for (int i = indexToRemove; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        // Очищуємо дубльовану комірку та зменшуємо лічильник[cite: 12]
        _patients[_count - 1] = null!; 
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        // Відпрацювання порожнього списку[cite: 12]
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"\n=== Пацієнти ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
        Console.WriteLine(new string('=', 30));
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Немає даних для статистики.");
            return;
        }

        // Змінні для накопичення даних за один прохід[cite: 12]
        int sumAges = 0;
        int adultCount = 0;
        int minIndex = 0;
        int maxIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            int age = _patients[i].Age;
            sumAges += age;

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }

            if (age < _patients[minIndex].Age)
            {
                minIndex = i;
            }

            if (age > _patients[maxIndex].Age)
            {
                maxIndex = i;
            }
        }

        double avgAge = (double)sumAges / _count;

        Console.WriteLine("\n=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:\t\t{_count}");
        Console.WriteLine($"Середній вік:\t{avgAge:F1} р.");
        Console.WriteLine($"Наймолодший:\t{_patients[minIndex].FullName} ({_patients[minIndex].Age} р.)");
        Console.WriteLine($"Найстарший:\t{_patients[maxIndex].FullName} ({_patients[maxIndex].Age} р.)");
        Console.WriteLine($"Дорослих:\t{adultCount} з {_count}");
        Console.WriteLine(new string('=', 30));
    }
}