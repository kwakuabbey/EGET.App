namespace EGET.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

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


    private void LoadSavedVehicles()
    {
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

            var nameLabel = new Label
            {
                Text = $"{vehicle.Year} {vehicle.Brand} {vehicle.Model}",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            };

            var priceLabel = new Label
            {
                Text = $"GH₵ {vehicle.Price}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#F6C800")
            };

            var detailsLabel = new Label
            {
                Text = $"{vehicle.Mileage} km • {vehicle.Transmission} • {vehicle.Fuel}",
                FontSize = 13,
                TextColor = Color.FromArgb("#BBBBBB")
            };

            var locationLabel = new Label
            {
                Text = $"📍 {vehicle.City}, {vehicle.Region}",
                FontSize = 13,
                TextColor = Color.FromArgb("#BBBBBB")
            };

            information.Children.Add(nameLabel);
            information.Children.Add(priceLabel);
            information.Children.Add(detailsLabel);
            information.Children.Add(locationLabel);

            layout.Children.Add(information);

            vehicleCard.Content = layout;

            SavedVehiclesLayout.Children.Add(vehicleCard);
        }
    }
}