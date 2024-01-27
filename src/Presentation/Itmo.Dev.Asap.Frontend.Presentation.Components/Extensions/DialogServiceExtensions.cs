using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Itmo.Dev.Asap.Frontend.Presentation.Components;

public static class DialogServiceExtensions
{
    public static IDialogReference ShowTemplated(
        this IDialogService service,
        DialogOptions options,
        RenderFragment fragment,
        EventCallback onOpened = default,
        EventCallback onClosed = default,
        string? contentStyle = null,
        string? style = null)
    {
        var parameters = new DialogParameters<DynamicDialog>
        {
            { x => x.Fragment, fragment },
            { x => x.OnOpened, onOpened },
            { x => x.OnClosed, onClosed },
            { x => x.ContentStyle, contentStyle },
            { x => x.Style, style },
        };

        return service.Show<DynamicDialog>(string.Empty, parameters, options);
    }

    public static async Task ShowTemplatedWithClosingAsync(
        this IDialogService service,
        DialogOptions options,
        Task closing,
        RenderFragment fragment,
        string? contentStyle = null)
    {
        var parameters = new DialogParameters<DynamicDialog>
        {
            { x => x.Fragment, fragment },
            { x => x.ContentStyle, contentStyle },
        };

        IDialogReference reference = await service.ShowAsync<DynamicDialog>(string.Empty, parameters, options);

        Task<object> resultTask = reference.GetReturnValueAsync<object>();

        await Task.WhenAny(closing, resultTask);

        if (closing.IsCompleted)
            reference.Close();
    }
}