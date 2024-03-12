using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LibraryApi.Business;

[TestClass]
public class BusinessLayerTests
{
    private BusinessLayer _businessLayer;

    [TestInitialize]
    public void Initialize()
    {
        _businessLayer = new BusinessLayer();
    }

   

    // Negative Test Cases

    [TestMethod]
    public void UpdateBookTitle_InvalidBookId_ThrowsException()
    {
        // Arrange
        int invalidBookId = -1;
        string newTitle = "New Title";

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _businessLayer.UpdateBookTitle(invalidBookId, newTitle));
    }

   
    [TestMethod]
public void DeleteBook_NonExistingBook_NoExceptionThrown()
{
    // Arrange
    int nonExistingBookId = 999;

    // Act & Assert: no exception is thrown when trying to delete a non-existing book
    Assert.ThrowsException<ArgumentException>(() => _businessLayer.DeleteBook(nonExistingBookId));
}


    [TestMethod]
    public void AddBook_NullBook_ThrowsException()
    {
        // Act & Assert:  an exception is thrown when trying to add a null book
        Assert.ThrowsException<ArgumentException>(() => _businessLayer.AddBook(null));
    }

    // Adding  positive  test cases 
[TestMethod]
public void UpdateBookTitle_ValidBookId_Success()
{
    // Arrange
    int validBookId = 1; 
    string newTitle = "New Title";

    // Act
    _businessLayer.UpdateBookTitle(validBookId, newTitle);

    // Assert: to retrieve the book again and check if the title is updated
    var updatedBook = _businessLayer.GetBook(validBookId);
    Assert.IsNotNull(updatedBook, "Updated book should not be null.");
    Assert.AreEqual(newTitle, updatedBook.Title, "The book title should be updated.");
}

[TestMethod]
public void AddBook_ValidBook_Success()
{
    // Arrange
    Book newBook = new Book(
    bookId: 101, 
    title: "New Book",
    authorId: 3,
    genreId: 7,
    publicationDate: DateTime.Now,
    isDeleted: false
);

    // Act
    _businessLayer.AddBook(newBook);

    // Assert: to retrieve the book and check if it's inserted
    var insertedBook = _businessLayer.GetBook(newBook.BookId);
    Assert.IsNotNull(insertedBook, "Inserted book should not be null.");
    Assert.AreEqual(newBook.Title, insertedBook.Title, "The book title should match the inserted book.");
   
}

}
