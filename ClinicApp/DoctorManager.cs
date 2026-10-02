namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
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

    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            // Пряме порівняння замість .ToLower().Contains()
            if (_doctors[i].Speciality == speciality)
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
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
    
    public bool TryFindById(int id, out Doctor doctor)
    {
        doctor = FindById(id);
        return doctor != null;
    }
    
    public Doctor[] FindBySpeciality(string query)
    {
        string search = query.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            string specName = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            if (specName.Contains(search))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            string specName = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            if (specName.Contains(search))
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }
    
    public Doctor? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count)
            {
                return _doctors[index];
            }
            return null;
        }
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

        for (int i = 0; i < _count; i++)
        {
            bool isUnique = true;
            for (int j = 0; j < i; j++)
            {
                // Прибрано .ToLower()
                if (_doctors[i].Speciality == _doctors[j].Speciality)
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    // Прибрано .ToLower()
                    if (_doctors[i].Speciality == _doctors[k].Speciality)
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