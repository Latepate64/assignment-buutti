using System.ComponentModel.DataAnnotations;

namespace webapi;

public class Book
{
    [Key]
    public string Title { get; set; }
    public string? Author { get; set; }
    public string Timestamp { get; set; }
}
