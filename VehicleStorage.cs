using System.Text.Json;

namespace EGET.App;

public static class VehicleStorage
{
    private const string VehiclesKey = "eget_vehicles";

    // GET ALL SAVED VEHICLES
    public static List<Vehicle> GetVehicles()
    {
        var vehicles = ReadVehicles();
        bool updated = false;

        foreach (var vehicle in vehicles)
        {
            if (string.IsNullOrWhiteSpace(vehicle.Id))
            {
                vehicle.Id = Guid.NewGuid().ToString();
                updated = true;
            }
        }

        if (updated)
        {
            SaveVehicles(vehicles);
        }

        return vehicles;
    }

    // SAVE A NEW VEHICLE
    public static void AddVehicle(Vehicle vehicle)
    {
        var vehicles = GetVehicles();

        if (string.IsNullOrWhiteSpace(vehicle.Id))
        {
            vehicle.Id = Guid.NewGuid().ToString();
        }

        // Prevent duplicate entries with the same ID.
        int existingIndex = vehicles.FindIndex(
            item => item.Id == vehicle.Id);

        if (existingIndex >= 0)
        {
            vehicles[existingIndex] = vehicle;
        }
        else
        {
            vehicles.Add(vehicle);
        }

        SaveVehicles(vehicles);
    }

    // UPDATE AN EXISTING VEHICLE
    public static bool UpdateVehicle(Vehicle updatedVehicle)
    {
        if (updatedVehicle == null ||
            string.IsNullOrWhiteSpace(updatedVehicle.Id))
        {
            return false;
        }

        var vehicles = GetVehicles();

        // Find the existing listing using its unique ID.
        int index = vehicles.FindIndex(
            vehicle => vehicle.Id == updatedVehicle.Id);

        if (index < 0)
        {
            return false;
        }

        // Replace the existing listing.
        vehicles[index] = updatedVehicle;

        SaveVehicles(vehicles);

        return true;
    }

    // DELETE ONE VEHICLE
    public static bool DeleteVehicle(string vehicleId)
    {
        if (string.IsNullOrWhiteSpace(vehicleId))
        {
            return false;
        }

        var vehicles = GetVehicles();

        int index = vehicles.FindIndex(
            vehicle => vehicle.Id == vehicleId);

        if (index < 0)
        {
            return false;
        }

        vehicles.RemoveAt(index);

        SaveVehicles(vehicles);

        return true;
    }

    // REMOVE ALL VEHICLES
    public static void ClearVehicles()
    {
        Preferences.Default.Remove(VehiclesKey);
    }

    // READ SAVED VEHICLES
    private static List<Vehicle> ReadVehicles()
    {
        string json = Preferences.Default.Get(
            VehiclesKey,
            "");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Vehicle>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Vehicle>>(json)
                   ?? new List<Vehicle>();
        }
        catch (JsonException)
        {
            return new List<Vehicle>();
        }
    }

    // SAVE VEHICLES TO LOCAL STORAGE
    private static void SaveVehicles(List<Vehicle> vehicles)
    {
        string json = JsonSerializer.Serialize(vehicles);

        Preferences.Default.Set(VehiclesKey, json);
    }
}