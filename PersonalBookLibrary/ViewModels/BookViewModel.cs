using PersonalBookLibrary.Models;

namespace PersonalBookLibrary.ViewModels;

public class BookViewModel
    : ViewModelBase
{
    public BookViewModel(BookModel book)
    {
        ArgumentNullException.ThrowIfNull(book);

        Title = book.Title;
        Author = book.Author;
        Genre = book.Genre;
        PublicationYear = book.PublicationYear;
        Description = book.Description;
    }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public int PublicationYear { get; set; }

    public string Description { get; set; }
}
