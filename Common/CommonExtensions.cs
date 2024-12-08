using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DNTPersianUtils.Core;

namespace Common;

public static class CommonExtensions
{
    private static readonly string[] formats =
    {
        "MM/dd/yyyy",
        "MM/dd/yyyy HH",
        "MM/dd/yyyy H:mm",
        "MM/dd/yyyy HH:mm",
        "MM/dd/yyyy H.mm",
        "MM/dd/yyyy HH.mm",
        "MM/dd/yyyy HH:mm:ss",
        "M/dd/yyyy H:mm:ss tt",
        "M/dd/yyyy H:mm:ss"
    };

    public static DateTime MinDate => new(1907, 1, 1);
    public static DateTime MaxDate => new(2060, 1, 1);

    public static DateTime ConvertJalaliToMiladi(this string persianDate)
    {
        var timeSpan = new TimeSpan(0, 0, 0, 0);
        var calendar = new PersianCalendar();
        try
        {
            persianDate = persianDate.PersianNumberToLatin();

            if (string.IsNullOrEmpty(persianDate)) return DateTime.MinValue;
            persianDate = persianDate.Trim().ToEnglishNumbers();
            persianDate = persianDate.Replace("-", "/");
            persianDate = persianDate.Replace(",", "/");
            persianDate = persianDate.Replace("؍", "/");
            persianDate = persianDate.Replace(".", "/");
            persianDate = persianDate.Replace("-", "/");
            var s = persianDate.Split(' ');
            if (s.Length == 2) timeSpan = TimeSpan.Parse(s[1]);
            var date = s[0];

            var match = Regex.Match(date,
                @"(?'Year'(^[1-4]\d{3})|(\d{2}))[/-:](((?'Month'0?[1-6])\/((?'Day'(3[0-1])|([1-2][0-9])|(0?[1-9])))|((?'Month'1[0-2]|(0?[7-9]))\/(?'Day'30|([1-2][0-9])|(0?[1-9])))))$");
            if (!match.Success) throw new Exception("InvalidPersianDate");
            var yearGroup = match.Groups["Year"].ToString();
            if (yearGroup.Length == 2) yearGroup = $"13{yearGroup}";
            var year = yearGroup.SafeInt(0);
            var month = match.Groups["Month"].SafeInt(0);
            var day = match.Groups["Day"].SafeInt(0);

            DateTime result;
            try
            {
                result = calendar.ToDateTime(year, month, day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                    timeSpan.Milliseconds);
            }
            catch (Exception exDate)
            {
                if (exDate.Message == "Day must be between 1 and 29 for month 12.\r\nParameter name: day")
                    result = calendar.ToDateTime(year, month, day - 1, timeSpan.Hours, timeSpan.Minutes,
                        timeSpan.Seconds, timeSpan.Milliseconds);
                else
                    throw new Exception("InvalidPersianDate");
            }

            // Explicitly convert to UTC
            return DateTime.SpecifyKind(result, DateTimeKind.Unspecified).ToUniversalTime();
        }
        catch (Exception)
        {
            throw new Exception("InvalidPersianDate");
        }
    }

    public static DateTime ConvertJalaliToMiladi(this string persianDate, string time)
    {
        try
        {
            if (string.IsNullOrEmpty(persianDate)) return DateTime.MinValue;
            persianDate = persianDate.Trim().ToEnglishNumbers();
            persianDate = persianDate.Replace("-", "/");
            persianDate = persianDate.Replace(",", "/");
            persianDate = persianDate.Replace("؍", "/");
            persianDate = persianDate.Replace(".", "/");
            persianDate = persianDate.Replace("-", "/");
            var s = persianDate.Split(' ');
            string date;
            var timeSpan = new TimeSpan(0, 0, 0, 0);
            if (s.Length == 2) timeSpan = TimeSpan.Parse(s[1]);
            date = s[0];

            var match = Regex.Match(date,
                @"(?'Year'(^[1-4]\d{3})|(\d{2}))[/-:](((?'Month'0?[1-6])\/((?'Day'(3[0-1])|([1-2][0-9])|(0?[1-9])))|((?'Month'1[0-2]|(0?[7-9]))\/(?'Day'30|([1-2][0-9])|(0?[1-9])))))$");
            if (!match.Success) throw new Exception("InvalidPersianDate");
            var yearGroup = match.Groups["Year"].ToString();
            if (yearGroup.Length == 2) yearGroup = $"13{yearGroup}";
            var year = yearGroup.SafeInt(0);
            var month = match.Groups["Month"].SafeInt(0);
            var day = match.Groups["Day"].SafeInt(0);
            var calendar = new PersianCalendar();

            DateTime result;
            try
            {
                result = calendar.ToDateTime(year, month, day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                    timeSpan.Milliseconds);
            }
            catch (Exception exDate)
            {
                if (exDate.Message == "Day must be between 1 and 29 for month 12.\r\nParameter name: day")
                    result = calendar.ToDateTime(year, month, day - 1, timeSpan.Hours, timeSpan.Minutes,
                        timeSpan.Seconds, timeSpan.Milliseconds);
                else
                    throw new Exception("InvalidPersianDate");
            }

            // Explicitly convert to UTC
            return DateTime.SpecifyKind(result, DateTimeKind.Unspecified).ToUniversalTime();
        }
        catch (Exception)
        {
            throw new Exception("InvalidPersianDate");
        }
    }

