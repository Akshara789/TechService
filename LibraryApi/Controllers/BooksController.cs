
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Business;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;


namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BusinessLayer businessLayer;

        // Inject BusinessLayer through constructor
        public BooksController(BusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer ?? throw new ArgumentNullException(nameof(businessLayer));
        }

      
[HttpGet]
public IActionResult ReadBooks(bool includeSoftDeleted = false)
{
    try
    {
        // Call GetBooks 
        var books = businessLayer.GetBooks(includeSoftDeleted);
        return Ok(books);
    }
    catch (Exception ex)
    {
       
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
    }
}


        [HttpGet("{bookId}")]
        public IActionResult ReadBook(int bookId)
        {
            try
            {
                var book = businessLayer.GetBook(bookId);

                if (book == null)
                {
                    return NotFound(); // Return 404 
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
               
                 return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{bookId}/update-title")]
        public IActionResult UpdateBookTitle(int bookId, [FromBody] string newTitle)
        {
            
              try
    {
        if (bookId <= 0 || string.IsNullOrWhiteSpace(newTitle))
        {
            return BadRequest(new { ErrorMessage = "Invalid input. Please provide a valid book ID and new title." });
        }

        businessLayer.UpdateBookTitle(bookId, newTitle);
        return Ok("Book title updated successfully.");
    }
            catch (Exception ex)
            {
               
                 return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                businessLayer.DeleteBook(bookId);
                return Ok("Book deleted successfully.");
            }
            
            catch (Exception ex)
            {
               
                 return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

      



[HttpPost]
public IActionResult AddBook([FromBody] Book newBook)
{
    try
    {
        // Check if any required field is missing
        if (newBook == null ||
            newBook.BookId <= 0 ||
            string.IsNullOrWhiteSpace(newBook.Title) ||
            newBook.AuthorId <= 0 ||
            newBook.GenreId <= 0 ||
            newBook.PublicationDate == default ||
            newBook.Damages == null)
            // newBook.RepairStatus == null)
        {
            return BadRequest(new { ErrorMessage = "Invalid input. Please provide all required book details." });
        }

        var existingBookWithSameTitle = businessLayer.GetBooks(includeSoftDeleted: true)
            .FirstOrDefault(book => book.Title.Equals(newBook.Title, StringComparison.OrdinalIgnoreCase));

        if (existingBookWithSameTitle != null)
        {
            return BadRequest(new { ErrorMessage = $"Book with the title '{newBook.Title}' already exists." });
        }

        // Check if a book with the same ID already exists
        var existingBook = businessLayer.GetBook(newBook.BookId);
        if (existingBook != null)
        {
            return BadRequest(new { ErrorMessage = $"Book with ID {newBook.BookId} already exists." });
        }

        if (newBook.PublicationDate > DateTime.Now)
        {
            return BadRequest(new { ErrorMessage = "Publication date should not be greater than the current date." });
        }

      

        businessLayer.AddBook(newBook);
       


        return Ok("Book added successfully.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
    }
}



[HttpPatch("{bookId}/update-repair-status")]
public IActionResult UpdateBookRepairStatus(int bookId)
{
    try
    {
        int monthlyBudget = 2000; // Set your monthly budget here
        businessLayer.UpdateBookRepairStatus(bookId, monthlyBudget);

        return Ok($"Book repair status updated successfully for book ID: {bookId}");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
    }
}




 
}
}


