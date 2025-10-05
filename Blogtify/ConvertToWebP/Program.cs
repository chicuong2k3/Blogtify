using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;

string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Blogtify.Client"));
string contentPath = Path.Combine(projectRoot, "content");
if (!Directory.Exists(contentPath))
{
    Console.WriteLine($"Folder not found: {contentPath}");
    return;
}

var files = Directory.GetFiles(contentPath, "*.*", SearchOption.AllDirectories);

foreach (var file in files)
{
    using var image = SixLabors.ImageSharp.Image.Load(file);
    var output = Path.Combine("wwwroot", Path.GetFileNameWithoutExtension(file) + ".webp");
    image.Save(output, new WebpEncoder());
}
