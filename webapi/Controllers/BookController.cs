using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet(Name = "GetBooks")]
    public IActionResult Get(int page, int offset, int pageSize = 20)
    {
        try
        {
            List<Book> books = _bookService.GetBooks()
                .OrderByDescending(b => GetDateTime(b.Timestamp))
                .Take(GetRangeForPage(page, offset, pageSize))
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
            Book book = new()
            {
                Title = command.Title,
                Author = !string.IsNullOrEmpty(command.Author) ? command.Author : null,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            };
            _bookService.AddBook(book);
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

    private static Range GetRangeForPage(int page, int offset, int pageSize)
    {
        return new Range(new Index(page * pageSize + offset), new Index((page + 1) * pageSize + offset));
    }
}
