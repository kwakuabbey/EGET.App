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
            var image = new Image
            {
                Source = ImageSource.FromFile(photo.FullPath),
                WidthRequest = 100,
                HeightRequest = 100,
                Aspect = Aspect.AspectFill
            };

            PhotoPreviewLayout.Children.Add(image);
        }
    }
}