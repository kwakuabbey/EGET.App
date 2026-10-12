using System.Globalization;

namespace EGET.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        LoadSavedVehicles();
    }

    // REFRESH VEHICLES WHEN THE HOME PAGE APPEARS
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSavedVehicles();
    }

    // OPEN THE SELLER DASHBOARD
    private async void OnMyVehiclesTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyVehiclesPage());
    }

    // BUY A CAR
    private async void OnBuyCarTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BrowsePage));
    }

    // OPEN FAVOURITES
    private async void OnFavouritesTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }

    // SELL A CAR
    private async void OnSellCarTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SellVehiclePage));
    }

    // LOAD SAVED VEHICLES
    private void LoadSavedVehicles()
    {
        // CLEAR EXISTING CARDS BEFORE RELOADING
        SavedVehiclesLayout.Children.Clear();

        // LOAD VEHICLES FROM LOCAL STORAGE
        var vehicles = VehicleStorage.GetVehicles();

        // EXCLUDE ORIGINAL SAMPLE LISTINGS
        // THESE VEHICLES ARE NOT DELETED FROM STORAGE
        var excludedVehicles = new[]
        {
            ("Range Rover", "Velar"),
            ("Toyota", "Land Cruiser"),
            ("Toyota", "Corolla")
        };

        // SHOW ONLY ACTIVE VEHICLES
        // KEEP THE ORIGINAL SAMPLE LISTINGS EXCLUDED
        var homePageVehicles = vehicles
            .Where(vehicle =>
                (
                    string.IsNullOrWhiteSpace(vehicle.Status)
                        ? "Active"
                        : vehicle.Status.Trim()
                ).Equals(
                    "Active",
                    StringComparison.OrdinalIgnoreCase
                )
                &&
                !excludedVehicles.Any(excluded =>
                    string.Equals(
                        vehicle.Brand?.Trim(),
                        excluded.Item1,
                        StringComparison.OrdinalIgnoreCase
                    )
                    &&
                    string.Equals(
                        vehicle.Model?.Trim(),
                        excluded.Item2,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
            )
            .ToList();

        // DISPLAY ACTIVE VEHICLES
        foreach (var vehicle in homePageVehicles)
        {
            // VEHICLE CARD
            var vehicleCard = new Border
            {
                BackgroundColor = Color.FromArgb("#1F1F1F"),
                StrokeThickness = 0,
                StrokeShape =
                    new Microsoft.Maui.Controls.Shapes.RoundRectangle
                    {
                        CornerRadius = new CornerRadius(15)
                    }
            };

            var layout = new VerticalStackLayout
            {
                Spacing = 0
            };

            // VEHICLE PHOTO
            if (vehicle.PhotoPaths != null &&
                vehicle.PhotoPaths.Count > 0 &&
                !string.IsNullOrWhiteSpace(vehicle.PhotoPaths[0]))
            {
                var image = new Image
                {
                    Source = ImageSource.FromFile(
                        vehicle.PhotoPaths[0]),
                    HeightRequest = 190,
                    Aspect = Aspect.AspectFill
                };

                layout.Children.Add(image);
            }
            else
            {
                // PLACEHOLDER WHEN A VEHICLE HAS NO PHOTO
                var placeholder = new Grid
                {
                    HeightRequest = 190,
                    BackgroundColor = Color.FromArgb("#292929")
                };

                placeholder.Children.Add(new Label
                {
                    Text = "VEHICLE PHOTO",
                    TextColor = Colors.Gray,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                });

                layout.Children.Add(placeholder);
            }

            // VEHICLE INFORMATION
            var information = new VerticalStackLayout
            {
                Padding = 15,
                Spacing = 8
            };

            // VEHICLE NAME
            var nameLabel = new Label
            {
                Text =
                    $"{vehicle.Year} " +
                    $"{FormatVehicleName(vehicle.Brand)} " +
                    $"{FormatVehicleName(vehicle.Model)}",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center
            };

            // VEHICLE STATUS
            string vehicleStatus =
                string.IsNullOrWhiteSpace(vehicle.Status)
                    ? "Active"
                    : vehicle.Status.Trim();

            Color statusBackground;
            Color statusBorder;
            Color statusText;

            switch (vehicleStatus.ToLowerInvariant())
            {
                case "sold":
                    statusBackground = Color.FromArgb("#17304A");
                    statusBorder = Color.FromArgb("#4A90E2");
                    statusText = Color.FromArgb("#8CC4FF");
                    break;

                case "draft":
                    statusBackground = Color.FromArgb("#49351A");
                    statusBorder = Color.FromArgb("#D99A35");
                    statusText = Color.FromArgb("#FFD080");
                    break;

                default:
                    vehicleStatus = "Active";
                    statusBackground = Color.FromArgb("#173D28");
                    statusBorder = Color.FromArgb("#2E8B57");
                    statusText = Color.FromArgb("#7CFC98");
                    break;
            }

            // STATUS BADGE
            var statusBadge = new Border
            {
                BackgroundColor = statusBackground,
                Stroke = statusBorder,
                StrokeThickness = 1,
                StrokeShape =
                    new Microsoft.Maui.Controls.Shapes.RoundRectangle
                    {
                        CornerRadius = new CornerRadius(8)
                    },
                Padding = new Thickness(9, 5),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Content = new Label
                {
                    Text = vehicleStatus.ToUpperInvariant(),
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = statusText
                }
            };

            // VEHICLE NAME AND STATUS ROW
            var titleRow = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                ColumnSpacing = 10
            };

            titleRow.Add(nameLabel, 0, 0);
            titleRow.Add(statusBadge, 1, 0);

            // VEHICLE PRICE
            var priceLabel = new Label
            {
                Text = FormatVehiclePrice(vehicle.Price),
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#F6C800")
            };

            // VEHICLE DETAILS
            var detailsLabel = new Label
            {
                Text =
                    $"{vehicle.Mileage} km • " +
                    $"{vehicle.Transmission} • " +
                    $"{vehicle.Fuel}",
                FontSize = 13,
                TextColor = Color.FromArgb("#BBBBBB")
            };

            // VEHICLE LOCATION
            string location = string.Join(
                ", ",
                new[] { vehicle.City, vehicle.Region }
                    .Where(value =>
                        !string.IsNullOrWhiteSpace(value))
                    .Select(value => value.Trim())
            );

            var locationLabel = new Label
            {
                Text = $"📍 {location}",
                FontSize = 13,
                TextColor = Color.FromArgb("#BBBBBB")
            };

            // VIEW DETAILS BUTTON
            var viewDetailsButton = new Button
            {
                Text = "View Details",
                BackgroundColor = Color.FromArgb("#F6C800"),
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 10,
                HeightRequest = 45,
                Margin = new Thickness(0, 8, 0, 0)
            };

            // OPEN THE SELECTED VEHICLE DETAILS
            viewDetailsButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(
                    new VehicleDetailsPage(vehicle));
            };

            // ADD INFORMATION TO THE CARD
            information.Children.Add(titleRow);
            information.Children.Add(priceLabel);
            information.Children.Add(detailsLabel);
            information.Children.Add(locationLabel);
            information.Children.Add(viewDetailsButton);

            // BUILD THE VEHICLE CARD
            layout.Children.Add(information);
            vehicleCard.Content = layout;

            // ADD THE CARD TO THE HOME PAGE
            SavedVehiclesLayout.Children.Add(vehicleCard);
        }
    }

    // FORMAT VEHICLE PRICE
    private static string FormatVehiclePrice(string? price)
    {
        if (string.IsNullOrWhiteSpace(price))
        {
            return "Price not specified";
        }

        string cleanedPrice = price
            .Replace("GH₵", "", StringComparison.OrdinalIgnoreCase)
            .Replace("GHS", "", StringComparison.OrdinalIgnoreCase)
            .Replace(",", "")
            .Trim();

        if (decimal.TryParse(
            cleanedPrice,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal amount))
        {
            return $"GH₵ {amount.ToString("#,##0.##", CultureInfo.InvariantCulture)}";
        }

        return $"GH₵ {price.Trim()}";
    }

    // FORMAT VEHICLE NAME
    private static string FormatVehicleName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        return string.Join(
            " ",
            name
                .Trim()
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(word =>
                    char.ToUpperInvariant(word[0]) +
                    word.Substring(1).ToLowerInvariant())
        );
    }
}