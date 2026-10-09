namespace EGET.App;

public class Message
{
    public string VehicleKey { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public bool IsFromBuyer { get; set; }

    public DateTime SentAt { get; set; } = DateTime.Now;
}