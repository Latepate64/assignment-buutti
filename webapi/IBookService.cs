namespace webapi
{
    public interface IBookService
    {
        void AddBook(Book book);
        List<Book> GetBooks();
    }
}