    public static DateTime ConvertJalaliToMiladi(int year, int month, int day)
    {
        var calendar = new PersianCalendar();
        var timeSpan = new TimeSpan(0, 0, 0, 0);
        try
        {
            return calendar.ToDateTime(year, month, day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                timeSpan.Milliseconds);
        }
        catch (Exception exDate)
        {
            if (exDate.Message == "Day must be between 1 and 29 for month 12.\r\nParameter name: day")
                return calendar.ToDateTime(year, month, day - 1, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                    timeSpan.Milliseconds);
            throw new Exception("InvalidPersianDate");
        }
    }

    public static string ConvertMiladiToJalali(this DateTime date, bool showTime)
    {
        if (date <= DateTime.MinValue) return "";
        var obj = new PersianCalendar();
        //if (date <= DateTime.MinValue)
        //{
        //    date = new DateTime(622, 3, 21);
        //}
        var day = obj.GetDayOfMonth(date);
        var month = obj.GetMonth(date);
        var year = obj.GetYear(date);
        var hour = obj.GetHour(date);
        var minute = obj.GetMinute(date);
        var second = obj.GetSecond(date);
        var dayStr = obj.GetDayOfMonth(date).CompareTo(10) >= 0 ? day.ToString() : "0" + day;
        var monthStr = obj.GetMonth(date).CompareTo(10) >= 0 ? month.ToString() : "0" + month;
        return showTime ? $"{year}/{monthStr}/{dayStr} {hour}:{minute}:{second}" : $"{year}/{monthStr}/{dayStr}";
    }

    public static string ConvertMiladiToJalali(this DateTime date)
    {
        return ConvertMiladiToJalali(date, false);
    }

    public static string ConvertMiladiToJalali(this DateTime? date)
    {
        return date == null ? string.Empty : ConvertMiladiToJalali((DateTime)date, false);
    }


    public static bool IsValidPersianDate(this string shamsiDate)
    {
        const bool result = false;
        if (shamsiDate == null) return false;
        shamsiDate = shamsiDate.Trim().ToEnglishNumbers();
        shamsiDate = shamsiDate.Replace("-", "/");
        shamsiDate = shamsiDate.Replace(",", "/");
        shamsiDate = shamsiDate.Replace("؍", "/");
        shamsiDate = shamsiDate.Replace(".", "/");
        shamsiDate = shamsiDate.Replace("-", "/");

        var arr = shamsiDate.Split('/');
        if (string.IsNullOrEmpty(shamsiDate)) return result;
        if (arr.Length != 3)
            return result;
        if (!short.TryParse(arr[0], out var year)) return result;
        if (!short.TryParse(arr[1], out var month)) return result;
        if (!short.TryParse(arr[2], out var day)) return result;

        //Check Year
        if (year < 0 || year > 2000)
            //Result = false;
            return false;
        //Check Month
        if (month < 0 || month > 12)
            //Result = false;
            return false;
        //Check Day
        if (day < 0 || day > 31)
            // Result = false;
            return false;
        //Check Valid Day With Month
        if (month < 7)
            //Result = true;
            return true;
        if (month < 12 && month > 6)
        {
            if (day > 30)
                //Result = false;
                return false;
            // Result = true;
            return true;
        }

        if (month != 12) return result;
        return day <= 29;
    }

    public static int GetPersianMonth(this DateTime? date)
    {
        if (date == null) return 0;
        var calendar = new PersianCalendar();
        return calendar.GetMonth(date.Value);
    }

