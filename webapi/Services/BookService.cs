using webapi.Models;

namespace webapi.Services
{
    public class BookService : IBookService
    {
        public void AddBook(Book book)
        {
            using SqliteDbContext dbContext = new();
            dbContext.Database.EnsureCreated();
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
        }

        public List<Book> GetBooks()
        {
            using SqliteDbContext dbContext = new();
            dbContext.Database.EnsureCreated();
            return dbContext.Books.ToList();
        }
    }
}
