using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();
        if (obj == null)
        {
            return result;
        }

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            object? value = prop.GetValue(obj);

            RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    result.AddError($"{prop.Name}: {requiredAttr.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null && value is string strValue)
            {
                if (strValue.Length > maxLengthAttr.Length)
                {
                    result.AddError($"{prop.Name}: {maxLengthAttr.ErrorMessage}");
                }
            }

            MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttr != null && value != null)
            {
                try
                {
                    double numericValue = Convert.ToDouble(value);
                    if (numericValue < minValueAttr.Min)
                    {
                        result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                    }
                }
                catch
                {
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine(type.Name);
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            object[] attrs = prop.GetCustomAttributes(false);

            if (attrs.Length == 0)
            {
                Console.WriteLine($"  {prop.Name}");
            }
            else
            {
                string attrList = "";
                for (int j = 0; j < attrs.Length; j++)
                {
                    string attrName = attrs[j].GetType().Name;
                    if (attrName.EndsWith("Attribute"))
                    {
                        attrName = attrName.Substring(0, attrName.Length - "Attribute".Length);
                    }
                    attrList += (j > 0 ? " " : "") + $"[{attrName}]";
                }

                Console.WriteLine($"  {prop.Name,-14} {attrList}");
            }
        }
    }
}