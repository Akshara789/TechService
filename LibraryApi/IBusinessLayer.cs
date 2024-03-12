using System.Collections.Generic;

namespace LibraryApi.Business
{
    public interface IBusinessLayer
    {
        List<Book> GetBooks(bool includeSoftDeleted, int minGenreId = 6);
        Book GetBook(int bookId);
        void UpdateBookTitle(int bookId, string newTitle);
        void DeleteBook(int bookId);
        void AddBook(Book newBook);
    }
}
