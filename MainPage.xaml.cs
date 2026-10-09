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

    private async void OnBuyCarTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BrowsePage));
    }

    private async void OnFavouritesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }

    private async void OnSellCarTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SellVehiclePage));
    }

    // LOAD SAVED VEHICLES
    private void LoadSavedVehicles()
    {
        // Clear existing cards before reloading.
        SavedVehiclesLayout.Children.Clear();

        var vehicles = VehicleStorage.GetVehicles();

        foreach (var vehicle in vehicles)
        {
            var vehicleCard = new Border
            {
                BackgroundColor = Color.FromArgb("#1F1F1F"),
                StrokeThickness = 0,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
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
                vehicle.PhotoPaths.Count > 0)
            {
                var image = new Image
                {
                    Source = ImageSource.FromFile(vehicle.PhotoPaths[0]),
                    HeightRequest = 190,
                    Aspect = Aspect.AspectFill
                };

                layout.Children.Add(image);
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
                Text = $"{vehicle.Year} {FormatVehicleName(vehicle.Brand)} {FormatVehicleName(vehicle.Model)}",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            };

            // VEHICLE PRICE
            var priceLabel = new Label
            {
                Text = $"GH₵ {FormatPrice(vehicle.Price)}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#F6C800")
            };

            // VEHICLE DETAILS
            var detailsLabel = new Label
            {
                Text = $"{vehicle.Mileage} km • {vehicle.Transmission} • {vehicle.Fuel}",
                FontSize = 13,
                TextColor = Color.FromArgb("#BBBBBB")
            };

            // VEHICLE LOCATION
            var locationLabel = new Label
            {
                Text = $"📍 {vehicle.City}, {vehicle.Region}",
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

            // OPEN VEHICLE DETAILS
            viewDetailsButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(
                    new VehicleDetailsPage(vehicle));
            };

            // ADD INFORMATION TO CARD
            information.Children.Add(nameLabel);
            information.Children.Add(priceLabel);
            information.Children.Add(detailsLabel);
            information.Children.Add(locationLabel);
            information.Children.Add(viewDetailsButton);

            // ADD INFORMATION TO LAYOUT
            layout.Children.Add(information);

            // ADD LAYOUT TO CARD
            vehicleCard.Content = layout;

            // ADD CARD TO HOME PAGE
            SavedVehiclesLayout.Children.Add(vehicleCard);
        }
    }

    // FORMAT VEHICLE PRICE
    private string FormatPrice(string price)
    {
        if (decimal.TryParse(
            price.Replace(",", ""),
            out decimal amount))
        {
            return amount.ToString("N0");
        }

        return price;
    }

    // FORMAT VEHICLE NAME
    private string FormatVehicleName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        return string.Join(
            " ",
            name
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(word =>
                    char.ToUpper(word[0]) +
                    word.Substring(1).ToLower()));
    }
}