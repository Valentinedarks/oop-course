namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50; // Ліміт для лікарів
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count
    {
        get { return _count; }
    }

    public void Add(Doctor doctor)
    {
        if (_count < MaxDoctors)
        {
            _doctors[_count] = doctor;
            _count++;
            Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
        }
        else
        {
            Console.WriteLine("Ліміт лікарів досягнуто.");
        }
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        // Той самий двопрохідний патерн, що й у пацієнтах[cite: 14]
        string search = speciality.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                result[index] = _doctors[i];
                index++;
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        // Повертаємо копію масиву рівно на _count елементів[cite: 14]
        Doctor[] copy = new Doctor[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _doctors[i];
        }
        return copy;
    }

    public bool Remove(int id)
    {
        int indexToRemove = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) return false;

        // Зсуваємо елементи[cite: 13]
        for (int i = indexToRemove; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"\n=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i].ToString());
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

        int availableNow = 0;
        for (int i = 0; i < _count; i++)
        {
            // Перевірка доступності[cite: 14]
            if (_doctors[i].IsAvailableNow)
            {
                availableNow++;
            }
        }

        Console.WriteLine("\n=== Статистика лікарів ===");
        Console.WriteLine($"Всього:\t\t{_count}");
        Console.WriteLine($"Доступні зараз:\t{availableNow}");
        Console.WriteLine("По спеціальностях:");

        // Пошук унікальних спеціальностей через вкладені цикли[cite: 14]
        for (int i = 0; i < _count; i++)
        {
            bool isUnique = true;
            // Перевіряємо, чи не зустрічалася ця спеціальність раніше (j < i)[cite: 14]
            for (int j = 0; j < i; j++)
            {
                if (_doctors[i].Speciality.ToLower() == _doctors[j].Speciality.ToLower())
                {
                    isUnique = false;
                    break;
                }
            }

            // Якщо це нова спеціальність, рахуємо скільки лікарів її мають[cite: 14]
            if (isUnique)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[i].Speciality.ToLower() == _doctors[k].Speciality.ToLower())
                    {
                        specCount++;
                    }
                }
                Console.WriteLine($"  {_doctors[i].Speciality}:\t{specCount}");
            }
        }
        Console.WriteLine(new string('=', 30));
    }
}