using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blogtify.Client.Theming;

public class ThemeProvider : IDisposable, IThemeProvider
{
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IJSRuntime _jsRuntime;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;
    private Theme? _theme;

    public ThemeProvider(
        PersistentComponentState persistentComponentState,
        IJSRuntime jsRuntime)
    {
        _persistentComponentState = persistentComponentState;
        _jsRuntime = jsRuntime;
        _persistingComponentStateSubscription =
            _persistentComponentState.RegisterOnPersisting(PersistTheme, RenderMode.InteractiveAuto);
    }

    public event ThemeChangedHandler? ThemeChanged;

    public async Task SetThemeAsync(Theme theme)
    {
        _theme = theme;
        ThemeChanged?.Invoke(theme);

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme.ToString());
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setTheme", theme.ToString());
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
        {
            // Ignore errors during prerender
        }
    }

    public async Task<Theme> GetThemeAsync()
    {
        if (_theme is null)
        {
            await ResolveInitialTheme();
        }

        return _theme!.Value;
    }

    private async Task ResolveInitialTheme()
    {
        string? themeStr = null;

        try
        {
            themeStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
        }
        catch
        {
            // JS not ready during prerender
        }

        if (string.IsNullOrEmpty(themeStr) &&
            _persistentComponentState.TryTakeFromJson<Theme>("Theme", out var restored))
        {
            themeStr = restored.ToString();
        }

        if (Enum.TryParse<Theme>(themeStr, out var theme))
        {
            _theme = theme;
        }
        else
        {
            _theme = Theme.Yeti; // default
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setTheme", _theme.Value.ToString());
        }
        catch (InvalidOperationException)
        {
            // Ignore prerender
        }
    }

    private async Task PersistTheme()
    {
        _persistentComponentState.PersistAsJson("Theme", await GetThemeAsync());
    }

    public void Dispose()
    {
        _persistingComponentStateSubscription.Dispose();
    }
}

public delegate void ThemeChangedHandler(Theme theme);
