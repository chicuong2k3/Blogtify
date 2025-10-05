using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;

string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Blogtify.Client"));
string contentPath = Path.Combine(projectRoot, "Content");

if (!Directory.Exists(contentPath))
{
    Console.WriteLine($"Folder not found: {contentPath}");
    return;
}

var files = Directory.GetFiles(contentPath, "*.*", SearchOption.AllDirectories);
