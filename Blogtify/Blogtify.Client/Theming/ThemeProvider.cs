using Microsoft.AspNetCore.Components;

namespace Blogtify.Client.Theming;

public class ThemeProvider : IDisposable, IThemeProvider
{
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IHttpContextProxy _httpContextProxy;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;
    private Theme? _theme;

    public ThemeProvider(
        PersistentComponentState persistentComponentState,
        IHttpContextProxy httpContextProxy)
    {
        _persistentComponentState = persistentComponentState;
        _httpContextProxy = httpContextProxy;
        _persistingComponentStateSubscription =
            _persistentComponentState.RegisterOnPersisting(PersistTheme);
    }

    public event ThemeChangedHandler? ThemeChanged;

    public async Task SetThemeAsync(Theme theme)
    {
        _theme = theme;
        ThemeChanged?.Invoke(theme);

        await _httpContextProxy.SetValueAsync("theme", theme.ToString());

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

        themeStr = await _httpContextProxy.GetValueAsync("theme");

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
