using Blazored.LocalStorage;

namespace Blogtify.Client.Theming;

public class WebAssemblyHttpContextProxy : IHttpContextProxy
{
    private readonly ILocalStorageService _localStorage;

    public WebAssemblyHttpContextProxy(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public bool IsSupported() => true;

    public async Task<string> GetValueAsync(string key)
    {
        return await _localStorage.GetItemAsync<string>(key) ?? string.Empty;
    }

    public async Task SetValueAsync(string key, string value, DateTimeOffset? expires = null)
    {
        await _localStorage.SetItemAsync(key, value);
    }
}
