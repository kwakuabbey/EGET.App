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

        // REMOVE PHOTO
        removeButton.Clicked += (sender, e) =>
        {
            PhotoPreviewLayout.Children.Remove(photoContainer);
        };

        photoContainer.Children.Add(removeButton);

        PhotoPreviewLayout.Children.Add(photoContainer);
    }

    // CREATE VEHICLE LISTING
    private async void OnCreateVehicleListingClicked(object sender, EventArgs e)
    {
        // CHECK BRAND
        if (string.IsNullOrWhiteSpace(BrandEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle brand.",
                "OK");

            BrandEntry.Focus();
            return;
        }

        // CHECK MODEL
        if (string.IsNullOrWhiteSpace(ModelEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle model.",
                "OK");

            ModelEntry.Focus();
            return;
        }

        // CHECK YEAR
        if (string.IsNullOrWhiteSpace(YearEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle year.",
                "OK");

            YearEntry.Focus();
            return;
        }

        // CHECK PRICE
        if (string.IsNullOrWhiteSpace(PriceEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle price.",
                "OK");

            PriceEntry.Focus();
            return;
        }

        // CHECK MILEAGE
        if (string.IsNullOrWhiteSpace(MileageEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle mileage.",
                "OK");

            MileageEntry.Focus();
            return;
        }

        // CHECK CITY
        if (string.IsNullOrWhiteSpace(CityEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the city.",
                "OK");

            CityEntry.Focus();
            return;
        }

        // CHECK REGION
        if (string.IsNullOrWhiteSpace(RegionEntry.Text))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the region.",
                "OK");

            RegionEntry.Focus();
            return;
        }

        // CHECK DESCRIPTION
        string description = DescriptionEditor.Text?.Trim() ?? "";

        int wordCount = description
            .Split(
                new[] { ' ', '\r', '\n', '\t' },
                StringSplitOptions.RemoveEmptyEntries)
            .Length;

        if (wordCount < 5)
        {
            await DisplayAlert(
                "Missing Information",
                "Please provide a vehicle description with at least 5 words.",
                "OK");

            DescriptionEditor.Focus();
            return;
        }

        // CHECK PHOTOS
        if (PhotoPreviewLayout.Children.Count < 5)
        {
            await DisplayAlert(
                "Missing Photos",
                "Please add at least 5 photos of your vehicle.",
                "OK");

            return;
        }

        // ALL REQUIRED INFORMATION IS VALID
        await DisplayAlert(
            "Vehicle Listing",
            "All required vehicle information has been entered successfully.",
            "OK");
    }
}