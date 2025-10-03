using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blogtify.Client.Theming;

public class FontProvider : IDisposable, IFontProvider
{
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IJSRuntime _jsRuntime;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;

    private string? _font;
    private string? _fontSize;

    public FontProvider(
        PersistentComponentState persistentComponentState,
        IJSRuntime jsRuntime)
    {
        _persistentComponentState = persistentComponentState;
        _jsRuntime = jsRuntime;

        _persistingComponentStateSubscription =
            _persistentComponentState.RegisterOnPersisting(PersistFontAndSize, RenderMode.InteractiveAuto);
    }

    public event FontChangedHandler? FontChanged;
    public event FontSizeChangedHandler? FontSizeChanged;

    public async Task SetFontAsync(string font)
    {
        _font = font;
        FontChanged?.Invoke(font);

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "font", font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.loadGoogleFont", font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFont", font);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
        {
            // Ignore errors during prerender
        }
    }

    public async Task<string> GetFontAsync()
    {
        if (string.IsNullOrEmpty(_font))
        {
            await ResolveInitialFont();
        }
        return _font!;
    }

    private async Task ResolveInitialFont()
    {
        string? fontStr = null;

        try
        {
            fontStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "font");
        }
        catch
        {
            // JS not ready during prerender
        }

        if (string.IsNullOrEmpty(fontStr) &&
            _persistentComponentState.TryTakeFromJson<string>("Font", out var restored))
        {
            fontStr = restored;
        }

        _font = !string.IsNullOrEmpty(fontStr) ? fontStr : "Inter";

        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.loadGoogleFont", _font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFont", _font);
        }
        catch (InvalidOperationException)
        {
            // Ignore prerender
        }
    }

    public async Task SetFontSizeAsync(string size)
    {
        _fontSize = size;
        FontSizeChanged?.Invoke(size);

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "fontSize", size);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFontSize", size);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
        {
            // Ignore prerender
        }
    }

    public async Task<string?> GetFontSizeAsync()
    {
        if (string.IsNullOrEmpty(_fontSize))
        {
            await ResolveInitialFontSize();
        }

        return _fontSize!;
    }

    private async Task ResolveInitialFontSize()
    {
        string? sizeStr = null;

        try
        {
            sizeStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "fontSize");
        }
        catch
        {
            // JS not ready during prerender
        }

        if (string.IsNullOrEmpty(sizeStr) &&
            _persistentComponentState.TryTakeFromJson<string>("FontSize", out var restored))
        {
            sizeStr = restored;
        }

        _fontSize = !string.IsNullOrEmpty(sizeStr) ? sizeStr : "16px";

        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFontSize", _fontSize);
        }
        catch (InvalidOperationException)
        {
            // Ignore prerender
        }
    }

    private async Task PersistFontAndSize()
    {
        _persistentComponentState.PersistAsJson("Font", await GetFontAsync());
        _persistentComponentState.PersistAsJson("FontSize", await GetFontSizeAsync());
    }

    public void Dispose()
    {
        _persistingComponentStateSubscription.Dispose();
    }
}

public delegate void FontChangedHandler(string font);
public delegate void FontSizeChangedHandler(string size);
