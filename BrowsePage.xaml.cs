namespace EGET.App;

public partial class BrowsePage : ContentPage, IQueryAttributable
{
    private List<Vehicle> allVehicles = new();

    private bool isRefreshingFilters;

    private string pendingSearchText = string.Empty;

    // RECEIVE SEARCH TEXT FROM THE HOME PAGE
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("SearchText", out object? value))
        {
            pendingSearchText = value?.ToString() ?? string.Empty;
        }
    }

    public BrowsePage()
    {
        InitializeComponent();

        LoadFilterOptions();
    }

    // REFRESH VEHICLES WHEN THE PAGE OPENS OR REAPPEARS
    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadVehicles();

        if (!string.IsNullOrWhiteSpace(pendingSearchText))
        {
            SearchEntry.Text = pendingSearchText;
            pendingSearchText = string.Empty;
        }
    }

    // LOAD FILTER OPTIONS
    private void LoadFilterOptions()
    {
        isRefreshingFilters = true;

        try
        {
            BrandPicker.Items.Clear();
            BrandPicker.Items.Add("All brands");

            string[] brands =
            {
                "Toyota",
                "Mercedes-Benz",
                "Hyundai",
                "Kia",
                "Honda",
                "Nissan",
                "Lexus",
                "BMW",
                "Volkswagen",
                "Ford",
                "Mitsubishi",
                "Mazda",
                "Subaru",
                "Audi",
                "Peugeot",
                "Renault",
                "Volvo",
                "Jeep",
                "Land Rover",
                "Range Rover",
                "Suzuki",
                "Isuzu",
                "Chevrolet",
                "Dodge",
                "Porsche",
                "Tesla",
                "BYD"
            };

            foreach (string brand in brands)
            {
                BrandPicker.Items.Add(brand);
            }

            BrandPicker.SelectedIndex = 0;

            YearPicker.Items.Clear();
            YearPicker.Items.Add("All years");

            for (int year = DateTime.Now.Year; year >= 1990; year--)
            {
                YearPicker.Items.Add(year.ToString());
            }

            YearPicker.SelectedIndex = 0;

            FuelPicker.Items.Clear();

            string[] fuelTypes =
            {
                "All fuel types",
                "Petrol",
                "Diesel",
                "Hybrid",
                "Electric",
                "LPG",
                "CNG"
            };

            foreach (string fuel in fuelTypes)
            {
                FuelPicker.Items.Add(fuel);
            }

            FuelPicker.SelectedIndex = 0;

            TransmissionPicker.Items.Clear();

            string[] transmissions =
            {
                "All transmissions",
                "Automatic",
                "Manual",
                "CVT",
                "Semi-Automatic"
            };

            foreach (string transmission in transmissions)
            {
                TransmissionPicker.Items.Add(transmission);
            }

            TransmissionPicker.SelectedIndex = 0;
        }
        finally
        {
            isRefreshingFilters = false;
        }
    }

    // LOAD SAVED VEHICLES
    private void LoadVehicles()
    {
        allVehicles = VehicleStorage.GetVehicles()
            .Where(vehicle =>
                !string.Equals(
                    vehicle.Status,
                    "Draft",
                    StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    vehicle.Status,
                    "Sold",
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        ApplyFilters();
    }

    // SEARCH BY BRAND, MODEL, YEAR OR LOCATION
    private void OnSearchTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    // APPLY FILTERS WHEN A PICKER OR LOCATION CHANGES
    private void OnFilterChanged(
        object sender,
        EventArgs e)
    {
        if (isRefreshingFilters)
        {
            return;
        }

        ApplyFilters();
    }

    // APPLY PRICE FILTERS
    private void OnPriceChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    // CLEAR ALL FILTERS
    private void OnClearFiltersClicked(
        object sender,
        EventArgs e)
    {
        SearchEntry.Text = string.Empty;

        MinimumPriceEntry.Text = string.Empty;
        MaximumPriceEntry.Text = string.Empty;

        LocationEntry.Text = string.Empty;

        isRefreshingFilters = true;

        try
        {
            BrandPicker.SelectedIndex = 0;
            YearPicker.SelectedIndex = 0;
            FuelPicker.SelectedIndex = 0;
            TransmissionPicker.SelectedIndex = 0;
        }
        finally
        {
            isRefreshingFilters = false;
        }

        ApplyFilters();
    }

    // FILTER VEHICLES
    private void ApplyFilters()
    {
        if (VehicleResultsLayout == null)
        {
            return;
        }

        string search = SearchEntry.Text?.Trim() ?? string.Empty;

        string location = LocationEntry.Text?.Trim() ?? string.Empty;

        string selectedBrand =
            BrandPicker.SelectedIndex > 0
                ? BrandPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        string selectedYear =
            YearPicker.SelectedIndex > 0
                ? YearPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        string selectedFuel =
            FuelPicker.SelectedIndex > 0
                ? FuelPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        string selectedTransmission =
            TransmissionPicker.SelectedIndex > 0
                ? TransmissionPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        decimal? minimumPrice = ParsePrice(MinimumPriceEntry.Text);
        decimal? maximumPrice = ParsePrice(MaximumPriceEntry.Text);

        IEnumerable<Vehicle> filteredVehicles = allVehicles;

        // SEARCH BRAND, MODEL, YEAR AND LOCATION
        if (!string.IsNullOrWhiteSpace(search))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                ContainsText(vehicle.Brand, search)
                ||
                ContainsText(vehicle.Model, search)
                ||
                ContainsText(vehicle.Year, search)
                ||
                ContainsText(vehicle.City, search)
                ||
                ContainsText(vehicle.Region, search));
        }

        // BRAND FILTER
        if (!string.IsNullOrWhiteSpace(selectedBrand))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Brand?.Trim(),
                    selectedBrand,
                    StringComparison.OrdinalIgnoreCase));
        }

        // YEAR FILTER
        if (!string.IsNullOrWhiteSpace(selectedYear))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Year?.Trim(),
                    selectedYear,
                    StringComparison.OrdinalIgnoreCase));
        }

        // LOCATION FILTER
        if (!string.IsNullOrWhiteSpace(location))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                ContainsText(vehicle.City, location)
                ||
                ContainsText(vehicle.Region, location));
        }

        // FUEL FILTER
        if (!string.IsNullOrWhiteSpace(selectedFuel))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Fuel?.Trim(),
                    selectedFuel,
                    StringComparison.OrdinalIgnoreCase));
        }

        // TRANSMISSION FILTER
        if (!string.IsNullOrWhiteSpace(selectedTransmission))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Transmission?.Trim(),
                    selectedTransmission,
                    StringComparison.OrdinalIgnoreCase));
        }

        // MINIMUM PRICE FILTER
        if (minimumPrice.HasValue)
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
            {
                decimal? price = ParsePrice(vehicle.Price);

                return price.HasValue && price.Value >= minimumPrice.Value;
            });
        }

        // MAXIMUM PRICE FILTER
        if (maximumPrice.HasValue)
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
            {
                decimal? price = ParsePrice(vehicle.Price);

                return price.HasValue && price.Value <= maximumPrice.Value;
            });
        }

        DisplayVehicles(filteredVehicles.ToList());
    }

    // DISPLAY MATCHING VEHICLE CARDS
    private void DisplayVehicles(List<Vehicle> vehicles)
    {
        VehicleResultsLayout.Children.Clear();

        ResultsCountLabel.Text =
            vehicles.Count == 1
                ? "1 car"
                : $"{vehicles.Count} cars";

        EmptyResultsLayout.IsVisible = vehicles.Count == 0;

        foreach (Vehicle vehicle in vehicles)
        {
            VehicleResultsLayout.Children.Add(
                CreateVehicleCard(vehicle));
        }
    }

    // CREATE A VEHICLE CARD
    private View CreateVehicleCard(Vehicle vehicle)
    {
        var cardLayout = new VerticalStackLayout
        {
            Spacing = 0
        };

        string photoPath =
            vehicle.PhotoPaths?.FirstOrDefault(path =>
                !string.IsNullOrWhiteSpace(path))
            ?? "corolla1.png";

        var vehicleImage = new Image
        {
            Source = photoPath,
            HeightRequest = 190,
            Aspect = Aspect.AspectFill
        };

        var imageBorder = new Border
        {
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(15, 15, 0, 0)
            },
            Content = vehicleImage
        };

        string vehicleName =
            $"{vehicle.Year} {vehicle.Brand} {vehicle.Model}".Trim();

        var nameLabel = new Label
        {
            Text = vehicleName,
            FontSize = 19,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            LineBreakMode = LineBreakMode.WordWrap
        };

        var priceLabel = new Label
        {
            Text = $"GH₵ {FormatPrice(vehicle.Price)}",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#F7C900")
        };

        var informationLabel = new Label
        {
            Text =
                $"{vehicle.Transmission} • {vehicle.Fuel} • {vehicle.City}",
            FontSize = 14,
            TextColor = Color.FromArgb("#BBBBBB")
        };

        var detailsButton = new Button
        {
            Text = "View Details",
            BackgroundColor = Color.FromArgb("#F7C900"),
            TextColor = Colors.Black,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 10,
            Margin = new Thickness(0, 8, 0, 0)
        };

        detailsButton.Clicked += async (sender, args) =>
        {
            await Shell.Current.Navigation.PushAsync(
                new VehicleDetailsPage(vehicle));
        };

        var detailsLayout = new VerticalStackLayout
        {
            Padding = 15,
            Spacing = 5,
            Children =
            {
                nameLabel,
                priceLabel,
                informationLabel,
                detailsButton
            }
        };

        cardLayout.Children.Add(imageBorder);
        cardLayout.Children.Add(detailsLayout);

        return new Border
        {
            BackgroundColor = Color.FromArgb("#1F1F1F"),
            Stroke = Color.FromArgb("#F7C900"),
            StrokeThickness = 1,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = 16
            },
            Content = cardLayout
        };
    }

    // CHECK WHETHER TEXT CONTAINS A SEARCH TERM
    private bool ContainsText(string? value, string search)
    {
        return !string.IsNullOrWhiteSpace(value)
            &&
            value.Contains(
                search,
                StringComparison.OrdinalIgnoreCase);
    }

    // PARSE PRICE VALUES
    private decimal? ParsePrice(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string cleaned = value
            .Replace("GH₵", "")
            .Replace("GHS", "")
            .Replace(",", "")
            .Replace(" ", "")
            .Trim();

        if (decimal.TryParse(
            cleaned,
            System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal amount))
        {
            return amount;
        }

        return null;
    }

    // FORMAT VEHICLE PRICE
    private string FormatPrice(string? price)
    {
        decimal? amount = ParsePrice(price);

        return amount.HasValue
            ? amount.Value.ToString(
                "N0",
                System.Globalization.CultureInfo.InvariantCulture)
            : price ?? string.Empty;
    }
}