using System.Text.Json;
using Json.Masker.Abstract;

namespace Json.Masker.Tests;

/// <summary>
/// Serializes <see cref="DateTime"/> values using the ISO <c>yyyy-MM-dd</c> format for masking tests.
/// </summary>
public sealed class IsoDateOnlyConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}

/// <summary>
/// Model that exercises masking when a System.Text.Json converter already formats the value.
/// </summary>
public class CustomerWithSystemTextJsonConverter
{
    [System.Text.Json.Serialization.JsonConverter(typeof(IsoDateOnlyConverter))]
    [Sensitive("####-**-**")]
    public DateTime Anniversary { get; set; }
}

/// <summary>
/// Serializes <see cref="DateTime"/> values using the ISO <c>yyyy-MM-dd</c> format for Newtonsoft masking tests.
/// </summary>
public sealed class IsoDateOnlyNewtonsoftConverter : global::Newtonsoft.Json.JsonConverter<DateTime>
{
    public override void WriteJson(
        global::Newtonsoft.Json.JsonWriter writer,
        DateTime value,
        global::Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("yyyy-MM-dd"));
    }

    public override DateTime ReadJson(
        global::Newtonsoft.Json.JsonReader reader,
        Type objectType,
        DateTime existingValue,
        bool hasExistingValue,
        global::Newtonsoft.Json.JsonSerializer serializer) => throw new NotSupportedException();
}

/// <summary>
/// Model that exercises masking when a Newtonsoft.Json converter already formats the value.
/// </summary>
public sealed class CustomerWithNewtonsoftConverter
{
    [global::Newtonsoft.Json.JsonConverter(typeof(IsoDateOnlyNewtonsoftConverter))]
    [Sensitive("####-**-**")]
    public DateTime Anniversary { get; set; }
}
