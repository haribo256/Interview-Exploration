using PersonalBookLibrary.Models;
using PersonalBookLibrary.Navigation;

namespace PersonalBookLibrary.ViewModels;

public class NewBookViewModel
    : ViewModelBase
{
    private string _title = string.Empty;
    private string _author = string.Empty;
    private string _genre = string.Empty;
    private string _publicationYear = string.Empty;
    private string _description = string.Empty;

    private string _titleErrorMessage = string.Empty;
    private string _authorErrorMessage = string.Empty;
    private string _genreErrorMessage = string.Empty;
    private string _publicationYearErrorMessage = string.Empty;
    private string _descriptionErrorMessage = string.Empty;

    public NewBookViewModel()
    {
        AddBookCommand = new Command(
            async () => await OnAddBookCommandAsync(),
            () => CanAddBook());
    }

    public int StringFieldMaxLength => 100;
    public int YearFieldMaxLength => 4;

    public string Title
    {
        get => _title;
        set
        {
            if (SetProperty<string>(ref _title, value))
            {
                TitleErrorMessage = ValidateStringProperty(value, "Title");
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string Author
    {
        get => _author;
        set
        {
            if (SetProperty<string>(ref _author, value))
            {
                AuthorErrorMessage = ValidateStringProperty(value, "Author");
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string Genre
    {
        get => _genre;
        set
        {
            if (SetProperty<string>(ref _genre, value))
            {
                GenreErrorMessage = ValidateStringProperty(value, "Genre");
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string PublicationYear
    {
        get => _publicationYear;
        set
        {
            if (SetProperty<string>(ref _publicationYear, value))
            {
                PublicationYearErrorMessage = ValidateIntProperty(value, "Publication year", 0, DateTime.Now.Year);
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (SetProperty<string>(ref _description, value))
            {
                DescriptionErrorMessage = ValidateStringProperty(value, "Description");
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string TitleErrorMessage
    {
        get => _titleErrorMessage;
        private set
        {
            if (SetProperty<string>(ref _titleErrorMessage, value))
            {
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string AuthorErrorMessage
    {
        get => _authorErrorMessage;
        private set
        {
            if (SetProperty<string>(ref _authorErrorMessage, value))
            {
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string GenreErrorMessage
    {
        get => _genreErrorMessage;
        private set
        {
            if (SetProperty<string>(ref _genreErrorMessage, value))
            {
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string PublicationYearErrorMessage
    {
        get => _publicationYearErrorMessage;
        private set
        {
            if (SetProperty<string>(ref _publicationYearErrorMessage, value))
            {
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public string DescriptionErrorMessage
    {
        get => _descriptionErrorMessage;
        private set
        {
            if (SetProperty<string>(ref _descriptionErrorMessage, value))
            {
                AddBookCommand.ChangeCanExecute();
            }
        }
    }

    public Command AddBookCommand { get; }

    private async Task OnAddBookCommandAsync()
    {
        var navigationParameters = new Dictionary<string, object>()
        {
            { NavigationParameters.NewBookReturnParameter , new BookViewModel(
                new BookModel(Title, Author, Genre, int.Parse(PublicationYear), Description)) }
        };

        await Shell.Current.GoToAsync(NavigationTargets.GoBack, navigationParameters);
    }

    private string ValidateStringProperty(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return $"{propertyName} is required.";
        }

        if (value.Length > StringFieldMaxLength)
        {
            return $"{propertyName} value is too long.";
        }

        return string.Empty;
    }

    private static string ValidateIntProperty(string value, string propertyName, int minValue, int maxValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return $"{propertyName} is required.";
        }

        var intValue = 0;

        if (!int.TryParse(value, out intValue))
        {
            return $"{propertyName} format is invalid.";
        }

        if (intValue < minValue || intValue > maxValue)
        {
            return $"{propertyName} value is out of range.";
        }

        return string.Empty;
    }

    private bool CanAddBook()
    {
        return
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(Author) &&
            !string.IsNullOrWhiteSpace(Genre) &&
            !string.IsNullOrWhiteSpace(PublicationYear) &&
            !string.IsNullOrWhiteSpace(Description) &&

            string.IsNullOrWhiteSpace(TitleErrorMessage) &&
            string.IsNullOrWhiteSpace(AuthorErrorMessage) &&
            string.IsNullOrWhiteSpace(GenreErrorMessage) &&
            string.IsNullOrWhiteSpace(PublicationYearErrorMessage) &&
            string.IsNullOrWhiteSpace(DescriptionErrorMessage);
    }
}
