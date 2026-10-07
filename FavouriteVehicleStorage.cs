using System.Text.Json;

namespace EGET.App;

public static class FavouriteVehicleStorage
{
    private const string FavouritesKey = "eget_favourite_vehicles";

    // GET ALL FAVOURITE VEHICLES
    public static List<Vehicle> GetFavourites()
    {
        string json = Preferences.Default.Get(FavouritesKey, "");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Vehicle>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Vehicle>>(json)
                   ?? new List<Vehicle>();
        }
        catch
        {
            return new List<Vehicle>();
        }
    }

    // ADD VEHICLE TO FAVOURITES
    public static void AddFavourite(Vehicle vehicle)
    {
        var favourites = GetFavourites();

        // Prevent the same vehicle from being added twice
        bool alreadySaved = favourites.Any(v =>
            v.Brand.Equals(vehicle.Brand, StringComparison.OrdinalIgnoreCase) &&
            v.Model.Equals(vehicle.Model, StringComparison.OrdinalIgnoreCase) &&
            v.Year == vehicle.Year &&
            v.Price == vehicle.Price);

        if (alreadySaved)
        {
            return;
        }

        favourites.Add(vehicle);

        string json = JsonSerializer.Serialize(favourites);

        Preferences.Default.Set(FavouritesKey, json);
    }

    // REMOVE VEHICLE FROM FAVOURITES
    public static void RemoveFavourite(Vehicle vehicle)
    {
        var favourites = GetFavourites();

        favourites.RemoveAll(v =>
            v.Brand.Equals(vehicle.Brand, StringComparison.OrdinalIgnoreCase) &&
            v.Model.Equals(vehicle.Model, StringComparison.OrdinalIgnoreCase) &&
            v.Year == vehicle.Year &&
            v.Price == vehicle.Price);

        string json = JsonSerializer.Serialize(favourites);

        Preferences.Default.Set(FavouritesKey, json);
    }

    // CHECK IF VEHICLE IS A FAVOURITE
    public static bool IsFavourite(Vehicle vehicle)
    {
        var favourites = GetFavourites();

        return favourites.Any(v =>
            v.Brand.Equals(vehicle.Brand, StringComparison.OrdinalIgnoreCase) &&
            v.Model.Equals(vehicle.Model, StringComparison.OrdinalIgnoreCase) &&
            v.Year == vehicle.Year &&
            v.Price == vehicle.Price);
    }
}