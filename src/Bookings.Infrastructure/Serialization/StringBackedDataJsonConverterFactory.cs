using System.Text.Json;
using System.Text.Json.Serialization;
using Bookings.Domain.Bookings.Abstractions;

namespace Bookings.Infrastructure.Serialization;

public class StringBackedDataJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.GetInterfaces()
            .Any(i => i.IsGenericType 
                      && i.GetGenericTypeDefinition() == typeof(IStringBackedData<>));

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(StringBackedDataJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}