    public static int GetPersianDayOfMonth(this DateTime? date)
    {
        if (date == null) return 0;
        var calendar = new PersianCalendar();
        return calendar.GetDayOfMonth(date.Value);
    }

    public static int GetPersianYear(this DateTime? date)
    {
        if (date == null) return 0;
        var calendar = new PersianCalendar();

        return calendar.GetYear(date.Value);
    }

    public static int GetPersianYear(this DateTime date)
    {
        var calendar = new PersianCalendar();

        return calendar.GetYear(date);
    }

    //TODO :Replace with regular expression
    public static DateTime AddMonths(this string jalaliDate, int period)
    {
        jalaliDate = jalaliDate.Replace(@"‏", string.Empty);
        var parts = jalaliDate.Split('/');
        if (parts.Length == 3)
        {
            var month = Convert.ToInt32(parts[1]);
            var newMonth = month + period;
            var newYear = Convert.ToInt32(parts[0]);
            if (newMonth <= 0)
            {
                newYear--;
                newMonth += 12;
            }
            else if (newMonth > 12)
            {
                newYear++;
                newMonth = newMonth % 12 == 0 ? 12 : newMonth % 12;
            }

            if (newMonth > 6 && newMonth < 12 && parts[2] == "31")
                parts[2] = "30";
            else if (newMonth == 12 && Convert.ToInt32(parts[2]) >= 30) parts[2] = "29";
            return ConvertJalaliToMiladi(newYear + "/" + newMonth + "/" + parts[2]);
        }

        return DateTime.UtcNow;
    }

    public static DateTime SafeMinDate(this DateTime date)
    {
        if (date == DateTime.MinValue) date = new DateTime(1907, 1, 1);
        return date;
    }

    public static bool SafeBool(this object i)
    {
        var b = false;
        if (i != null) bool.TryParse(i.SafeString(), out b);
        return b;
    }

    public static int SafeBoolToInt(this object i)
    {
        var b = SafeBool(i);
        return b ? 1 : 0;
    }

    public static T ConvertToValue<T>(Type type, string value)
    {
        return (T)ConvertToValue(type, value);
    }

    public static object ConvertToValue(Type type, string value)
    {
        if (type == typeof(DateTime))
            try
            {
                return value.SafeString().ConvertJalaliToMiladi().SafeDateTime();
            }
            catch
            {
                return value.SafeString().SafeDateTime();
            }

        if (type == typeof(bool) || type == typeof(bool?))
            return value.SafeString().SafeBool();

        if (type == typeof(float) || type == typeof(float?))
            return value.SafeString().SafeFloat();

        if (type == typeof(decimal) || type == typeof(decimal?))
            return value.SafeString().SafeDecimal();

        if (type == typeof(long) || type == typeof(long?))
            return value.SafeString().SafeLong();

        if (type == typeof(double) || type == typeof(double?))
            return value.SafeString().SafeDouble();

        if (type == typeof(int) || type == typeof(int?))
            return value.SafeString().SafeInt();

        if (type == typeof(short) || type == typeof(short?))
            return value.SafeString().SafeInt16();

        if (type == typeof(byte) || type == typeof(byte?))
            return value.SafeString().SafeByte();

        if (type == typeof(string))
            return value.SafeString().SafePersianEncode().Trim();

        return value;
    }

    public static byte SafeByte(this object i)
    {
        byte val = 0;
        if (i != null) byte.TryParse(i.SafeString().Split('.')[0], out val);

        return val;
    }

    public static int SafeInt(this object i, int exceptionValue)
    {
        if (i != null) int.TryParse(i.SafeString().Split('.')[0], out exceptionValue);

        return exceptionValue;
    }

    public static int SafeInt(this object i)
    {
        return SafeInt(i, -1);
    }

    public static string PersianNumberToLatin(this string number)
    {
        if (number is null) return null;
        var s = number;
        s =
            s.Replace("\u06F0", "0").Replace("\u06F1", "1").Replace("\u06F2", "2").Replace("\u06F3", "3").Replace(
                "\u06F4", "4").Replace("\u06F5", "5").Replace("\u06F6", "6").Replace("\u06F7", "7").Replace(
                "\u06F8", "8").Replace("\u06F9", "9");
        return s;
    }

