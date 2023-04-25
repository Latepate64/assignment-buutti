using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBooks")]
    public IEnumerable<Book> Get()
    {
        using SqliteDbContext dbContext = new();
        dbContext.Database.EnsureCreated();
        List<Book> books = dbContext.Books.ToList();
        Console.WriteLine($"book count: {books.Count}");
        foreach (Book book in books)
        {
            Console.WriteLine($"{book.Title} {book.Author} {book.Timestamp}");
        }
        return books;
    }
}
