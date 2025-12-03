using System.Text.Json;
using System.Text.Json.Serialization;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Infrastructure.Serialization;

public class ContactDataJsonConverter : JsonConverter<ContactData>
{
    private const string PhoneNumberName = "phone";
    private const string EmailName = "email";
    
    public override ContactData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected {nameof(JsonTokenType.StartObject)} token.");
        var properties = new Dictionary<string, string?>
        {
            [PhoneNumberName] = null,
            [EmailName] = null
        };
        FillPropertiesValues(properties, ref reader);
        var phoneNumberString = properties[PhoneNumberName];
        var emailString = properties[EmailName];
        
        var email = emailString is null 
            ? (Email?)null 
            : Email.FromString(emailString);
        var phoneNumber = phoneNumberString is null 
            ? (PhoneNumber?)null 
            : PhoneNumber.FromString(phoneNumberString);
        return new ContactData(email: email, phoneNumber: phoneNumber);
    }

    public override void Write(Utf8JsonWriter writer, ContactData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.PhoneNumber.HasValue)
            writer.WriteString(PhoneNumberName, value.PhoneNumber);

        if (value.Email.HasValue)
            writer.WriteString(EmailName, value.Email);

        writer.WriteEndObject();
    }

    private static void FillPropertiesValues(Dictionary<string, string?> properties, ref Utf8JsonReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException(
                    $"Expected {nameof(JsonTokenType.PropertyName)} token, got {reader.TokenType}.");

            var propertyName = reader.GetString()!;
            if (!properties.ContainsKey(propertyName))
                throw new JsonException($"Unexpected {nameof(propertyName)}: {propertyName}.");
            if (!reader.Read())
                throw new JsonException("Unexpected end when reading property value.");
            
            var propertyValue = reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.String => reader.GetString(),
                _ => throw new JsonException($"Unexpected token for {propertyName}: {reader.TokenType}.")
            };
            properties[propertyName] = propertyValue;
        }
    }
}
