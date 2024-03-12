using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DatabaseHelperTests
{
    private DatabaseHelper _databaseHelper;

    [TestInitialize]
    public void Initialize()
    {
         string connectionString = "Server=localhost;Port=3306;Database=library;Uid=root;";
        
        _databaseHelper = new DatabaseHelper();
    }

  
   [TestMethod]
public void InsertBook_ValidBook_Success()
{
    // Arrange
    Book validBook = new Book(
        bookId: 102,
        title: "Sample Title",
        authorId: 2,
        genreId: 3,
        publicationDate: DateTime.Now,
        isDeleted: false
    );

    // Act
    _databaseHelper.InsertBook(validBook);

    // Assert
    // Retrieve the inserted book from the database
    Book insertedBook = _databaseHelper.ReadBook(validBook.BookId);

    // assertions to verify the properties of the inserted book
    Assert.IsNotNull(insertedBook, "Inserted book should not be null.");
    Assert.AreEqual(validBook.Title, insertedBook.Title, "The book title should match the inserted book.");
    Assert.AreEqual(validBook.AuthorId, insertedBook.AuthorId, "The author ID should match the inserted book.");
  
}



    [TestMethod]
    public void InsertBook_InvalidBook_Failure()
    {
        // Arrange
        Book invalidBook = null;

        // Act
        _databaseHelper.InsertBook(invalidBook);

      
    }

    [TestMethod]
    public void SoftDeleteBook_ExistingBook_Success()
    {
        // Arrange
        int existingBookId = 4;

        // Act
        bool result = _databaseHelper.SoftDeleteBook(existingBookId);

        // Assert
        Assert.IsTrue(result, "Soft delete should be successful for an existing book.");
    }

    [TestMethod]
    public void SoftDeleteBook_NonExistingBook_Failure()
    {
        // Arrange
        int nonExistingBookId = 33;

        // Act
        bool result = _databaseHelper.SoftDeleteBook(nonExistingBookId);

        // Assert
        Assert.IsFalse(result, "Soft delete should fail for a non-existing book.");
    }
}
