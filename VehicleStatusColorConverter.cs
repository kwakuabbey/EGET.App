using System.Globalization;

namespace EGET.App;

public class VehicleStatusColorConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        string status = value?.ToString()?.Trim() ?? "Active";

        string colorType = parameter?.ToString()?.ToLowerInvariant()
                           ?? "text";

        status = status.ToLowerInvariant();

        return colorType switch
        {
            "background" => status switch
            {
                "sold" => Color.FromArgb("#17304A"),
                "draft" => Color.FromArgb("#49351A"),
                _ => Color.FromArgb("#173D28")
            },

            "border" => status switch
            {
                "sold" => Color.FromArgb("#4A90E2"),
                "draft" => Color.FromArgb("#D99A35"),
                _ => Color.FromArgb("#2E8B57")
            },

            _ => status switch
            {
                "sold" => Color.FromArgb("#8CC4FF"),
                "draft" => Color.FromArgb("#FFD080"),
                _ => Color.FromArgb("#7CFC98")
            }
        };
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}