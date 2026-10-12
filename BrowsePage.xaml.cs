using System.Globalization;

namespace EGET.App;

public partial class BrowsePage : ContentPage, IQueryAttributable
{
    // ALL SAVED VEHICLES AVAILABLE FOR BROWSING
    private List<Vehicle> allVehicles = new();

    // PREVENT FILTER EVENTS DURING INITIALIZATION
    private bool isRefreshingFilters;

    // SEARCH TEXT RECEIVED FROM THE HOME PAGE
    private string pendingSearchText = string.Empty;


    // INITIALIZE THE PAGE
    public BrowsePage()
    {
        InitializeComponent();

        LoadFilterOptions();
    }


    // RECEIVE SEARCH TEXT FROM THE HOME PAGE
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("SearchText", out object? value))
        {
            pendingSearchText = value?.ToString() ?? string.Empty;
        }
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


    // LOAD ALL FILTER AND SORTING OPTIONS
    private void LoadFilterOptions()
    {
        isRefreshingFilters = true;

        try
        {
            // BRAND OPTIONS
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


            // MODEL YEAR OPTIONS
            YearPicker.Items.Clear();
            YearPicker.Items.Add("All years");

            for (int year = DateTime.Now.Year; year >= 1990; year--)
            {
                YearPicker.Items.Add(year.ToString());
            }

            YearPicker.SelectedIndex = 0;


            // FUEL OPTIONS
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


            // TRANSMISSION OPTIONS
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


            // SORTING OPTIONS
            SortPicker.Items.Clear();

            string[] sortOptions =
            {
                "Newest First",
                "Price: Low to High",
                "Price: High to Low"
            };

            foreach (string option in sortOptions)
            {
                SortPicker.Items.Add(option);
            }

            SortPicker.SelectedIndex = 0;
        }
        finally
        {
            isRefreshingFilters = false;
        }
    }


    // LOAD SAVED ACTIVE VEHICLES
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


    // APPLY BRAND, YEAR, LOCATION, FUEL AND TRANSMISSION FILTERS
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


    // UPDATE RESULTS WHEN THE SORTING OPTION CHANGES
    private void OnSortChanged(
        object sender,
        EventArgs e)
    {
        if (isRefreshingFilters)
        {
            return;
        }

        ApplyFilters();
    }


    // UPDATE RESULTS WHEN THE PRICE RANGE CHANGES
    private void OnPriceChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilters();
    }


    // CLEAR ALL SEARCH AND FILTER OPTIONS
    private void OnClearFiltersClicked(
        object sender,
        EventArgs e)
    {
        isRefreshingFilters = true;

        try
        {
            SearchEntry.Text = string.Empty;

            MinimumPriceEntry.Text = string.Empty;
            MaximumPriceEntry.Text = string.Empty;

            LocationEntry.Text = string.Empty;

            BrandPicker.SelectedIndex = 0;
            YearPicker.SelectedIndex = 0;
            FuelPicker.SelectedIndex = 0;
            TransmissionPicker.SelectedIndex = 0;

            SortPicker.SelectedIndex = 0;
        }
        finally
        {
            isRefreshingFilters = false;
        }

        ApplyFilters();
    }


    // APPLY ALL VEHICLE FILTERS AND SORTING
    private void ApplyFilters()
    {
        if (VehicleResultsLayout == null)
        {
            return;
        }

        // SEARCH TEXT
        string search = SearchEntry.Text?.Trim() ?? string.Empty;

        // LOCATION
        string location = LocationEntry.Text?.Trim() ?? string.Empty;

        // SELECTED BRAND
        string selectedBrand =
            BrandPicker.SelectedIndex > 0
                ? BrandPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        // SELECTED YEAR
        string selectedYear =
            YearPicker.SelectedIndex > 0
                ? YearPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        // SELECTED FUEL
        string selectedFuel =
            FuelPicker.SelectedIndex > 0
                ? FuelPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        // SELECTED TRANSMISSION
        string selectedTransmission =
            TransmissionPicker.SelectedIndex > 0
                ? TransmissionPicker.SelectedItem?.ToString() ?? string.Empty
                : string.Empty;

        // PRICE RANGE
        decimal? minimumPrice = ParsePrice(MinimumPriceEntry.Text);
        decimal? maximumPrice = ParsePrice(MaximumPriceEntry.Text);


        // START WITH ALL ACTIVE VEHICLES
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


        // FILTER BY BRAND
        if (!string.IsNullOrWhiteSpace(selectedBrand))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Brand?.Trim(),
                    selectedBrand,
                    StringComparison.OrdinalIgnoreCase));
        }


        // FILTER BY YEAR
        if (!string.IsNullOrWhiteSpace(selectedYear))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Year?.Trim(),
                    selectedYear,
                    StringComparison.OrdinalIgnoreCase));
        }


        // FILTER BY LOCATION
        if (!string.IsNullOrWhiteSpace(location))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                ContainsText(vehicle.City, location)
                ||
                ContainsText(vehicle.Region, location));
        }


        // FILTER BY FUEL TYPE
        if (!string.IsNullOrWhiteSpace(selectedFuel))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Fuel?.Trim(),
                    selectedFuel,
                    StringComparison.OrdinalIgnoreCase));
        }


        // FILTER BY TRANSMISSION
        if (!string.IsNullOrWhiteSpace(selectedTransmission))
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
                string.Equals(
                    vehicle.Transmission?.Trim(),
                    selectedTransmission,
                    StringComparison.OrdinalIgnoreCase));
        }


        // FILTER BY MINIMUM PRICE
        if (minimumPrice.HasValue)
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
            {
                decimal? price = ParsePrice(vehicle.Price);

                return price.HasValue
                    && price.Value >= minimumPrice.Value;
            });
        }


        // FILTER BY MAXIMUM PRICE
        if (maximumPrice.HasValue)
        {
            filteredVehicles = filteredVehicles.Where(vehicle =>
            {
                decimal? price = ParsePrice(vehicle.Price);

                return price.HasValue
                    && price.Value <= maximumPrice.Value;
            });
        }


        // CONVERT FILTERED RESULTS TO A LIST
        List<Vehicle> results = filteredVehicles.ToList();


        // SORT THE RESULTS
        string selectedSort =
            SortPicker.SelectedItem?.ToString() ?? "Newest First";

        switch (selectedSort)
        {
            case "Price: Low to High":

                results = results
                    .OrderBy(vehicle => !ParsePrice(vehicle.Price).HasValue)
                    .ThenBy(vehicle => ParsePrice(vehicle.Price) ?? 0m)
                    .ToList();

                break;


            case "Price: High to Low":

                results = results
                    .OrderBy(vehicle => !ParsePrice(vehicle.Price).HasValue)
                    .ThenByDescending(vehicle => ParsePrice(vehicle.Price) ?? 0m)
                    .ToList();

                break;


            default:

                // NEWEST MODEL YEAR FIRST
                results = results
                    .OrderByDescending(vehicle =>
                        int.TryParse(vehicle.Year, out int year)
                            ? year
                            : 0)
                    .ToList();

                break;
        }


        // DISPLAY THE FINAL RESULTS
        DisplayVehicles(results);
    }


    // DISPLAY MATCHING VEHICLE CARDS
    private void DisplayVehicles(List<Vehicle> vehicles)
    {
        VehicleResultsLayout.Children.Clear();

        // UPDATE THE RESULTS COUNT
        ResultsCountLabel.Text = vehicles.Count == 1
            ? "1 car"
            : $"{vehicles.Count} cars";

        // SHOW OR HIDE THE EMPTY RESULTS MESSAGE
        EmptyResultsLayout.IsVisible = vehicles.Count == 0;

        if (vehicles.Count == 0)
        {
            EmptyResultsTitle.Text = "No matching vehicles";

            EmptyResultsMessage.Text =
                "We couldn't find vehicles matching your search. " +
                "Try another brand, model, location, or price range.";
        }

        // CREATE A CARD FOR EACH VEHICLE
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


        // VEHICLE PHOTO
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

            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(15, 15, 0, 0)
                },

            Content = vehicleImage
        };


        // VEHICLE NAME
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


        // VEHICLE PRICE
        var priceLabel = new Label
        {
            Text = $"GH₵ {FormatPrice(vehicle.Price)}",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#F7C900")
        };


        // VEHICLE INFORMATION
        var informationLabel = new Label
        {
            Text =
                $"{vehicle.Transmission} • {vehicle.Fuel} • {vehicle.City}",

            FontSize = 14,
            TextColor = Color.FromArgb("#BBBBBB")
        };


        // VIEW DETAILS BUTTON
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


        // VEHICLE INFORMATION LAYOUT
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


        // ASSEMBLE THE CARD
        cardLayout.Children.Add(imageBorder);
        cardLayout.Children.Add(detailsLayout);


        return new Border
        {
            BackgroundColor = Color.FromArgb("#1F1F1F"),
            Stroke = Color.FromArgb("#F7C900"),
            StrokeThickness = 1,

            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = 16
                },

            Content = cardLayout
        };
    }


    // CHECK WHETHER TEXT CONTAINS A SEARCH TERM
    private static bool ContainsText(
        string? value,
        string search)
    {
        return !string.IsNullOrWhiteSpace(value)
            &&
            value.Contains(
                search,
                StringComparison.OrdinalIgnoreCase);
    }


    // PARSE A VEHICLE PRICE
    private static decimal? ParsePrice(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string cleaned = value
            .Replace("GH₵", "", StringComparison.OrdinalIgnoreCase)
            .Replace("GHS", "", StringComparison.OrdinalIgnoreCase)
            .Replace(",", "")
            .Replace(" ", "")
            .Trim();

        if (decimal.TryParse(
            cleaned,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal amount))
        {
            return amount;
        }

        return null;
    }


    // FORMAT VEHICLE PRICE FOR DISPLAY
    private static string FormatPrice(string? price)
    {
        decimal? amount = ParsePrice(price);

        return amount.HasValue
            ? amount.Value.ToString(
                "N0",
                CultureInfo.InvariantCulture)
            : price ?? string.Empty;
    }
}