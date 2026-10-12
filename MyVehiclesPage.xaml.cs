namespace EGET.App;

public partial class MyVehiclesPage : ContentPage
{
    public MyVehiclesPage()
    {
        InitializeComponent();
        LoadMyVehicles();
    }

    // REFRESH THE DASHBOARD WHEN THE PAGE APPEARS
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMyVehicles();
    }

    // LOAD THE SELLER'S SAVED VEHICLES
    private void LoadMyVehicles()
    {
        var vehicles = VehicleStorage.GetVehicles();

        // UPDATE THE DASHBOARD SUMMARY
        int totalListings = vehicles.Count;

        TotalListingsLabel.Text = totalListings == 1
            ? "1 vehicle"
            : $"{totalListings} vehicles";

        // REFRESH THE VEHICLE LIST
        MyVehiclesCollection.ItemsSource = null;
        MyVehiclesCollection.ItemsSource = vehicles;
    }

    // OPEN THE SELL VEHICLE PAGE
    private async void OnAddVehicleClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(new SellVehiclePage());
    }

    // VIEW VEHICLE DETAILS
    private async void OnViewDetailsClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Vehicle vehicle)
        {
            return;
        }

        await Navigation.PushAsync(
            new VehicleDetailsPage(vehicle));
    }

    // EDIT A VEHICLE LISTING
    private async void OnEditVehicleClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Vehicle vehicle)
        {
            return;
        }

        await Navigation.PushAsync(
            new EditVehiclePage(vehicle));
    }

    // DELETE A VEHICLE LISTING
    private async void OnDeleteVehicleClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Vehicle vehicle)
        {
            return;
        }

        bool confirm = await DisplayAlert(
            "Delete Listing",
            $"Are you sure you want to delete your {vehicle.Brand} {vehicle.Model} listing? This action cannot be undone.",
            "Delete",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        bool deleted = VehicleStorage.DeleteVehicle(
            vehicle.Id);

        if (deleted)
        {
            LoadMyVehicles();

            await DisplayAlert(
                "Listing Deleted",
                "Your vehicle listing has been deleted successfully.",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Delete Failed",
                "The vehicle listing could not be deleted. Please try again.",
                "OK");
        }
    }

    // CHANGE A VEHICLE LISTING'S STATUS
    private async void OnChangeStatusClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Vehicle vehicle)
        {
            return;
        }

        string? selectedStatus = await DisplayActionSheet(
            $"Status for {vehicle.Brand} {vehicle.Model}",
            "Cancel",
            null,
            "Active",
            "Draft",
            "Sold");

        // STOP IF THE SELLER CANCELS
        if (string.IsNullOrWhiteSpace(selectedStatus) ||
            selectedStatus == "Cancel")
        {
            return;
        }

        // SAVE THE SELECTED STATUS
        vehicle.Status = selectedStatus;

        bool updated = VehicleStorage.UpdateVehicle(vehicle);

        if (updated)
        {
            LoadMyVehicles();

            await DisplayAlert(
                "Status Updated",
                $"{vehicle.Brand} {vehicle.Model} is now marked as {selectedStatus}.",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Update Failed",
                "The vehicle status could not be saved. Please try again.",
                "OK");
        }
    }
}