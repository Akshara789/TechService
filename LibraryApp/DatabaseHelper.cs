

using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic; 

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

    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
            string insertQuery = "INSERT INTO books (book_id, title, author_id, genre_id, publication_date) " +
                                 "VALUES (@bookId, @title, @authorId, @genreId, @publicationDate)";

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@bookId", book.BookId);
                command.Parameters.AddWithValue("@title", book.Title);
                command.Parameters.AddWithValue("@authorId", book.AuthorId);
                command.Parameters.AddWithValue("@genreId", book.GenreId);
                command.Parameters.AddWithValue("@publicationDate", book.PublicationDate);

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
                            Convert.ToBoolean(reader["is_deleted"])
                        );

                        books.Add(book);

                        Console.WriteLine($"Book ID: {book.BookId}, Title: {book.Title}, " +
                                          $"Author ID: {book.AuthorId}, Genre ID: {book.GenreId}, " +
                                          $"Publication Date: {book.PublicationDate}");
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


    // details of book having title with adventure
public void SearchBooksByTitle(string keyword)
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT * FROM books WHERE title LIKE @keyword";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Book ID: {reader["book_id"]}, Title: {reader["title"]}, " +
                                          $"Author ID: {reader["author_id"]}, Genre ID: {reader["genre_id"]}, " +
                                          $"Publication Date: {reader["publication_date"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching books by title: {ex.Message}");
        }
    }
}

//  BETWEEN query on the publication_date
public void SearchBooksByPublicationDate(DateTime startDate, DateTime endDate)
{
    using (MySqlConnection connection =OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT * FROM books WHERE publication_date BETWEEN @startDate AND @endDate";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Book ID: {reader["book_id"]}, Title: {reader["title"]}, " +
                                          $"Author ID: {reader["author_id"]}, Genre ID: {reader["genre_id"]}, " +
                                          $"Publication Date: {reader["publication_date"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching books by publication date: {ex.Message}");
        }
    }
}

//  INNER JOIN query on books and authors
public void GetBooksAndAuthors()
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT books.title, authors.author_name " +
                                 "FROM books " +
                                 "INNER JOIN authors ON books.author_id = authors.author_id";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Title: {reader["title"]}, Author: {reader["author_name"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving books and authors: {ex.Message}");
        }
    }
}

//  JOIN query with conditions on books, authors, and genres
public void GetBooksAuthorsGenresByPublicationDate(DateTime startDate)
{
    using (MySqlConnection connection =OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT books.title, authors.author_name, genres.genre_name, books.publication_date " +
                                 "FROM books " +
                                 "JOIN authors ON books.author_id = authors.author_id " +
                                 "JOIN genres ON books.genre_id = genres.genre_id " +
                                 "WHERE books.publication_date > @startDate";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@startDate", startDate);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Title: {reader["title"]}, Author: {reader["author_name"]}, " +
                                          $"Genre: {reader["genre_name"]}, Publication Date: {reader["publication_date"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving books, authors, and genres: {ex.Message}");
        }
    }
}

//  the INNER JOIN query with conditions on books, authors, and borrowers
public void GetBooksAuthorsBorrowersByBorrowerName(string borrowerName)
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT books.title, authors.author_name, borrowers.borrower_name " +
                                 "FROM books " +
                                 "INNER JOIN authors ON books.author_id = authors.author_id " +
                                 "INNER JOIN borrowers ON books.book_id = borrowers.book_id " +
                                 "WHERE borrowers.borrower_name = @borrowerName";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@borrowerName", borrowerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Title: {reader["title"]}, Author: {reader["author_name"]}, " +
                                          $"Borrower: {reader["borrower_name"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving books, authors, and borrowers: {ex.Message}");
        }
    }
}

//groupby query on genres and books

public void GetGenreBookCount(int minBookCount)
{
    using (MySqlConnection connection = OpenAndReturnConnection())
    {
        try
        {
           

            string selectQuery = "SELECT genre_id, COUNT(*) AS book_count " +
                                 "FROM books " +
                                 "GROUP BY genre_id " +
                                 "HAVING COUNT(*) = @minBookCount";

            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@minBookCount", minBookCount);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Genre ID: {reader["genre_id"]}, Book Count: {reader["book_count"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving genre book count: {ex.Message}");
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





 }