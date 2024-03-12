

using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic; 
using LibraryApi.Business;
using System.Linq;




public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper()
    {
        
        connectionString = ReadConnectionStringFromFile();
         Console.WriteLine("Connection opened successfully.");
    }
       private string ReadConnectionStringFromFile()
    {
        try
        {
           
            string filePath = "connectionString.txt";

            // Read the connection string from the file
            string connectionString = System.IO.File.ReadAllText(filePath);

            return connectionString;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading connection string from file: {ex.Message}");
            return null;
        }
    }

  
      public MySqlConnection OpenAndReturnConnection()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);;
        try
        {
            connection.Open();
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening connection: {ex.Message}");
        }
        return connection;
    }



       // CREATE
   
    public void InsertBook(Book book)
{
    if (book == null)
    {
        Console.WriteLine("Invalid book data.");
        return;
    }

    // Basic validations
    if (string.IsNullOrWhiteSpace(book.Title))
    {
        Console.WriteLine("Title cannot be empty or null.");
        return;
    }

    // Validate publication date 
    if (book.PublicationDate > DateTime.Now)
    {
        Console.WriteLine("Publication date cannot be in the future.");
        return;
    }
      // Set RepairStatus to "Not Started" by default
    book.RepairStatus = "Not Started";
     
    
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
            

            string insertQuery = "INSERT INTO books (book_id, title, author_id, genre_id, publication_date,damages,repair_status) " +
                                 "VALUES (@bookId, @title, @authorId, @genreId, @publicationDate,@damages,@repairStatus)";

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@bookId", book.BookId);
                command.Parameters.AddWithValue("@title", book.Title);
                command.Parameters.AddWithValue("@authorId", book.AuthorId);
                command.Parameters.AddWithValue("@genreId", book.GenreId);
                command.Parameters.AddWithValue("@publicationDate", book.PublicationDate);
                command.Parameters.AddWithValue("@damages", (object)book.Damages ?? DBNull.Value);
               command.Parameters.AddWithValue("@repairStatus", book.RepairStatus);
           
     


                command.ExecuteNonQuery();

                Console.WriteLine("Book inserted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting book: {ex.Message}");
        }
    }
}






        // READ
 ///int minGenreId
public List<Book> ReadBooks(bool includeSoftDeleted = false)
{
    List<Book> books = new List<Book>();
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
            string selectQuery = includeSoftDeleted
                ? "SELECT * FROM books"
                : "SELECT * FROM books WHERE is_deleted = false";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book(
                            Convert.ToInt32(reader["book_id"]),
                            Convert.ToString(reader["title"]),
                            Convert.ToInt32(reader["author_id"]),
                            Convert.ToInt32(reader["genre_id"]),
                            Convert.ToDateTime(reader["publication_date"]),
                            Convert.ToBoolean(reader["is_deleted"]),
                             Convert.ToString(reader["damages"]),
                            Convert.ToString(reader["repair_status"])
                           
                        );

                        books.Add(book);

                        // Console.WriteLine($"Book ID: {book.BookId}, Title: {book.Title}, " +
                        //                   $"Author ID: {book.AuthorId}, Genre ID: {book.GenreId}, " +
                        //                   $"Publication Date: {book.PublicationDate}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading books: {ex.Message}");
        }
    }

    return books;
}

