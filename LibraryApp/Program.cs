using System;
using System.Collections.Generic;


class Program
{
    static void Main()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

           dbHelper.OpenAndReturnConnection();
      
            //create
        //  dbHelper.InsertBook(11, "Book", 3, 3, DateTime.Now);
       Book newBook = new Book(11, "", 3, 3, DateTime.Now, false);

        dbHelper.InsertBook(newBook);
           // READ
        dbHelper.ReadBooks();

         // UPDATE
        dbHelper.UpdateBookTitle(11, "Updated Book Title");

         // DELETE
        dbHelper.DeleteBook(11);

         
         //like query on title
        dbHelper.SearchBooksByTitle("Adventure");

         //Between query on publication date
         dbHelper.SearchBooksByPublicationDate(new DateTime(2022, 01, 01), new DateTime(2023, 12, 31));

          //  INNER JOIN query on books and authors
         dbHelper.GetBooksAndAuthors();

          // JOIN query with conditions on books, authors, and genres
    dbHelper.GetBooksAuthorsGenresByPublicationDate(new DateTime(2022, 01, 01));

     //  INNER JOIN query with conditions on books, authors, and borrowers
    dbHelper.GetBooksAuthorsBorrowersByBorrowerName("John Doe");

    //groupby query on books and genres
    dbHelper.GetGenreBookCount(1);

 


dbHelper.ReadBooks(true);



bool softDeleteSuccess = dbHelper.SoftDeleteBook(4);

if (softDeleteSuccess)
{
    Console.WriteLine("Soft delete was successful.");
}
else
{
    Console.WriteLine("Soft delete failed.");
}




    }
}