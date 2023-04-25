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
    public IEnumerable<Book> Get(int page)
    {
        const int PageSize = 20;
        using SqliteDbContext dbContext = new();
        dbContext.Database.EnsureCreated();
        List<Book> books = dbContext.Books.AsEnumerable().OrderByDescending(b => b.Timestamp).Take(new Range(new Index(page * PageSize), new Index((page + 1) * PageSize))).ToList();
        Console.WriteLine($"book count: {books.Count}");
        foreach (Book book in books)
        {
            Console.WriteLine($"{book.Title} {book.Author} {book.Timestamp}");
        }
        return books;
    }
}
