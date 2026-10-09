namespace EGET.App;

[QueryProperty(nameof(SelectedVehicle), "SelectedVehicle")]
public partial class MessageSellerPage : ContentPage
{
    private Vehicle? selectedVehicle;
    private string vehicleKey = string.Empty;

    public Vehicle? SelectedVehicle
    {
        get => selectedVehicle;

        set
        {
            selectedVehicle = value;

            if (selectedVehicle != null)
            {
                vehicleKey = GenerateVehicleKey(selectedVehicle);

                Title = $"{selectedVehicle.Brand} {selectedVehicle.Model}";

                LoadMessages();
            }
        }
    }

    public MessageSellerPage()
    {
        InitializeComponent();
    }

    // GENERATE A CONVERSATION KEY FOR EACH VEHICLE
    private string GenerateVehicleKey(Vehicle vehicle)
    {
        return $"{vehicle.Brand.Trim().ToLowerInvariant()}_" +
               $"{vehicle.Model.Trim().ToLowerInvariant()}_" +
               $"{vehicle.Year.Trim()}_" +
               $"{vehicle.Price.Trim()}";
    }

    // LOAD PREVIOUS MESSAGES
    private void LoadMessages()
    {
        MessagesLayout.Children.Clear();

        var messages = MessageStorage.GetMessages(vehicleKey);

        if (messages.Count == 0)
        {
            AddSellerMessage(
                "Hello! How can I help you with this vehicle?");

            return;
        }

        foreach (var message in messages)
        {
            AddMessageBubble(message);
        }
    }

    // DISPLAY SELLER GREETING
    private void AddSellerMessage(string text)
    {
        var messageBubble = new Border
        {
            BackgroundColor = Color.FromArgb("#1F1F1F"),
            StrokeThickness = 0,
            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(14)
                },
            Padding = 15,
            HorizontalOptions = LayoutOptions.Start,
            MaximumWidthRequest = 320
        };

        messageBubble.Content = new Label
        {
            Text = text,
            FontSize = 15,
            TextColor = Colors.White
        };

        MessagesLayout.Children.Add(messageBubble);
    }

    // DISPLAY A SAVED MESSAGE
    private void AddMessageBubble(Message message)
    {
        var messageBubble = new Border
        {
            BackgroundColor = message.IsFromBuyer
                ? Color.FromArgb("#F7C900")
                : Color.FromArgb("#1F1F1F"),

            StrokeThickness = 0,

            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(14)
                },

            Padding = 15,

            HorizontalOptions = message.IsFromBuyer
                ? LayoutOptions.End
                : LayoutOptions.Start,

            MaximumWidthRequest = 320
        };

        messageBubble.Content = new Label
        {
            Text = message.Text,
            FontSize = 15,
            TextColor = message.IsFromBuyer
                ? Color.FromArgb("#111111")
                : Colors.White
        };

        MessagesLayout.Children.Add(messageBubble);
    }

    // SEND AND SAVE MESSAGE
    private async void OnSendMessageClicked(
        object sender,
        EventArgs e)
    {
        if (selectedVehicle == null ||
            string.IsNullOrWhiteSpace(vehicleKey))
        {
            await DisplayAlert(
                "Conversation Unavailable",
                "Please open Contact Seller from a vehicle listing.",
                "OK");

            return;
        }

        string messageText = MessageEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(messageText))
        {
            await DisplayAlert(
                "Message",
                "Please type a message before sending.",
                "OK");

            return;
        }

        var message = new Message
        {
            VehicleKey = vehicleKey,
            Text = messageText,
            IsFromBuyer = true,
            SentAt = DateTime.Now
        };

        MessageStorage.AddMessage(message);

        AddMessageBubble(message);

        MessageEntry.Text = "";
    }
}