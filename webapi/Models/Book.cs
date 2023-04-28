using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class Book : IEquatable<Book>
{
    [Key]
    public string Title { get; set; }
    public string? Author { get; set; }
    public string Timestamp { get; set; }

    public bool Equals(Book? other)
    {
        return other != null &&
            Title == other.Title &&
            Author == other.Author &&
            Timestamp == other.Timestamp;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Book);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Author, Timestamp);
    }

    public override string ToString()
    {
        return $"{Title} {Author} {Timestamp}";
    }
}
