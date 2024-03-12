using System;
using System.Runtime.Serialization;

[DataContract]
public class Book
{
    [DataMember]
    public int BookId { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public int AuthorId { get; set; }

    [DataMember]
    public int GenreId { get; set; }

    [DataMember]
    public DateTime PublicationDate { get; set; }

    [DataMember]
    public bool IsDeleted { get; set; }

    // Additional constructor to initialize properties
    public Book(int bookId, string title, int authorId, int genreId, DateTime publicationDate, bool isDeleted)
    {
        BookId = bookId;
        Title = title;
        AuthorId = authorId;
        GenreId = genreId;
        PublicationDate = publicationDate;
        IsDeleted = isDeleted;
    }
}
