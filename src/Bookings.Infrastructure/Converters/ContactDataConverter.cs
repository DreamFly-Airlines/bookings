using System.Text.Json;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class ContactDataConverter() : ValueConverter<ContactData, string>(
    data => JsonSerializer.Serialize(data, Options),
    json => JsonSerializer.Deserialize<ContactData>(json, Options))
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new ContactDataJsonConverter() }
    };
}