namespace EGET.App;

public partial class MessageSellerPage : ContentPage
{
    public MessageSellerPage()
    {
        InitializeComponent();
    }

    // SEND MESSAGE
    private async void OnSendMessageClicked(object sender, EventArgs e)
    {
        string message = MessageEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(message))
        {
            await DisplayAlert(
                "Message",
                "Please type a message before sending.",
                "OK");

            return;
        }

        // CREATE BUYER MESSAGE
        var messageBubble = new Border
        {
            BackgroundColor = Color.FromArgb("#F7C900"),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(14)
            },
            Padding = 15,
            HorizontalOptions = LayoutOptions.End,
            MaximumWidthRequest = 320
        };

        var messageLabel = new Label
        {
            Text = message,
            FontSize = 15,
            TextColor = Color.FromArgb("#111111")
        };

        messageBubble.Content = messageLabel;

        MessagesLayout.Children.Add(messageBubble);

        // CLEAR MESSAGE BOX
        MessageEntry.Text = "";

        // MOVE TO THE LATEST MESSAGE
        await Task.Delay(100);

        await DisplayAlert(
            "Message Sent",
            "Your message has been sent to the seller.",
            "OK");
    }
}