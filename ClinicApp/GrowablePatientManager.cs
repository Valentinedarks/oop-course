namespace ClinicApp;

public class GrowablePatientManager
{
    // Внутрішній масив з початковою довжиною 4[cite: 21]
    private Patient[] _patients = new Patient[4];
    private int _count = 0;

    public int Count
    {
        get { return _count; }
    }

    // Повертає поточну довжину внутрішнього масиву _patients[cite: 21, 22]
    public int Capacity
    {
        get { return _patients.Length; }
    }

    // Приватний метод розширення масиву
    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2; // Збільшення вдвічі[cite: 21, 22]
        
        // Створення нового масиву та копіювання елементів[cite: 21, 22]
        Patient[] newArray = new Patient[newCapacity];
        for (int i = 0; i < _count; i++)
        {
            newArray[i] = _patients[i];
        }
        
        _patients = newArray;
        
        // Вивід повідомлення про розширення[cite: 22]
        Console.WriteLine($"Масив заповнений! Розширення: {oldCapacity} -> {newCapacity}");
    }

    public void Add(Patient patient)
    {
        // Перевірка на заповненість[cite: 21, 22]
        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count] = patient;
        _count++;
        
        // Форматований вивід згідно з прикладом[cite: 22]
        Console.WriteLine($"Додано [{patient.Id}]. Розмір: {_count} / {Capacity}");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null; // Повертає null, якщо не знайдено[cite: 21]
    }

    public bool Remove(int id)
    {
        int indexToRemove = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) return false;

        // Видалення зі зсувом ліворуч[cite: 21]
        for (int i = indexToRemove; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }
        _patients[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        Console.WriteLine($"\n=== Пацієнти (Динамічний список: {_count} / {Capacity}) ===");
        if (_count == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
    }
}