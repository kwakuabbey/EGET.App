namespace EGET.App;

public partial class SellVehiclePage : ContentPage
{
    public SellVehiclePage()
    {
        InitializeComponent();
    }

    private async void OnAddPhotosTapped(object sender, TappedEventArgs e)
    {
        var photos = await MediaPicker.Default.PickPhotosAsync();

        if (photos == null)
            return;

        PhotoPreviewLayout.Children.Clear();

        foreach (var photo in photos)
        {
            AddPhotoPreview(photo.FullPath);
        }
    }

    private void AddPhotoPreview(string photoPath)
    {
        // PHOTO IMAGE
        var image = new Image
        {
            Source = ImageSource.FromFile(photoPath),
            WidthRequest = 120,
            HeightRequest = 120,
            Aspect = Aspect.AspectFill
        };

        // PHOTO FRAME
        var imageFrame = new Border
        {
            WidthRequest = 120,
            HeightRequest = 120,
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#F6C800"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            },
            Content = image
        };

        // PHOTO CONTAINER
        var photoContainer = new Grid
        {
            WidthRequest = 120,
            HeightRequest = 120
        };

        // ADD PHOTO TO CONTAINER
        photoContainer.Children.Add(imageFrame);

        // REMOVE BUTTON
        var removeButton = new Button
        {
            Text = "×",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            BackgroundColor = Color.FromArgb("#CC0000"),
            WidthRequest = 30,
            HeightRequest = 30,
            CornerRadius = 15,
            Padding = 0,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start
        };

        // REMOVE PHOTO WHEN × IS PRESSED
        removeButton.Clicked += (sender, e) =>
        {
            PhotoPreviewLayout.Children.Remove(photoContainer);
        };

        // ADD BUTTON ON TOP OF PHOTO
        photoContainer.Children.Add(removeButton);

        // ADD COMPLETE PHOTO CONTAINER TO PREVIEW AREA
        PhotoPreviewLayout.Children.Add(photoContainer);
    }
}