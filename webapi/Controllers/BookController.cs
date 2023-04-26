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
    public IActionResult Get(int page, int offset)
    {
        try
        {
            using SqliteDbContext dbContext = new();
            dbContext.Database.EnsureCreated();
            List<Book> books = dbContext.Books.AsEnumerable()
                .OrderByDescending(b => GetDateTime(b.Timestamp))
                .Take(GetRangeForPage(page, offset))
                .ToList();
            return Ok(books);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost(Name = "AddBook")]
    public IActionResult Add([FromBody] AddBookCommand command)
    {
        try
        {
            if (string.IsNullOrEmpty(command.Title))
            {
                return BadRequest("Book title must be provided");
            }
            using SqliteDbContext dbContext = new();
            dbContext.Database.EnsureCreated();
            Book book = new()
            {
                Title = command.Title,
                Author = !string.IsNullOrEmpty(command.Author) ? command.Author : null,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            };
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
            return Ok(book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static DateTime? GetDateTime(string date)
    {
        return DateTime.TryParse(date, out DateTime dt) ? dt : null;
    }

    private static Range GetRangeForPage(int page, int offset)
    {
        const int PageSize = 20;
        return new Range(new Index(page * PageSize + offset), new Index((page + 1) * PageSize + offset));
    }
}

public class AddBookCommand
{
    public string Title { get; set; }
    public string Author { get; set; }
}