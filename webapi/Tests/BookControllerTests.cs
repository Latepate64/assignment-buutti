using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using webapi.Controllers;
using Xunit;

namespace webapi.Tests
{
    public class BookControllerTests
    {
        private readonly List<Book> _books = new()
        {
            new Book { Author = "Mikko", Timestamp = "1980", Title = "Eepos" },
            new Book { Author = "Ismo", Timestamp = "1990", Title = "Runo" },
            new Book { Author = "Aatu", Timestamp = "2000", Title = "Tarina" },
            new Book { Author = "Seppo", Timestamp = "2010", Title = "Loru" },
            new Book { Author = "Teemu", Timestamp = "2020", Title = "Satu" },
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

                        System.Range range = new System.Range(new Index(page * pageSize + offset), new Index((page + 1) * pageSize + offset));
                        Assert.Equal(_books.Take(range), books);
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
        [InlineData("Aatu", "Tarina")]
        [InlineData(null, "Tarina")]
        [InlineData("Aatu", null)]
        public void Add_ValidParameters_ReturnBook(string author, string title)
        {
            BookController controller = new(Mock.Of<IBookService>());
            AddBookCommand command = new() { Author = author, Title = title };

            IActionResult result = controller.Add(command);

            Book book = (result as OkObjectResult).Value as Book;
            Assert.Equal(new Book { Author = author, Title = title, Timestamp = book.Timestamp }, book);
        }
    }
}
