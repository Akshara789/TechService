// IDatabaseHelper.cs

using System.Collections.Generic;

namespace LibraryApi.Business
{
    public interface IDatabaseHelper
    {
        List<Book> ReadBooks(bool includeSoftDeleted,int minGenreId);
        Book ReadBook(int bookId);
        void UpdateBookTitle(int bookId, string newTitle);
        void DeleteBook(int bookId);
        void InsertBook(Book newBook);
    }
}
