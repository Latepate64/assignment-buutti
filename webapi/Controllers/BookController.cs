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
    public void Add([FromBody] AddBookCommand book)
    {
        Console.WriteLine($"Add book: {book.Title} {book.Author}");
        if (string.IsNullOrEmpty(book.Title))
        {
            throw new ArgumentNullException("title");
        }
        using SqliteDbContext dbContext = new();
        dbContext.Database.EnsureCreated();
        dbContext.Books.Add(new Book { Title = book.Title, Author = book.Author, Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") });
        dbContext.SaveChanges();
        Console.WriteLine($"Book added");
    }
}

public class AddBookCommand
{
    public string Title { get; set; }
    public string Author { get; set; }
}