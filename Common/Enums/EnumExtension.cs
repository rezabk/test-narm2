using System.ComponentModel;

namespace Common.Enums;

public static class EnumExtension
{
    public static TAttribute GetAttribute<TAttribute>(this System.Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = System.Enum.GetName(type, value);
        if (name == null)
            return null;
        return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }

    public static string GetPatternName(this System.Enum value)
    {
        if (value == null) return string.Empty;

        var description = GetAttribute<Pattern>(value);
        return description?.PatternName;
    }

    public static string GetEnumDescription(this System.Enum value)
    {
        if (value == null) return string.Empty;

        var description = GetAttribute<DescriptionAttribute>(value);
        return description?.Description;
    }


    public static T? SafeNullableEnum2<T>(this int e) where T : struct
    {
        if (!System.Enum.IsDefined(typeof(T), e))
            return null;
        return (T)System.Enum.ToObject(typeof(T), e);
    }

    public static T? SafeNullableEnum2<T>(this int? e) where T : struct
    {
        if (!System.Enum.IsDefined(typeof(T), e))
            return null;
        return (T)System.Enum.ToObject(typeof(T), e);
    }


    public static int GetId(this System.Enum value)
    {
        if (value == null) return 0;

        var val = Convert.ChangeType(value, typeof(int));
        return val.SafeInt();
    }

    public static string? GetName(this System.Enum value)
    {
        return value == null ? null : nameof(value);
    }

    public static string EnumListToString<T>(this List<T> value) where T : System.Enum
    {
        var result = "";
        foreach (var item in value)
        {
            var enumInt = item.GetId();
            result += enumInt.ToString();
        }

        return result;
    }

    public static List<T> ToEnumList<T>(this string value) where T : System.Enum
    {
        var result = new List<T>();

        foreach (var item in value)
        {
            var t = (T)System.Enum.Parse(typeof(T), item.ToString());
            result.Add(t);
        }

        return result;
    }
}