using Microsoft.JSInterop;

namespace Blogtify.Client.Services;

public class CallbackHelper
{
    private readonly BlogContentLayout _parent;
    private readonly string? _identifier;
    private readonly string _url;

    public CallbackHelper(BlogContentLayout parent, string? identifier, string url)
    {
        _parent = parent;
        _identifier = identifier;
        _url = url;
    }

    [JSInvokable]
    public async Task OnDisqusLoaded()
    {
        if (!string.IsNullOrEmpty(_identifier))
        {
            await _parent.ResetDisqus(_identifier, _url);
        }

    }
}
