using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blogtify.Client.Theming;

public class FontProvider : IDisposable, IFontProvider
{
    private readonly IHttpContextProxy _httpContextProxy;
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IJSRuntime _jsRuntime;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;

    private string? _font;
    private string? _fontSize;

    public FontProvider(
        IHttpContextProxy httpContextProxy,
        PersistentComponentState persistentComponentState,
        IJSRuntime jsRuntime)
    {
        _httpContextProxy = httpContextProxy;
        _persistentComponentState = persistentComponentState;
        _jsRuntime = jsRuntime;

        _persistingComponentStateSubscription =
            _persistentComponentState.RegisterOnPersisting(PersistFontAndSize);
    }

    public event FontChangedHandler? FontChanged;
    public event FontSizeChangedHandler? FontSizeChanged;

    public async Task SetFontAsync(string font)
    {
        _font = font;
        FontChanged?.Invoke(font);

        await _httpContextProxy.SetValueAsync("Font", font);
        await _jsRuntime.InvokeVoidAsync("themeSwitcher.loadGoogleFont", font);
        await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFont", font);
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
        if (_httpContextProxy.IsSupported()
            && await _httpContextProxy.GetValueAsync("Font") is string cookie)
        {
            _font = cookie;
        }
        else if (_persistentComponentState.TryTakeFromJson<string>("Font", out var restored))
        {
            _font = restored;
        }
        else
        {
            _font = "Inter";
        }
    }

    public async Task SetFontSizeAsync(string size)
    {
        _fontSize = size;
        FontSizeChanged?.Invoke(size);

        await _httpContextProxy.SetValueAsync("FontSize", size);
        await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFontSize", size);
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
        if (_httpContextProxy.IsSupported()
            && await _httpContextProxy.GetValueAsync("FontSize") is string cookie)
        {
            _fontSize = cookie;
        }
        else if (_persistentComponentState.TryTakeFromJson<string>("FontSize", out var restored))
        {
            _fontSize = restored;
        }
        else
        {
            _fontSize = "16px";
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
