using System.Text.Json;

namespace EGET.App;

public static class VehicleStorage
{
    private const string VehiclesKey = "eget_vehicles";

    // GET ALL SAVED VEHICLES
    public static List<Vehicle> GetVehicles()
    {
        string json = Preferences.Default.Get(VehiclesKey, "");

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

    // SAVE A NEW VEHICLE
    public static void AddVehicle(Vehicle vehicle)
    {
        var vehicles = GetVehicles();

        vehicles.Add(vehicle);

        string json = JsonSerializer.Serialize(vehicles);

        Preferences.Default.Set(VehiclesKey, json);
    }

    // REMOVE ALL VEHICLES
    public static void ClearVehicles()
    {
        Preferences.Default.Remove(VehiclesKey);
    }
}