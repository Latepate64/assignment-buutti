namespace webapi;

public class Book
{
    public Book(string title, string? author, DateOnly dateAdded)
    {
        Title = title;
        Author = author;
        DateAdded = dateAdded;
    }

    public string Title { get; set; }

    public string? Author { get; set; }
    public DateOnly DateAdded { get; set; }


}
