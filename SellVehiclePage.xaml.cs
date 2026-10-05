namespace EGET.App;

public partial class SellVehiclePage : ContentPage
{
    private readonly List<string> selectedPhotoPaths = new();

    public SellVehiclePage()
    {
        InitializeComponent();
    }

    // ADD VEHICLE PHOTOS
    private async void OnAddPhotosTapped(object sender, TappedEventArgs e)
    {
        var photos = await MediaPicker.Default.PickPhotosAsync();

        if (photos == null)
            return;

        PhotoPreviewLayout.Children.Clear();
        selectedPhotoPaths.Clear();

        foreach (var photo in photos)
        {
            selectedPhotoPaths.Add(photo.FullPath);
            AddPhotoPreview(photo.FullPath);
        }
    }

    // CREATE PHOTO PREVIEW
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
            selectedPhotoPaths.Remove(photoPath);
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
        if (selectedPhotoPaths.Count < 5)
        {
            await DisplayAlert(
                "Missing Photos",
                "Please add at least 5 photos of your vehicle.",
                "OK");

            return;
        }

        // CREATE VEHICLE OBJECT
        var vehicle = new Vehicle
        {
            Brand = BrandEntry.Text?.Trim() ?? "",
            Model = ModelEntry.Text?.Trim() ?? "",
            Year = YearEntry.Text?.Trim() ?? "",
            Price = PriceEntry.Text?.Trim() ?? "",
            Mileage = MileageEntry.Text?.Trim() ?? "",
            Fuel = FuelPicker.SelectedItem?.ToString() ?? "",
            Transmission = TransmissionPicker.SelectedItem?.ToString() ?? "",
            City = CityEntry.Text?.Trim() ?? "",
            Region = RegionEntry.Text?.Trim() ?? "",
            Description = DescriptionEditor.Text?.Trim() ?? "",
            PhotoPaths = new List<string>(selectedPhotoPaths)
        };

        // SAVE VEHICLE
        VehicleStorage.AddVehicle(vehicle);

        // CONFIRM VEHICLE WAS CREATED
        await DisplayAlert(
            "Vehicle Listing",
            $"{vehicle.Brand} {vehicle.Model} listing has been created successfully.",
            "OK");
    }
}