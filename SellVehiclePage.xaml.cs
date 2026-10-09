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
        try
        {
            var photos = await MediaPicker.Default.PickPhotosAsync();

            if (photos == null || photos.Count == 0)
                return;

            // Add new photos without removing previously selected photos.
            foreach (var photo in photos)
            {
                if (!selectedPhotoPaths.Contains(photo.FullPath))
                {
                    selectedPhotoPaths.Add(photo.FullPath);
                }
            }

            RefreshPhotoPreviews();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Photo Selection",
                $"Unable to select photos: {ex.Message}",
                "OK");
        }
    }

    // REFRESH ALL PHOTO PREVIEWS
    private void RefreshPhotoPreviews()
    {
        PhotoPreviewLayout.Children.Clear();

        for (int i = 0; i < selectedPhotoPaths.Count; i++)
        {
            AddPhotoPreview(selectedPhotoPaths[i], i);
        }
    }

    // CREATE PHOTO PREVIEW
    private void AddPhotoPreview(string photoPath, int index)
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
            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(12)
                },
            Content = image
        };

        // PHOTO CONTAINER
        var photoContainer = new Grid
        {
            WidthRequest = 130,
            RowDefinitions =
            {
                new RowDefinition { Height = 120 },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        photoContainer.Children.Add(imageFrame);
        Grid.SetRow(imageFrame, 0);

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
            selectedPhotoPaths.Remove(photoPath);
            RefreshPhotoPreviews();
        };

        photoContainer.Children.Add(removeButton);
        Grid.SetRow(removeButton, 0);

        // PHOTO ORDER CONTROLS
        var controls = new VerticalStackLayout
        {
            Spacing = 4,
            Padding = new Thickness(0, 6, 0, 0)
        };

        // MAIN PHOTO LABEL
        if (index == 0)
        {
            controls.Children.Add(new Label
            {
                Text = "★ MAIN PHOTO",
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#F6C800"),
                HorizontalTextAlignment = TextAlignment.Center
            });
        }
        else
        {
            controls.Children.Add(new Label
            {
                Text = $"Photo {index + 1}",
                FontSize = 11,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center
            });
        }

        // MOVE UP BUTTON
        var moveUpButton = new Button
        {
            Text = "↑ Move Up",
            FontSize = 11,
            Padding = new Thickness(2),
            HeightRequest = 34,
            BackgroundColor = Color.FromArgb("#333333"),
            TextColor = Colors.White,
            IsEnabled = index > 0
        };

        moveUpButton.Clicked += (sender, e) =>
        {
            MovePhoto(index, -1);
        };

        controls.Children.Add(moveUpButton);

        // MOVE DOWN BUTTON
        var moveDownButton = new Button
        {
            Text = "↓ Move Down",
            FontSize = 11,
            Padding = new Thickness(2),
            HeightRequest = 34,
            BackgroundColor = Color.FromArgb("#333333"),
            TextColor = Colors.White,
            IsEnabled = index < selectedPhotoPaths.Count - 1
        };

        moveDownButton.Clicked += (sender, e) =>
        {
            MovePhoto(index, 1);
        };

        controls.Children.Add(moveDownButton);

        photoContainer.Children.Add(controls);
        Grid.SetRow(controls, 1);

        PhotoPreviewLayout.Children.Add(photoContainer);
    }

    // MOVE A PHOTO UP OR DOWN
    private void MovePhoto(int currentIndex, int direction)
    {
        int newIndex = currentIndex + direction;

        if (newIndex < 0 || newIndex >= selectedPhotoPaths.Count)
            return;

        // Swap the positions of the selected photos.
        (selectedPhotoPaths[currentIndex], selectedPhotoPaths[newIndex]) =
            (selectedPhotoPaths[newIndex], selectedPhotoPaths[currentIndex]);

        // Refresh the previews to display the new order.
        RefreshPhotoPreviews();
    }

    // CREATE VEHICLE LISTING
    private async void OnCreateVehicleListingClicked(
        object sender,
        EventArgs e)
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
            Description = description,

            // Preserve the selected photo order.
            // The first photo is the intended main photo.
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