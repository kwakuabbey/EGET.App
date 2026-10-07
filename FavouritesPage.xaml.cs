namespace EGET.App;

public partial class FavouritesPage : ContentPage
{
    public FavouritesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadFavouriteVehicles();
    }

    // LOAD ALL FAVOURITE VEHICLES
    private void LoadFavouriteVehicles()
    {
        FavouriteVehiclesLayout.Children.Clear();

        // MOVE THE OLD COROLLA FAVOURITE
        // INTO THE NEW FAVOURITES SYSTEM
        AddLegacyCorollaIfNeeded();

        var favourites = FavouriteVehicleStorage.GetFavourites();

        if (favourites.Count == 0)
        {
            EmptyFavouritesView.IsVisible = true;
            FavouriteVehiclesLayout.IsVisible = false;
            return;
        }

        EmptyFavouritesView.IsVisible = false;
        FavouriteVehiclesLayout.IsVisible = true;

        foreach (var vehicle in favourites)
        {
            AddFavouriteVehicleCard(vehicle);
        }
    }

    // KEEP THE EXISTING COROLLA FAVOURITE
    private void AddLegacyCorollaIfNeeded()
    {
        if (!FavouriteStorage.IsCorollaFavourite())
            return;

        var favourites = FavouriteVehicleStorage.GetFavourites();

        bool corollaAlreadyExists = favourites.Any(v =>
            v.Brand.Equals("Toyota", StringComparison.OrdinalIgnoreCase) &&
            v.Model.Equals("Corolla", StringComparison.OrdinalIgnoreCase) &&
            v.Year == "2020");

        if (corollaAlreadyExists)
            return;

        var corolla = new Vehicle
        {
            Brand = "Toyota",
            Model = "Corolla",
            Year = "2020",
            Price = "185000",
            Mileage = "65000",
            Fuel = "Petrol",
            Transmission = "Automatic",
            City = "Accra",
            Region = "Greater Accra",
            Description =
                "Well maintained Toyota Corolla available for sale. Contact the seller for more information and viewing arrangements.",
            PhotoPaths = new List<string>
            {
                "corolla1.png"
            }
        };

        FavouriteVehicleStorage.AddFavourite(corolla);
    }

    // CREATE A FAVOURITE VEHICLE CARD
    private void AddFavouriteVehicleCard(Vehicle vehicle)
    {
        var vehicleCard = new Border
        {
            BackgroundColor = Color.FromArgb("#1F1F1F"),
            Stroke = Color.FromArgb("#F6C800"),
            StrokeThickness = 1,
            StrokeShape =
                new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(16)
                }
        };

        var layout = new VerticalStackLayout
        {
            Spacing = 0
        };

        // VEHICLE PHOTO
        if (vehicle.PhotoPaths.Count > 0)
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
            Spacing = 7
        };

        // VEHICLE NAME
        var nameLabel = new Label
        {
            Text =
                $"{vehicle.Year} {FormatVehicleName(vehicle.Brand)} {FormatVehicleName(vehicle.Model)}",
            FontSize = 20,
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
            Text =
                $"{vehicle.Mileage} km • {vehicle.Transmission} • {vehicle.Fuel}",
            FontSize = 14,
            TextColor = Color.FromArgb("#BBBBBB")
        };

        // VEHICLE LOCATION
        var locationLabel = new Label
        {
            Text = $"📍 {FormatVehicleName(vehicle.City)}, {FormatVehicleName(vehicle.Region)}",
            FontSize = 14,
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

        viewDetailsButton.Clicked += async (sender, e) =>
        {
            await Navigation.PushAsync(
                new VehicleDetailsPage(vehicle));
        };

        // REMOVE FAVOURITE BUTTON
        var removeButton = new Button
        {
            Text = "Remove from Favourites",
            BackgroundColor = Color.FromArgb("#222222"),
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 45,
            Margin = new Thickness(0, 5, 0, 0)
        };

        removeButton.Clicked += async (sender, e) =>
        {
            bool confirm = await DisplayAlert(
                "Remove Favourite",
                $"Remove {FormatVehicleName(vehicle.Brand)} {FormatVehicleName(vehicle.Model)} from your favourites?",
                "Remove",
                "Cancel");

            if (!confirm)
                return;

            FavouriteVehicleStorage.RemoveFavourite(vehicle);

            // Also remove the old Corolla favourite flag if applicable
            if (vehicle.Brand.Equals("Toyota", StringComparison.OrdinalIgnoreCase) &&
                vehicle.Model.Equals("Corolla", StringComparison.OrdinalIgnoreCase) &&
                vehicle.Year == "2020")
            {
                FavouriteStorage.RemoveCorolla();
            }

            LoadFavouriteVehicles();
        };

        information.Children.Add(nameLabel);
        information.Children.Add(priceLabel);
        information.Children.Add(detailsLabel);
        information.Children.Add(locationLabel);
        information.Children.Add(viewDetailsButton);
        information.Children.Add(removeButton);

        layout.Children.Add(information);

        vehicleCard.Content = layout;

        FavouriteVehiclesLayout.Children.Add(vehicleCard);
    }

    // FORMAT PRICE
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