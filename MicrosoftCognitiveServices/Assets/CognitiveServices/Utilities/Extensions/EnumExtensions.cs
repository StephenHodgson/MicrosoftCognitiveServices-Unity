using System.Text;

public static class EnumExtensions
{
    private static readonly StringBuilder stringBuilder = new StringBuilder();
    public static string ToStringCamelCase<T>(this T enumType)
    {
        var typeName = enumType.ToString();
        stringBuilder.Clear();
        stringBuilder.Append(char.ToLowerInvariant(typeName[0]));
        stringBuilder.Append(typeName.Substring(1));
        return stringBuilder.ToString();
    }
}
