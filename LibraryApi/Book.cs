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

     [DataMember]
    public string Damages { get; set; }

    [DataMember]
    public string RepairStatus { get; set; }

    //  [DataMember]
    // public int SeverityLevel { get; set; }
   
    // internal int SeverityLevel { get; set; }

    
    // Parameterless constructor for JSON deserialization
    public Book()
    {
    }

    // Additional constructor to initialize properties
    public Book(int bookId, string title, int authorId, int genreId, DateTime publicationDate, bool isDeleted,string damages,string repair_status)
    {
        BookId = bookId;
        Title = title;
        AuthorId = authorId;
        GenreId = genreId;
        PublicationDate = publicationDate;
        IsDeleted = isDeleted;
        Damages = damages;
        RepairStatus = repair_status;
       
    }

   
}

  


