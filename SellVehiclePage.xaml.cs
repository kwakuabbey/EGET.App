namespace EGET.App;

public partial class SellVehiclePage : ContentPage
{
    // STORE SELECTED PHOTO PATHS IN DISPLAY ORDER
    private readonly List<string> selectedPhotoPaths = new();

    public SellVehiclePage()
    {
        InitializeComponent();
    }

    // ADD VEHICLE PHOTOS
    private async void OnAddPhotosTapped(
        object sender,
        TappedEventArgs e)
    {
        try
        {
            var photos = await MediaPicker.Default.PickPhotosAsync();

            if (photos == null || photos.Count == 0)
                return;

            foreach (var photo in photos)
            {
                if (string.IsNullOrWhiteSpace(photo.FullPath))
                    continue;

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
        var image = new Image
        {
            Source = ImageSource.FromFile(photoPath),
            WidthRequest = 120,
            HeightRequest = 120,
            Aspect = Aspect.AspectFill
        };

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

        // REMOVE PHOTO BUTTON
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

        removeButton.Clicked += (sender, e) =>
        {
            selectedPhotoPaths.Remove(photoPath);
            RefreshPhotoPreviews();
        };

        photoContainer.Children.Add(removeButton);
        Grid.SetRow(removeButton, 0);

        var controls = new VerticalStackLayout
        {
            Spacing = 4,
            Padding = new Thickness(0, 6, 0, 0)
        };

        // IDENTIFY THE MAIN PHOTO
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

        // MOVE PHOTO UP
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

        // MOVE PHOTO DOWN
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

        if (currentIndex < 0 ||
            currentIndex >= selectedPhotoPaths.Count ||
            newIndex < 0 ||
            newIndex >= selectedPhotoPaths.Count)
        {
            return;
        }

        // SWAP THE PHOTO POSITIONS
        (
            selectedPhotoPaths[currentIndex],
            selectedPhotoPaths[newIndex]
        ) = (
            selectedPhotoPaths[newIndex],
            selectedPhotoPaths[currentIndex]
        );

        RefreshPhotoPreviews();
    }

    // CREATE VEHICLE LISTING
    private async void OnCreateVehicleListingClicked(
        object sender,
        EventArgs e)
    {
        string brand = BrandEntry.Text?.Trim() ?? string.Empty;
        string model = ModelEntry.Text?.Trim() ?? string.Empty;
        string year = YearEntry.Text?.Trim() ?? string.Empty;
        string price = PriceEntry.Text?.Trim() ?? string.Empty;
        string mileage = MileageEntry.Text?.Trim() ?? string.Empty;

        string fuel =
            FuelPicker.SelectedItem?.ToString() ?? string.Empty;

        string transmission =
            TransmissionPicker.SelectedItem?.ToString() ?? string.Empty;

        string city = CityEntry.Text?.Trim() ?? string.Empty;
        string region = RegionEntry.Text?.Trim() ?? string.Empty;

        string description =
            DescriptionEditor.Text?.Trim() ?? string.Empty;

        // VALIDATE REQUIRED FIELDS
        if (string.IsNullOrWhiteSpace(brand))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle brand.",
                "OK");

            BrandEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle model.",
                "OK");

            ModelEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(year))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle year.",
                "OK");

            YearEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(price))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle price.",
                "OK");

            PriceEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(mileage))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the vehicle mileage.",
                "OK");

            MileageEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(fuel))
        {
            await DisplayAlert(
                "Missing Information",
                "Please select the vehicle fuel type.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(transmission))
        {
            await DisplayAlert(
                "Missing Information",
                "Please select the transmission type.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the city.",
                "OK");

            CityEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(region))
        {
            await DisplayAlert(
                "Missing Information",
                "Please enter the region.",
                "OK");

            RegionEntry.Focus();
            return;
        }

        // VALIDATE DESCRIPTION
        int wordCount = description
            .Split(
                new[] { ' ', '\r', '\n', '\t' },
                StringSplitOptions.RemoveEmptyEntries)
            .Length;

        if (wordCount < 5)
        {
            await DisplayAlert(
                "Description Too Short",
                "Please provide a vehicle description with at least five words.",
                "OK");

            DescriptionEditor.Focus();
            return;
        }

        // REQUIRE AT LEAST FIVE PHOTOS
        if (selectedPhotoPaths.Count < 5)
        {
            await DisplayAlert(
                "Missing Photos",
                "Please add at least five photos of your vehicle.",
                "OK");

            return;
        }

        // CREATE A NEW VEHICLE WITH A UNIQUE ID
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid().ToString(),

            Brand = brand,
            Model = model,
            Year = year,
            Price = price,
            Mileage = mileage,
            Fuel = fuel,
            Transmission = transmission,
            City = city,
            Region = region,
            Description = description,

            // PRESERVE PHOTO ORDER.
            // PHOTO AT INDEX ZERO IS THE MAIN PHOTO.
            PhotoPaths = new List<string>(selectedPhotoPaths)
        };

        try
        {
            // SAVE THE VEHICLE
            VehicleStorage.AddVehicle(vehicle);

            await DisplayAlert(
                "Vehicle Listing",
                $"{vehicle.Brand} {vehicle.Model} listing has been created successfully.",
                "OK");

            // RETURN TO THE PREVIOUS PAGE
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Save Failed",
                $"The vehicle listing could not be saved: {ex.Message}",
                "OK");
        }
    }
}