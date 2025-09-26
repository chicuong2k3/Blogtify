namespace Blogtify.Client.Models;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PostMetadataAttribute : Attribute
{

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Cover { get; }
    public bool IsDraft { get; set; }
    public string? LastModified { get; set; }

}
