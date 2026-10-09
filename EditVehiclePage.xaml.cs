using Microsoft.Maui.Media;

namespace EGET.App;

public partial class EditVehiclePage : ContentPage
{
    private readonly Vehicle originalVehicle;
    private readonly List<string> selectedPhotoPaths = new();

    public EditVehiclePage(Vehicle vehicle)
    {
        InitializeComponent();

        // Resolve the vehicle from local storage using its ID.
        var savedVehicle = VehicleStorage.GetVehicles()
            .FirstOrDefault(item => item.Id == vehicle.Id);

        if (savedVehicle == null)
        {
            throw new InvalidOperationException(
                $"Vehicle ID {vehicle.Id} was not found in local storage.");
        }

        originalVehicle = savedVehicle;

        LoadVehicleDetails();
    }
    // LOAD THE EXISTING LISTING
    private void LoadVehicleDetails()
    {
        BrandEntry.Text = originalVehicle.Brand;
        ModelEntry.Text = originalVehicle.Model;
        YearEntry.Text = originalVehicle.Year;
        PriceEntry.Text = originalVehicle.Price;
        MileageEntry.Text = originalVehicle.Mileage;

        FuelPicker.SelectedItem = originalVehicle.Fuel;
        TransmissionPicker.SelectedItem = originalVehicle.Transmission;

        CityEntry.Text = originalVehicle.City;
        RegionEntry.Text = originalVehicle.Region;
        DescriptionEditor.Text = originalVehicle.Description;

        selectedPhotoPaths.Clear();

        if (originalVehicle.PhotoPaths != null)
        {
            selectedPhotoPaths.AddRange(originalVehicle.PhotoPaths);
        }

        RefreshPhotoPreviews();
    }

