using System.Globalization;

namespace Json.Masker.Abstract;

/// <summary>
/// Provides helper extensions for converting values to invariant string representations.
/// </summary>
public static class UtilExtensions
{
    /// <summary>
    /// Attempts to convert the provided value into its invariant string representation.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="str">When this method returns, contains the converted string if the conversion was successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when conversion succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToString<T>(this T? value, out string? str)
    {
        str = value.ToInvariantString();
        return str != null;
    }

    /// <summary>
    /// Converts a value to an invariant string when possible.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The invariant string representation, or <see langword="null"/> when conversion is not possible.</returns>
    private static string? ToInvariantString<T>(this T? value)
    {
        switch (value)
        {
            case null:
                return null;
            case string s:
                return s;
            case DateTime dt:
                return dt.ToString("o", CultureInfo.InvariantCulture);
            case DateTimeOffset dto:
                return dto.ToString("o", CultureInfo.InvariantCulture);
            case TimeSpan ts:
                return ts.ToString("c", CultureInfo.InvariantCulture);
            case double d:
                return d.ToString("R", CultureInfo.InvariantCulture);
            case float f:
                return f.ToString("R", CultureInfo.InvariantCulture);
            case DBNull:
                return null;
            case byte[] b: // really doubt this will be required, just in case :)
                return Convert.ToBase64String(b);
        }

        if (value is IFormattable formattable)
        {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }

        if (value is IConvertible convertible)
        {
            return convertible.ToString(CultureInfo.InvariantCulture);
        }
        
        // just in case: if ToString isn't overridden - don't care
        var type = value.GetType();
        var toStringDeclaring = type.GetMethod(nameof(ToString), Type.EmptyTypes)?.DeclaringType;
        var str = value.ToString();
        return toStringDeclaring == typeof(object) ? null : str;
    }
}
