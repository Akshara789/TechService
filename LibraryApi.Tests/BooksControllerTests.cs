
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Controllers;
using LibraryApi.Business; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;


[TestClass]
public class BooksControllerTests
{
    [TestMethod]
    public void ReadBooks_ShouldReturnOkResult()
    {
        // Arrange
        var businessLayer = new BusinessLayer(); //  the default constructor
        var controller = new BooksController(businessLayer);

        // Act
        var result = controller.ReadBooks();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void UpdateBookTitle_ShouldReturnOkResult()
    {
        // Arrange
        var businessLayer = new BusinessLayer(); // the default constructor
        var controller = new BooksController(businessLayer);
        int bookId = 1;
        string newTitle = "New Book Title";

        // Act
        var result = controller.UpdateBookTitle(bookId, newTitle);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void DeleteBook_ShouldReturnOkResult()
    {
        // Arrange
        var businessLayer = new BusinessLayer(); // the default constructor
        var controller = new BooksController(businessLayer);
        int bookId = 1;

        // Act
        var result = controller.DeleteBook(bookId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void AddBook_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var businessLayer = new BusinessLayer(); //  the default constructor
        var controller = new BooksController(businessLayer);
        var newBook = new Book(1, "Sample Title", 1, 1, DateTime.Now, false);

        // Act
        var result = controller.AddBook(newBook);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }


[TestMethod]
public void UpdateBookTitle_InvalidBookId_ShouldReturnBadRequest()
{
    // Arrange
    var businessLayer = new BusinessLayer(); // the default constructor
    var controller = new BooksController(businessLayer);
    int invalidBookId = -1;
    string newTitle = "New Book Title";

    // Act
    var result = controller.UpdateBookTitle(invalidBookId, newTitle);

    // Assert
    Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
}





   
}
