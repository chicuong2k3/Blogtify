namespace Blogtify.Client.Theming;

public interface IFontProvider
{
    event FontChangedHandler FontChanged;

    Task<string> GetFontAsync();
    Task SetFontAsync(string font);

    Task<string> GetFontSizeAsync();
    Task SetFontSizeAsync(string size);
}
