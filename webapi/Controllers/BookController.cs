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
        return new List<Book>
        {
            new Book("Viisasten kivi", "JK Rowling", new DateOnly(1996, 1, 1)),
            new Book("LOTR", "Tolkien", new DateOnly(1950, 3, 10)),
            new Book("Muumit", "Tove Janson", new DateOnly(1970, 11, 27)),
        };
    }
}
