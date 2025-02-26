using System.Text.RegularExpressions;

namespace LegislativeApp.Application.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return Regex.Replace(input, @"([a-z])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToCsvFilePath<T>(this string directoryPath)
    {
        var fileName = typeof(T).Name.ToSnakeCase() + "s.csv";
        return Path.Combine(directoryPath, fileName);
    }
}