    // ADD MORE PHOTOS
    // Button.Clicked requires EventArgs.
    private async void OnAddPhotosTapped(object sender, EventArgs e)
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
            await DisplayAlertAsync(
                "Photo Selection",
                $"Unable to select photos: {ex.Message}",
                "OK");
        }
    }

    // REFRESH THE PHOTO PREVIEWS
    private void RefreshPhotoPreviews()
    {
        PhotoPreviewLayout.Children.Clear();

        for (int i = 0; i < selectedPhotoPaths.Count; i++)
        {
            AddPhotoPreview(selectedPhotoPaths[i], i);
        }

        PhotoCountLabel.Text = selectedPhotoPaths.Count == 1
            ? "1 photo selected"
            : $"{selectedPhotoPaths.Count} photos selected";
    }

    // DISPLAY ONE PHOTO AND ITS CONTROLS
    private void AddPhotoPreview(string photoPath, int index)
    {
        var image = new Image
        {
            Source = ImageSource.FromFile(photoPath),
            HeightRequest = 120,
            WidthRequest = 150,
            Aspect = Aspect.AspectFill
        };

        var imageBorder = new Border
        {
            Stroke = Colors.LightGray,
            StrokeThickness = 1,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = 8
            },
            Content = image,
            HeightRequest = 120,
            WidthRequest = 150
        };

        var photoCard = new VerticalStackLayout
        {
            WidthRequest = 155,
            Spacing = 6
        };

        photoCard.Children.Add(imageBorder);

        // MARK THE FIRST PHOTO AS THE MAIN PHOTO
        if (index == 0)
        {
            photoCard.Children.Add(new Label
            {
                Text = "★ MAIN PHOTO",
                TextColor = Color.FromArgb("#B38F00"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center
            });
        }

        // REMOVE PHOTO BUTTON
        var removeButton = new Button
        {
            Text = "Remove",
            BackgroundColor = Color.FromArgb("#8B0000"),
            TextColor = Colors.White,
            FontSize = 12,
            CornerRadius = 8,
            HeightRequest = 36,
            Padding = new Thickness(4, 0)
        };

        removeButton.Clicked += (sender, args) =>
        {
            selectedPhotoPaths.RemoveAt(index);
            RefreshPhotoPreviews();
        };

        photoCard.Children.Add(removeButton);

        // REORDER PHOTO BUTTONS
        var reorderButtons = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 4
        };

        var upButton = new Button
        {
            Text = "↑",
            BackgroundColor = Color.FromArgb("#333333"),
            TextColor = Colors.White,
            FontSize = 16,
            CornerRadius = 8,
            HeightRequest = 36,
            Padding = 0,
            IsEnabled = index > 0
        };

        upButton.Clicked += (sender, args) =>
        {
            MovePhoto(index, index - 1);
        };

        var downButton = new Button
        {
            Text = "↓",
            BackgroundColor = Color.FromArgb("#333333"),
            TextColor = Colors.White,
            FontSize = 16,
            CornerRadius = 8,
            HeightRequest = 36,
            Padding = 0,
            IsEnabled = index < selectedPhotoPaths.Count - 1
        };

        downButton.Clicked += (sender, args) =>
        {
            MovePhoto(index, index + 1);
        };

        reorderButtons.Add(upButton, 0, 0);
        reorderButtons.Add(downButton, 1, 0);

        photoCard.Children.Add(reorderButtons);

        PhotoPreviewLayout.Children.Add(photoCard);
    }

    // MOVE A PHOTO TO A NEW POSITION
    private void MovePhoto(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 ||
            oldIndex >= selectedPhotoPaths.Count ||
            newIndex < 0 ||
            newIndex >= selectedPhotoPaths.Count)
        {
            return;
        }

        string photoPath = selectedPhotoPaths[oldIndex];

        selectedPhotoPaths.RemoveAt(oldIndex);
        selectedPhotoPaths.Insert(newIndex, photoPath);

        RefreshPhotoPreviews();
    }

    // SAVE CHANGES TO THE EXISTING LISTING
    private async void OnSaveChangesClicked(object sender, EventArgs e)
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

        // CHECK REQUIRED FIELDS
        if (string.IsNullOrWhiteSpace(brand) ||
            string.IsNullOrWhiteSpace(model) ||
            string.IsNullOrWhiteSpace(year) ||
            string.IsNullOrWhiteSpace(price) ||
            string.IsNullOrWhiteSpace(mileage) ||
            string.IsNullOrWhiteSpace(fuel) ||
            string.IsNullOrWhiteSpace(transmission) ||
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(region) ||
            string.IsNullOrWhiteSpace(description))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please complete all vehicle details before saving.",
                "OK");

            return;
        }

        // VALIDATE YEAR
        if (!int.TryParse(year, out int yearNumber) ||
            yearNumber < 1886 ||
            yearNumber > DateTime.Now.Year + 1)
        {
            await DisplayAlertAsync(
                "Invalid Year",
                "Please enter a valid vehicle year.",
                "OK");

            return;
        }

        // VALIDATE PRICE
        if (!decimal.TryParse(
                price.Replace(",", ""),
                out decimal priceNumber) ||
            priceNumber <= 0)
        {
            await DisplayAlertAsync(
                "Invalid Price",
                "Please enter a valid price greater than zero.",
                "OK");

            return;
        }

        // VALIDATE MILEAGE
        if (!int.TryParse(
                mileage.Replace(",", ""),
                out int mileageNumber) ||
            mileageNumber < 0)
        {
            await DisplayAlertAsync(
                "Invalid Mileage",
                "Please enter a valid mileage of zero or more.",
                "OK");

            return;
        }

        // REQUIRE AT LEAST FIVE PHOTOS
        if (selectedPhotoPaths.Count < 5)
        {
            await DisplayAlertAsync(
                "Not Enough Photos",
                "Please keep at least five photos for this listing.",
                "OK");

            return;
        }

        SaveChangesButton.IsEnabled = false;

        try
        {
            // KEEP THE ORIGINAL VEHICLE ID.
            // THIS UPDATES THE EXISTING LISTING, NOT A NEW ONE.
            originalVehicle.Brand = brand;
            originalVehicle.Model = model;
            originalVehicle.Year = yearNumber.ToString();
            originalVehicle.Price = priceNumber.ToString("0.##");
            originalVehicle.Mileage = mileageNumber.ToString();
            originalVehicle.Fuel = fuel;
            originalVehicle.Transmission = transmission;
            originalVehicle.City = city;
            originalVehicle.Region = region;
            originalVehicle.Description = description;

            originalVehicle.PhotoPaths =
                new List<string>(selectedPhotoPaths);

            bool updated =
                VehicleStorage.UpdateVehicle(originalVehicle);

            if (!updated)
            {
                await DisplayAlertAsync(
                    "Update Failed",
                    "The listing could not be found in storage. Your changes could not be saved.",
                    "OK");

                return;
            }

            await DisplayAlertAsync(
                "Listing Updated",
                "Your vehicle listing has been updated successfully.",
                "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to update the listing: {ex.Message}",
                "OK");
        }
        finally
        {
            SaveChangesButton.IsEnabled = true;
        }
    }

    // CANCEL EDITING
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        bool leave = await DisplayAlertAsync(
            "Cancel Editing",
            "Discard your unsaved changes?",
            "Yes",
            "No");

        if (leave)
        {
            await Navigation.PopAsync();
        }
    }
}