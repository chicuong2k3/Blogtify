using Microsoft.AspNetCore.Components;

namespace Blogtify.Client.Theming;

public class FontProvider : IDisposable, IFontProvider
{
    private readonly PersistentComponentState _persistentComponentState;
    private readonly IHttpContextProxy _httpContextProxy;
    private PersistingComponentStateSubscription _persistingComponentStateSubscription;

    private string? _font;
    private string? _fontSize;

    public FontProvider(
        PersistentComponentState persistentComponentState,
        IHttpContextProxy httpContextProxy)
    {
        _persistentComponentState = persistentComponentState;
        _httpContextProxy = httpContextProxy;
        _persistingComponentStateSubscription =
            _persistentComponentState.RegisterOnPersisting(PersistFontAndSize);
    }

    public event FontChangedHandler? FontChanged;
    public event FontSizeChangedHandler? FontSizeChanged;

    public async Task SetFontAsync(string font)
    {
        _font = font;
        FontChanged?.Invoke(font);

        await _httpContextProxy.SetValueAsync("font", font);

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

        fontStr = await _httpContextProxy.GetValueAsync("font");


        if (string.IsNullOrEmpty(fontStr) &&
            _persistentComponentState.TryTakeFromJson<string>("Font", out var restored))
        {
            fontStr = restored;
        }

        _font = !string.IsNullOrEmpty(fontStr) ? fontStr : "Inter";

    }

    public async Task SetFontSizeAsync(string size)
    {
        _fontSize = size;
        FontSizeChanged?.Invoke(size);

        await _httpContextProxy.SetValueAsync("fontSize", size);

    }

    public async Task<string> GetFontSizeAsync()
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

        sizeStr = await _httpContextProxy.GetValueAsync("fontSize");


        if (string.IsNullOrEmpty(sizeStr) &&
            _persistentComponentState.TryTakeFromJson<string>("FontSize", out var restored))
        {
            sizeStr = restored;
        }

        _fontSize = !string.IsNullOrEmpty(sizeStr) ? sizeStr : "14px";
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
