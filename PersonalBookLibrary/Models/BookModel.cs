namespace PersonalBookLibrary.Models;

public record BookModel(
    string Title, 
    string Author, 
    string Genre, 
    int PublicationYear,
    string Description)
{
}
