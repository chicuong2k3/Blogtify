using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;

var files = Directory.GetFiles("content", "*.*", SearchOption.AllDirectories)
    .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".gif"));

foreach (var file in files)
{
    using var image = SixLabors.ImageSharp.Image.Load(file);
    var output = Path.Combine("wwwroot", Path.GetFileNameWithoutExtension(file) + ".webp");
    image.Save(output, new WebpEncoder());
}
