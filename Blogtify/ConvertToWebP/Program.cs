using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;

string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Blogtify.Client"));
string contentPath = Path.Combine(projectRoot, "Content");
string outputFolder = Path.Combine(projectRoot, "wwwroot");

if (!Directory.Exists(contentPath))
{
    Console.WriteLine($"Folder not found: {contentPath}");
    return;
}

if (!Directory.Exists(outputFolder))
{
    Directory.CreateDirectory(outputFolder);
}

var files = Directory.GetFiles(contentPath, "*.*", SearchOption.AllDirectories)
                .Where(f => new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp" }
                .Contains(Path.GetExtension(f).ToLower()));

foreach (var file in files)
{
    using var image = SixLabors.ImageSharp.Image.Load(file);
    var output = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(file) + ".webp");
    image.Save(output, new WebpEncoder());
}
