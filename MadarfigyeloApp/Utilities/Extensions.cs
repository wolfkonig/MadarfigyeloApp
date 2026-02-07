using MadarfigyeloApp.Resources;

public static class EnumExtensions
{
    /// <summary>
    /// Tries to find a matching text resource in AppRes.resx
    /// Using the Enum value in the parameter as key
    /// If not found, returns the Enum value as string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetResxText(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (TryGetResxText(value, out string? text) && text is not null)
        {
            return text;
        }

        return value.ToString();
    }

    public static bool TryGetResxText(this Enum value, out string? text)
    {
        text = AppRes.ResourceManager.GetString(value.ToString(), AppRes.Culture);
        return text != null;
    }
}