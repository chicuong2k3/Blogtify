using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blogtify.Client.Theming;

public class ThemeProvider : IDisposable, IThemeProvider
{
    private readonly IHttpContextProxy _httpContextProxy;
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IJSRuntime _jsRuntime;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;
    private Theme? _theme;
    private bool _cookiesSet = false;

    public ThemeProvider(
        IHttpContextProxy httpContextProxy,
        PersistentComponentState persistentComponentState,
        IJSRuntime jsRuntime)
    {
        _httpContextProxy = httpContextProxy;
        _persistentComponentState = persistentComponentState;
        _jsRuntime = jsRuntime;
        _persistingComponentStateSubscription = _persistentComponentState.RegisterOnPersisting(PersistTheme, RenderMode.InteractiveAuto);
    }

    public event ThemeChangedHandler? ThemeChanged;

    public async Task SetThemeAsync(Theme theme)
    {
        _theme = theme;
        ThemeChanged?.Invoke(theme);

        if (!_cookiesSet && _httpContextProxy.IsSupported())
        {
            try
            {
                await _httpContextProxy.SetValueAsync("Theme", theme.ToString());
                _cookiesSet = true;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Headers are read-only"))
            {
                _cookiesSet = true; 
            }
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme.ToString());  // Lưu client-side
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setTheme", theme.ToString());  // Cập nhật CSS
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering") || ex.Message.Contains("Headers"))
        {
            // Bỏ qua lỗi JS/prerender
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
        if (_httpContextProxy.IsSupported() && await _httpContextProxy.GetValueAsync("Theme") is string cookie)
        {
            themeStr = cookie;
        }
        else
        {
            try
            {
                themeStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            }
            catch { /* JS chưa sẵn sàng */ }

            if (string.IsNullOrEmpty(themeStr) && _persistentComponentState.TryTakeFromJson<Theme>("Theme", out var restored))
            {
                themeStr = restored.ToString();
            }
        }

        if (Enum.TryParse<Theme>(themeStr, out var theme))
        {
            _theme = theme;
        }
        else
        {
            _theme = Theme.Yeti; 
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setTheme", _theme.Value.ToString());
        }
        catch (InvalidOperationException) { /* Bỏ qua prerender */ }
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
