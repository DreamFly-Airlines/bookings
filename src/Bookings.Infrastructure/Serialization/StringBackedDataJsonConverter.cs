using System.Text.Json;
using System.Text.Json.Serialization;
using Bookings.Domain.Bookings.Abstractions;

namespace Bookings.Infrastructure.Serialization;

public class StringBackedDataJsonConverter<T> : JsonConverter<T> where T : struct, IStringBackedData<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions _)
        => T.FromString(reader.GetString() ??  throw new JsonException(
            $"Expected string convertable to {typeof(T)} via \"{nameof(T.FromString)}()\" method, got null instead."));

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions _)
        => writer.WriteStringValue(value.ToString());
}