namespace EGET.App;

public static class FavouriteStorage
{
    private const string CorollaKey = "favourite_corolla";

    public static bool IsCorollaFavourite()
    {
        return Preferences.Default.Get(CorollaKey, false);
    }

    public static void SaveCorolla()
    {
        Preferences.Default.Set(CorollaKey, true);
    }

    public static void RemoveCorolla()
    {
        Preferences.Default.Remove(CorollaKey);
    }
}