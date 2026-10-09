using System;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException("Ім'я не може бути порожнім або довшим за 50 символів.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (phone == null || phone.Length != 10)
        {
            throw new ArgumentException("Телефон повинен містити рівно 10 цифр.", nameof(phone));
        }
        
        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
            {
                throw new ArgumentException("Телефон повинен містити лише цифри.", nameof(phone));
            }
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Дата поза допустимими межами.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Значення повинно бути більшим за нуль.");
        }
    }
}