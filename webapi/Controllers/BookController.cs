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
        //Console.WriteLine($"book count: {books.Count}");
        //foreach (Book book in books)
        //{
        //    Console.WriteLine($"{book.Title} {book.Author} {book.Timestamp}");
        //}
        return books;
    }

    [HttpPost(Name = "AddBook")]
    public Book Add([FromBody] AddBookCommand command)
    {
        Console.WriteLine($"Add book: {command.Title} {command.Author}");
        if (string.IsNullOrEmpty(command.Title))
        {
            throw new ArgumentNullException("title");
        }
        using SqliteDbContext dbContext = new();
        dbContext.Database.EnsureCreated();
        Book book = new() { Title = command.Title, Author = command.Author, Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") };
        dbContext.Books.Add(book);
        dbContext.SaveChanges();
        Console.WriteLine($"Book added");
        return book;
    }
}

public class AddBookCommand
{
    public string Title { get; set; }
    public string Author { get; set; }
}