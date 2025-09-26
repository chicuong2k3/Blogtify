namespace Blogtify.Client.Models;

public class ContentDto
{
    public int Id { get; set; }
    public string Route { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Cover { get; set; }
    public string? Category { get; set; }
    public bool IsDraft { get; set; }
    public string? Details { get; set; }
    public DateTime? LastModified { get; set; }
    public string? SeoDescriptions { get; set; }
}
