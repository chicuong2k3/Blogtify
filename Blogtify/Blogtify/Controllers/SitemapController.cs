using Blogtify.Client.Models;
using Blogtify.Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Blogtify.Controllers;

[ApiController]
[Route("sitemap.xml")]
public class SitemapController : ControllerBase
{
    private readonly AppDataManager _appDataManager;

    public SitemapController(AppDataManager appDataManager)
    {
        _appDataManager = appDataManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetSitemap()
    {
        var request = HttpContext.Request;
        var baseUri = $"{request.Scheme}://{request.Host.Value}";

        List<ContentDto> contents = [];
        List<ContentDto> temp = [];
        int page = 1;
        do
        {
            temp = await _appDataManager.GetContentsAsync(page, 50, string.Empty, [], "desc");
            contents.AddRange(temp);
            page++;
        } while (temp.Any());



        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

        foreach (var content in contents)
        {
            var fullUrl = $"{baseUri}/{content.Route.TrimStart('/')}";
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{fullUrl}</loc>");
            sb.AppendLine($"    <lastmod>{(content.LastModified ?? DateTime.UtcNow):yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>daily</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml");
    }
}
