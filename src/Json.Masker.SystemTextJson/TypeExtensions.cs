namespace Json.Masker.SystemTextJson;

internal static class TypeExtensions
{
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