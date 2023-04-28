using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Controllers;
using webapi.Models;
using webapi.Services;
using Xunit;

namespace webapi.Tests
{
    public class BookControllerTests
    {
        private readonly List<Book> _books = new()
        {
            new Book { Author = "Mikko", Timestamp = "2000-01-01 00:00:01", Title = "Eepos" },
            new Book { Author = "Seppo", Timestamp = "", Title = "Loru" },
            new Book { Author = "Ismo", Timestamp = "1980-02-02 00:00:00", Title = "Runo" },
            new Book { Author = "Teemu", Timestamp = "not_a_date", Title = "Satu" },
            new Book { Author = "Aatu", Timestamp = "2020-03-03 00:00:00", Title = "Tarina" },
            new Book { Author = "Anna", Timestamp = null, Title = "Opas" },
            new Book { Author = "Liisa", Timestamp = "2000-01-01", Title = "Muistio" },
        };

        [Fact]
        public void Get_ValidParameters_ReturnBooks()
        {
            Mock<IBookService> bookService = new();
            bookService.Setup(x => x.GetBooks()).Returns(_books);
            BookController controller = new(bookService.Object);

            int iterations = _books.Count + 1;
            for (int page = 0; page < iterations; ++page)
            {
                for (int offset = 0; offset < iterations; ++offset)
                {
                    for (int pageSize = 0; pageSize < iterations; ++pageSize)
                    {
                        List<Book>? books = (controller.Get(page, offset, pageSize) as OkObjectResult).Value as List<Book>;

                        System.Range range = GetRangeForPage(page, offset, pageSize);
                        IEnumerable<Book> expected = _books.OrderByDescending(b => GetDateTime(b.Timestamp)).Take(range);
                        Assert.Equal(expected, books);
                    }
                }
            }
        }

        [Fact]
        public void Get_ServiceThrowsException_ReturnBadRequest()
        {
            Mock<IBookService> bookService = new();
            string message = "This exception should be thrown";
            bookService.Setup(x => x.GetBooks()).Throws(new Exception(message));
            BookController controller = new(bookService.Object);

            IActionResult result = controller.Get(0, 0, 0);

            Assert.Equal(message, (result as BadRequestObjectResult).Value);
        }

        [Theory]
        [InlineData("Tarina", "Aatu", "Aatu")]
        [InlineData("Satu", null, null)]
        [InlineData("Loru", "", null)]
        public void Add_ValidParameters_ReturnBook(string title, string author, string expectedAuthor)
        {
            BookController controller = new(Mock.Of<IBookService>());
            AddBookCommand command = new() { Author = author, Title = title };

            IActionResult result = controller.Add(command);

            Book book = (result as OkObjectResult).Value as Book;
            Assert.Equal(new Book { Author = expectedAuthor, Title = title, Timestamp = book.Timestamp }, book);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Add_TitleMissing_ReturnBadRequest(string title)
        {
            BookController controller = new(Mock.Of<IBookService>());
            AddBookCommand command = new() { Author = "Aatu", Title = title };

            IActionResult result = controller.Add(command);

            Assert.Equal("Book title must be provided", (result as BadRequestObjectResult).Value);
        }

        [Fact]
        public void Add_ServiceThrowsException_ReturnBadRequest()
        {
            string message = "This exception should be thrown";
            Mock<IBookService> bookService = new();
            bookService.Setup(x => x.AddBook(It.IsAny<Book>())).Throws(new Exception(message));
            BookController controller = new(bookService.Object);
            AddBookCommand command = new() { Author = "Aatu", Title = "Satu" };

            IActionResult result = controller.Add(command);

            Assert.Equal(message, (result as BadRequestObjectResult).Value);
        }

        private static System.Range GetRangeForPage(int page, int offset, int pageSize)
        {
            return new System.Range(new Index(page * pageSize + offset), new Index((page + 1) * pageSize + offset));
        }

        private static DateTime? GetDateTime(string date)
        {
            return DateTime.TryParse(date, out DateTime dt) ? dt : null;
        }
    }
}
