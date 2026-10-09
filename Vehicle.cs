namespace EGET.App;

public class Vehicle
{
    // UNIQUE ID FOR EACH VEHICLE LISTING
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Year { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty;

    public string Mileage { get; set; } = string.Empty;

    public string Fuel { get; set; } = string.Empty;

    public string Transmission { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> PhotoPaths { get; set; } = new();
}