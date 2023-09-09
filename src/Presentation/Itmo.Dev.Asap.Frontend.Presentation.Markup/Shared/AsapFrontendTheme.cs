using MudBlazor;
using MudBlazor.Utilities;

namespace Itmo.Dev.Asap.Frontend.Presentation.Markup.Shared;

public static class AsapFrontendTheme
{
    public static readonly MudTheme Instance = new MudTheme
    {
        Palette = new PaletteLight
        {
            Primary = new MudColor("#000000"),
            Secondary = new MudColor("#4daaee"),
            Background = new MudColor("#ffffff"),
            DrawerBackground = new MudColor("#4daaee"),
            DrawerText = new MudColor("#ffffff"),
            DrawerIcon = new MudColor("#ffffff"),
        },
    };
}