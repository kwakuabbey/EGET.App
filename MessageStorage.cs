using System.Text.Json;

namespace EGET.App;

public static class MessageStorage
{
    private const string MessagesKey = "eget_messages";

    // GET MESSAGES FOR A VEHICLE
    public static List<Message> GetMessages(string vehicleKey)
    {
        string json = Preferences.Default.Get(MessagesKey, "");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Message>();
        }

        try
        {
            var messages =
                JsonSerializer.Deserialize<List<Message>>(json)
                ?? new List<Message>();

            return messages
                .Where(m => m.VehicleKey == vehicleKey)
                .OrderBy(m => m.SentAt)
                .ToList();
        }
        catch (JsonException)
        {
            return new List<Message>();
        }
    }

    // SAVE A MESSAGE
    public static void AddMessage(Message message)
    {
        var messages = GetAllMessages();

        messages.Add(message);

        string updatedJson = JsonSerializer.Serialize(messages);

        Preferences.Default.Set(MessagesKey, updatedJson);
    }

    // GET ALL MESSAGES
    private static List<Message> GetAllMessages()
    {
        string json = Preferences.Default.Get(MessagesKey, "");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Message>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Message>>(json)
                   ?? new List<Message>();
        }
        catch (JsonException)
        {
            return new List<Message>();
        }
    }

    // CLEAR ALL MESSAGES
    public static void ClearMessages()
    {
        Preferences.Default.Remove(MessagesKey);
    }
}