    public static DateTime ParseDate(string input)
    {
        return DateTime.ParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public static string SafeTrim(this string str)
    {
        if (!string.IsNullOrEmpty(str)) return str.Trim();

        return null;
    }

    public static string SafeString(this object i)
    {
        if (i != null) return i.ToString();

        return null;
    }

    public static string SafeString(this object i, bool isEmpty)
    {
        if (i != null) return i.ToString();

        return string.Empty;
    }

    public static DateTime SafeDateTime(this object d)
    {
        if (d != null)
        {
            DateTime.TryParse(d.SafeString(), out var dt);
            return dt;
        }

        return new DateTime(1907, 1, 1);
    }

    public static TimeSpan SafeTime(this object d)
    {
        if (d != null)
        {
            TimeSpan.TryParse(d.SafeString(), out var dt);
            return dt;
        }

        return TimeSpan.MinValue;
    }

    public static bool SafeBoolean(this object d)
    {
        if (d != null)
        {
            bool.TryParse(d.SafeString(), out var dt);
            return dt;
        }

        return new bool();
    }

    public static Guid SafeGuid(this object d)
    {
        if (d != null)
        {
            Guid.TryParse(d.ToString(), out var g);
            return g;
        }

        return Guid.Empty;
    }

    public static string SafePersianEncode(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        str = str.Replace("ي", "ی");
        str = str.Replace("ك", "ک");
        return str.Trim();
    }

    public static string SafeSqlSingleQuotes(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        str = str.Replace("'", "''");
        return str;
    }

    public static string ReverseString(this string str)
    {
        return new string(str.ToCharArray().Reverse().ToArray());
    }

    public static string RemoveNoise(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        return str.Replace("‏", "");
    }

    public static string SafeSqlDateTime(this object dateTime)
    {
        if (dateTime is DateTime)
            return ((DateTime)dateTime).ToString("yyyy/MM/dd HH:mm:ss");
        return null;
    }

    public static long SafeLong(this object i)
    {
        return SafeLong(i, 0);
    }

    public static long SafeLong(this object i, long exceptionValue)
    {
        if (i == null) return exceptionValue;
        long.TryParse(i.SafeString(), out exceptionValue);
        return exceptionValue;
    }

    public static float SafeFloat(this object i)
    {
        float.TryParse(i.SafeString(), out var id);
        return id;
    }

    public static double SafeDouble(this object i)
    {
        double.TryParse(i.SafeString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var id);
        return id;
    }

    public static double SafeDouble(this object i, double exceptionValue)
    {
        if (i != null) double.TryParse(i.SafeString().Split('.')[0], out exceptionValue);
        return exceptionValue;
    }

    public static short SafeInt16(this object i)
    {
        short id;
        short.TryParse(i.SafeString(), out id);
        return id;
    }

    public static decimal SafeDecimal(this object i)
    {
        decimal id;
        decimal.TryParse(i.SafeString(), out id);
        return id;
    }

    public static TValue SafeDictionary<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key,
        TValue ifNotFound = default)
    {
        TValue val;
        if (input.TryGetValue(key, out val)) return val;

        return ifNotFound;
    }

    public static object SafeReader(this IDataReader reader, string name)
    {
        if (reader != null)
            for (var i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return reader[name];

        return null;
    }

    public static object SafeReader(this IDataRecord reader, string name)
    {
        if (reader == null)
            return null;
        for (var i = 0; i < reader.FieldCount; ++i)
            if (reader.GetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                return reader[name];
        return null;
    }

    public static bool IsCorrectNationalCode(this string nationalCode)
    {
        try
        {
            var chArray = nationalCode.ToCharArray();
            if (chArray.Length != 10) return false;
            var numArray = new int[chArray.Length];
            for (var i = 0; i < chArray.Length; i++) numArray[i] = (int)char.GetNumericValue(chArray[i]);
            var num2 = numArray[9];
            switch (nationalCode)
            {
                case "0000000000":
                case "1111111111":
                case "22222222222":
                case "33333333333":
                case "4444444444":
                case "5555555555":
                case "6666666666":
                case "7777777777":
                case "8888888888":
                case "9999999999":
                    return false;
            }

            var num3 = numArray[0] * 10 + numArray[1] * 9 + numArray[2] * 8 + numArray[3] * 7 + numArray[4] * 6 +
                       numArray[5] * 5 + numArray[6] * 4 + numArray[7] * 3 + numArray[8] * 2;
            var num4 = num3 - num3 / 11 * 11;
            if ((num4 == 0 && num2 == num4) || (num4 == 1 && num2 == 1) || (num4 > 1 && num2 == Math.Abs(num4 - 11)))
                return true;
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}