//readbook by id


   

    public Book ReadBook(int bookId)
    {
        using (MySqlConnection connection = OpenAndReturnConnection())
        {
            try
            {
                string selectQuery = "SELECT * FROM books WHERE book_id = @bookId";

                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@bookId", bookId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Book book = new Book(
                                Convert.ToInt32(reader["book_id"]),
                                Convert.ToString(reader["title"]),
                                Convert.ToInt32(reader["author_id"]),
                                Convert.ToInt32(reader["genre_id"]),
                                Convert.ToDateTime(reader["publication_date"]),
                                Convert.ToBoolean(reader["is_deleted"]),
                                 Convert.ToString(reader["damages"]),
                                 Convert.ToString(reader["repair_status"])
                               
                            );

                            Console.WriteLine($"Book ID: {book.BookId}, Title: {book.Title}, " +
                                              $"Author ID: {book.AuthorId}, Genre ID: {book.GenreId}, " +
                                              $"Publication Date: {book.PublicationDate},"+
                                               $"Damages: {book.Damages},"+
                                               $"RepairStatus: {book.RepairStatus}"
                                            //    $"SeverityLevel: {book.SeverityLevel}"
                                               );

                            return book;
                        }
                        else
                        {
                            // Book not found
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading book: {ex.Message}");
                return null;
            }
        }
    }




    
   // UPDATE
    public void UpdateBookTitle(int bookId, string newTitle)
    {
        using (MySqlConnection connection = OpenAndReturnConnection())
        {
            try
            {
                

                string updateQuery = "UPDATE books SET title = @newTitle WHERE book_id = @bookId";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@newTitle", newTitle);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Book title updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book title: {ex.Message}");
            }
        }
    }

       // DELETE
    public void DeleteBook(int bookId)
    {
        using (MySqlConnection connection = OpenAndReturnConnection())
        {
            try
            {
                

                string deleteQuery = "DELETE FROM books WHERE book_id = @bookId";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@bookId", bookId);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Book deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book: {ex.Message}");
            }
        }
    }


   
     // SoftDelete method
public bool SoftDeleteBook(int bookId)
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
            string softDeleteQuery = "UPDATE books SET is_deleted = 1 WHERE book_id = @bookId";

            using (MySqlCommand command = new MySqlCommand(softDeleteQuery, connection))
            {
                command.Parameters.AddWithValue("@bookId", bookId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    
                    return true;
                }
                else
                {
                    Console.WriteLine("Book not found or already deleted.");
                    return false;
                }
             }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error performing soft delete: {ex.Message}");
            return false;
        }
    }
}

                                  

public void UpdateBookRepairStatus(int bookId, int monthlyBudget)
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
            // Fetch damages information for the given book based on severity level and monthly budget
            string selectDamageInfo = "SELECT * FROM damages " +
                                      "WHERE severity_level <= 4 " +
                                      "AND repair_cost <= @monthlyBudget " +
                                      "ORDER BY severity_level DESC, repair_cost DESC";

            List<BookDamagesInfo> bookDamagesInfoList = new List<BookDamagesInfo>();

            using (MySqlCommand selectCommand = new MySqlCommand(selectDamageInfo, connection))
            {
                selectCommand.Parameters.AddWithValue("@monthlyBudget", monthlyBudget);

                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BookDamagesInfo damagesInfo = new BookDamagesInfo
                        {
                            DamageId = Convert.ToInt32(reader["damage_id"]),
                            DamageType = Convert.ToString(reader["damage_type"]),
                            SeverityLevel = Convert.ToInt32(reader["severity_level"]),
                            RepairCost = Convert.ToInt32(reader["repair_cost"])
                        };

                        bookDamagesInfoList.Add(damagesInfo);
                    }
                }
            }

            // Update repair status based on the sorted list and monthly budget
            foreach (var info in bookDamagesInfoList)
            {
                int budget=2000;
                // Update the repair status to 'Completed' if within budget
                if (budget >= info.RepairCost)
                {
                    string updateQuery = "UPDATE books SET repair_status = 'Completed' WHERE book_id = @bookId";

                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@bookId", bookId);
                        updateCommand.ExecuteNonQuery();

                        // Deduct the repair cost from the monthly budget
                        monthlyBudget -= info.RepairCost;
                    }
                }
                else
                {
                    // Update the repair status to 'In Progress' if budget is insufficient
                    string updateQuery = "UPDATE books SET repair_status = 'In Progress' WHERE book_id = @bookId";

                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@bookId", bookId);
                        updateCommand.ExecuteNonQuery();
                    }

                    // No need to continue updating if budget is insufficient
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating repair status: {ex.Message}");
        }
    }
}







   
}







