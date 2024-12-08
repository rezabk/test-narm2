using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace Common.Enums;

public static class EnumHelper<T>
    where T : struct, System.Enum // This constraint requires C# 7.3 or later.
{
    //public static int GetId(this Enum value)
    //{
    //    if (value == null)
    //    {
    //        return 0;
    //    }

    //    var result = Convert.ChangeType(value, typeof(int));
    //    return (int)result;
    //}
    public static IList<T> GetValues(System.Enum value)
    {
        var enumValues = new List<T>();

        foreach (var fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            enumValues.Add((T)System.Enum.Parse(value.GetType(), fi.Name, false));
        return enumValues;
    }

    public static T Parse(string value)
    {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(System.Enum value)
    {
        var _result = value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name)
            .ToList();
        return _result;
    }

    public static IList<string> GetDisplayValues(System.Enum value)
    {
        var _result = GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        return _result;
    }

    private static string lookupResource(Type resourceManagerProvider, string resourceKey)
    {
        foreach (var staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static |
                                                                             BindingFlags.NonPublic |
                                                                             BindingFlags.Public))
            if (staticProperty.PropertyType == typeof(ResourceManager))
            {
                var resourceManager = (ResourceManager)staticProperty.GetValue(null, null);
                return resourceManager.GetString(resourceKey);
            }

        return resourceKey; // Fallback with the key name
    }

    public static string GetDisplayValue(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        var _result = descriptionAttributes[0];
        if (descriptionAttributes[0] != null)
            return lookupResource(descriptionAttributes[0].GetType(), descriptionAttributes[0].Description);

        if (descriptionAttributes == null) return string.Empty;
        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : value.ToString();
    }

    public static IEnumerable<EnumDto> GetValueAndDescription()
    {
        var _result = System.Enum.GetValues(typeof(T))
            .Cast<System.Enum>()
            .Select(value => new EnumDto
            {
                Id = (int)Convert.ChangeType(value, typeof(int)),
                Description = GetDisplayValue(Parse(value.ToString())),
                value = value.ToString()
            })
            .OrderBy(item => item.Id);

        //if (Request_value != null)
        //    _result = (IOrderedEnumerable<EnumDto>)_result.Where(x => x.value == Request_value);

        return _result.ToList();
    }
}

public class EnumDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string value { get; set; }
    public bool IsActive { get; set; } = false;
}