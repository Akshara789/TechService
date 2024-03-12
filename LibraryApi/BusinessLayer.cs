
using System;
using System.Collections.Generic;
using System.Linq; 

namespace LibraryApi.Business
{
 

    public class BusinessLayer
    {
        private readonly DatabaseHelper databaseHelper;

        public BusinessLayer()
        {
            databaseHelper = new DatabaseHelper();
        }

  

      public List<Book> GetBooks(bool includeSoftDeleted)
        {
            // Retrieve all books from the database
            var allBooks = databaseHelper.ReadBooks(includeSoftDeleted);

          

           

            return allBooks;
        }

        public Book GetBook(int bookId)
        {
        
            return databaseHelper.ReadBook(bookId);
        }

        public void UpdateBookTitle(int bookId, string newTitle)
        {
            
            if (bookId <= 0)
        {
            throw new ArgumentException("Invalid book ID.", nameof(bookId));
        }

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("New title cannot be empty or null.", nameof(newTitle));
        }
            databaseHelper.UpdateBookTitle(bookId, newTitle);
        }

        public void DeleteBook(int bookId)
        {
            
            if (bookId <= 0)
        {
            throw new ArgumentException("Invalid book ID.", nameof(bookId));
        }
        
          var existingBook = databaseHelper.ReadBook(bookId);
    if (existingBook == null)
    {
        throw new ArgumentException("Book with the specified ID does not exist.", nameof(bookId));
    }
            databaseHelper.DeleteBook(bookId);
        }

        public void AddBook(Book newBook)
        {
            

            // Validate the new book
            if (newBook == null)
            {
                throw new ArgumentException("Invalid book data.");
            }
    

            
            databaseHelper.InsertBook(newBook);
        }

    
    public void UpdateBookRepairStatus(int bookId, int monthlyBudget)
{
    // Call the DatabaseHelper method to update the repair status
    databaseHelper.UpdateBookRepairStatus(bookId, monthlyBudget);
}








           
        
}
}



