using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Controllers;
using Xunit;

namespace webapi.Tests
{
    public class BookControllerTests
    {
        private readonly List<Book> _books = new()
        {
            new Book { Author = "Aatu", Timestamp = "2000", Title = "Tarina" },
            new Book { Author = "Seppo", Timestamp = "2010", Title = "Loru" },
            new Book { Author = "Teemu", Timestamp = "2020", Title = "Satu" },
        };

        [Fact]
        public void Get_ValidParameters_ShouldPass()
        {
            Mock<IBookService> bookService = new();
            bookService.Setup(x => x.GetBooks()).Returns(_books);
            BookController controller = new(bookService.Object);

            List<Book>? books = (controller.Get(0, 0) as OkObjectResult).Value as List<Book>;

            Assert.Equal(_books, books);
        }
    }
}
