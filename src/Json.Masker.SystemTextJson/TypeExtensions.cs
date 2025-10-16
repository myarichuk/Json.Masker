namespace Json.Masker.SystemTextJson;

/// <summary>
/// Helper methods for working with <see cref="Type"/> instances inside the System.Text.Json integration.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Determines whether the provided type can be treated as a scalar for masking purposes.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns><see langword="true"/> when the type is a primitive, string, or other known scalar type; otherwise, <see langword="false"/>.</returns>
    internal static bool IsProperPrimitive(this Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(Guid) ||
               type.IsEnum;
    }
}
