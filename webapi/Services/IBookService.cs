using webapi.Models;

namespace webapi.Services
{
    public interface IBookService
    {
        void AddBook(Book book);
        List<Book> GetBooks();
    }
}
