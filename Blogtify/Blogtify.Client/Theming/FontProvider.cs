using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
    private bool _cookiesSet = false;

    public FontProvider(
        IHttpContextProxy httpContextProxy,
        PersistentComponentState persistentComponentState,
        IJSRuntime jsRuntime)
    {
        _httpContextProxy = httpContextProxy;
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

        if (!_cookiesSet && _httpContextProxy.IsSupported())
        {
            try
            {
                await _httpContextProxy.SetValueAsync("Font", font);
                _cookiesSet = true;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Headers are read-only"))
            {
                _cookiesSet = true;  // Bỏ qua, dùng fallback
            }
        }

        // Áp dụng qua JS (localStorage + font load), bỏ qua prerender/headers
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "font", font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.loadGoogleFont", font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFont", font);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering") || ex.Message.Contains("Headers"))
        {
            // Bỏ qua lỗi
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
        if (_httpContextProxy.IsSupported() && await _httpContextProxy.GetValueAsync("Font") is string cookie)
        {
            fontStr = cookie;
        }
        else
        {
            try
            {
                fontStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "font");
            }
            catch { /* JS chưa sẵn sàng */ }

            if (string.IsNullOrEmpty(fontStr) && _persistentComponentState.TryTakeFromJson<string>("Font", out var restored))
            {
                fontStr = restored;
            }
        }

        _font = !string.IsNullOrEmpty(fontStr) ? fontStr : "Inter";

        // Áp dụng initial font qua JS
        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.loadGoogleFont", _font);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFont", _font);
        }
        catch (InvalidOperationException) { /* Bỏ qua prerender */ }
    }

    public async Task SetFontSizeAsync(string size)
    {
        _fontSize = size;
        FontSizeChanged?.Invoke(size);

        // Set cookie chỉ lần đầu và nếu headers cho phép
        if (!_cookiesSet && _httpContextProxy.IsSupported())
        {
            try
            {
                await _httpContextProxy.SetValueAsync("FontSize", size);
                _cookiesSet = true;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Headers are read-only"))
            {
                _cookiesSet = true;  // Bỏ qua
            }
        }


        // Áp dụng qua JS (localStorage), bỏ qua prerender/headers
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "fontSize", size);
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFontSize", size);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering") || ex.Message.Contains("Headers"))
        {
            // Bỏ qua lỗi
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
        // Ưu tiên: Cookie > localStorage > State > Default
        string? sizeStr = null;
        if (_httpContextProxy.IsSupported() && await _httpContextProxy.GetValueAsync("FontSize") is string cookie)
        {
            sizeStr = cookie;
        }
        else
        {
            try
            {
                sizeStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "fontSize");
            }
            catch { /* JS chưa sẵn sàng */ }

            if (string.IsNullOrEmpty(sizeStr) && _persistentComponentState.TryTakeFromJson<string>("FontSize", out var restored))
            {
                sizeStr = restored;
            }
        }

        _fontSize = !string.IsNullOrEmpty(sizeStr) ? sizeStr : "16px";

        // Áp dụng initial size qua JS
        try
        {
            await _jsRuntime.InvokeVoidAsync("themeSwitcher.setFontSize", _fontSize);
        }
        catch (InvalidOperationException) { /* Bỏ qua prerender */ }
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
