using Blogtify.Client.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Blogtify.Client.Services;

public class AppDataManager
{
    private readonly HttpClient _httpClient;
    private List<ContentDto>? _cachedContents;

    private readonly Dictionary<string, string> _detailsCache = new();

    public AppDataManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<string> AllPostCategories => GetAllPostCategories();
    public List<string> AllCourseCategories => GetAllCourseCategories();

    public async Task<int> GetTotalContentsAsync(string query, List<string> categories)
    {
        var contents = FilterContents(query, categories);

        foreach (var p in contents)
        {
            if (!NormalizeText(p.Title).Contains(NormalizeText(query)))
            {
                p.Details ??= await GetDetailsAsync(p);
            }
        }

        return contents.Count(p =>
            NormalizeText(p.Title).Contains(NormalizeText(query)) ||
            (p.Details?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
    }

    public async Task<List<ContentDto>> GetContentsAsync(int page, int pageSize, string query, List<string> categories)
    {
        var contents = FilterContents(query, categories);

        var result = new List<ContentDto>();
        foreach (var p in contents)
        {
            var matchTitle = NormalizeText(p.Title).Contains(NormalizeText(query));
            if (!matchTitle)
            {
                p.Details ??= await GetDetailsAsync(p);
            }

            if (matchTitle || (p.Details?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            {
                result.Add(p);
            }
        }

        return result
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public ContentDto? GetContentByRoute(string route)
    {
        return GetAllContents()
            .FirstOrDefault(p => p.Route.Equals(route, StringComparison.OrdinalIgnoreCase));
    }

    public int GetContentMaxId()
    {
        return GetAllContents().OrderByDescending(c => c.Id).First().Id;
    }

    public List<ContentDto> GetRecommendContents(ContentDto content)
    {
        var contents = GetAllContents()
            .Where(c => !string.IsNullOrEmpty(c.Category)
                        && c.Category.Equals(content.Category, StringComparison.InvariantCultureIgnoreCase))
            .OrderByDescending(c => c.Id);

        return [
            ..contents.Where(c => c.Id < content.Id).Take(5),
            ..contents.Where(c => c.Id > content.Id).Take(5)
        ];
    }

    private async Task<string> GetDetailsAsync(ContentDto content)
    {
        if (_detailsCache.TryGetValue(content.Route, out var details))
            return details;

        var html = await _httpClient.GetStringAsync(new Uri(_httpClient.BaseAddress!, content.Route));
        details = ExtractBodyContent(html);
        _detailsCache[content.Route] = details;
        return details;
    }

    private List<ContentDto> GetAllContents()
    {
        if (_cachedContents != null)
            return _cachedContents;

        _cachedContents = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(CustomComponentBase).IsAssignableFrom(t)
                        && t.GetCustomAttribute<RouteAttribute>() is not null)
            .Select(t =>
            {
                var route = t.GetCustomAttribute<RouteAttribute>()?.Template ?? t.Name;

                var meta = t.GetCustomAttribute<PostMetadataAttribute>();
                var title = meta?.Title ?? t.Name.Replace("_", " ");
                var category = meta?.Category;
                var cover = meta?.Cover;
                var isDraft = meta?.IsDraft;
                var id = meta?.Id ?? throw new Exception($"Post '{title}' does not have id.");
                var lastModified = meta?.LastModified;

                return new ContentDto
                {
                    Id = id,
                    Title = title,
                    Route = route,
                    Category = category,
                    Cover = cover,
                    IsDraft = isDraft ?? false,
                    LastModified = lastModified != null
                        ? DateTime.ParseExact(lastModified, "dd-MM-yyyy",
                            System.Globalization.CultureInfo.InvariantCulture)
                        : null
                };
            })
            .Where(c => !c.IsDraft)
            .ToList();

        return _cachedContents;
    }

    private List<ContentDto> FilterContents(string query, List<string> categories)
    {
        query ??= string.Empty;

        var contents = GetAllContents().AsEnumerable();

        if (categories != null && categories.Count > 0)
        {
            contents = contents.Where(p => !string.IsNullOrEmpty(p.Category)
                                           && categories.Contains(p.Category));
        }

        return contents.ToList();
    }

    private string ExtractBodyContent(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var div = doc.DocumentNode
            .SelectSingleNode("//div[contains(@class,'col-lg-9') and contains(@class,'ps-2') and contains(@class,'main-content')]");

        if (div == null)
            return string.Empty;

        var text = div.InnerText;
        return Regex.Replace(text, @"\s+", " ").Trim();
    }

    private List<ContentDto> GetAllPosts()
    {
        return GetAllContents()
            .Where(c => c.Route.StartsWith("/post"))
            .ToList();
    }

    private List<string> GetAllPostCategories()
    {
        return GetAllPosts().Select(p => p.Category).Distinct().ToList()!;
    }

    private List<ContentDto> GetAllCourses()
    {
        return GetAllContents()
            .Where(c => c.Route.StartsWith("/course"))
            .ToList();
    }

    private List<string> GetAllCourseCategories()
    {
        return GetAllCourses().Select(p => p.Category).Distinct().ToList()!;
    }

    private static string NormalizeText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var normalized = text.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
    }
}
