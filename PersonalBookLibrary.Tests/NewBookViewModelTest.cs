using PersonalBookLibrary.ViewModels;

namespace PersonalBookLibrary.Tests;

public class NewBookViewModelTest
{
    [TestCase(" ", "Title is required.")]
    [TestCase(
        "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        "Title value is too long.")]
    public void Title_WrongValue_ShouldDisplayErrorMessage(string titlePropertyValue, string errorMessage)
    {
        var viewModel = GetSut();

        viewModel.Title = titlePropertyValue;

        Assert.IsNotEmpty(viewModel.TitleErrorMessage);
        Assert.AreEqual(errorMessage, viewModel.TitleErrorMessage);
        Assert.False(viewModel.AddBookCommand.CanExecute(null));
    }

    [TestCase(" ", "Author is required.")]
    [TestCase(
        "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        "Author value is too long.")]
    public void Author_WrongValue_ShouldDisplayErrorMessage(string authorPropertyValue, string errorMessage)
    {
        var viewModel = GetSut();

        viewModel.Author = authorPropertyValue;

        Assert.IsNotEmpty(viewModel.AuthorErrorMessage);
        Assert.AreEqual(errorMessage, viewModel.AuthorErrorMessage);
        Assert.False(viewModel.AddBookCommand.CanExecute(null));
    }

    [TestCase(" ", "Genre is required.")]
    [TestCase(
        "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        "Genre value is too long.")]
    public void Genre_WrongValue_ShouldDisplayErrorMessage(string genrePropertyValue, string errorMessage)
    {
        var viewModel = GetSut();

        viewModel.Genre = genrePropertyValue;

        Assert.IsNotEmpty(viewModel.GenreErrorMessage);
        Assert.AreEqual(errorMessage, viewModel.GenreErrorMessage);
        Assert.False(viewModel.AddBookCommand.CanExecute(null));
    }

    [TestCase(" ", "Publication year is required.")]
    [TestCase("1994.0", "Publication year format is invalid.")]
    [TestCase("-1", "Publication year value is out of range.")]
    [TestCase("9999", "Publication year value is out of range.")]
    public void PublicationYear_WrongValue_ShouldDisplayErrorMessage(string publicationYearPropertyValue, string errorMessage)
    {
        var viewModel = GetSut();

        viewModel.PublicationYear = publicationYearPropertyValue;

        Assert.IsNotEmpty(viewModel.PublicationYearErrorMessage);
        Assert.AreEqual(errorMessage, viewModel.PublicationYearErrorMessage);
        Assert.False(viewModel.AddBookCommand.CanExecute(null));
    }

    [TestCase(" ", "Description is required.")]
    [TestCase(
        "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        "Description value is too long.")]
    public void Description_WrongValue_ShouldDisplayErrorMessage(string descriptionPropertyValue, string errorMessage)
    {
        var viewModel = GetSut();

        viewModel.Description = descriptionPropertyValue;

        Assert.IsNotEmpty(viewModel.DescriptionErrorMessage);
        Assert.AreEqual(errorMessage, viewModel.DescriptionErrorMessage);
        Assert.False(viewModel.AddBookCommand.CanExecute(null));
    }

    [TestCase("Moby Dick")]
    public void Title_CorrectValue_ShouldNotDisplayErrorMessage(string titlePropertyValue)
    {
        var viewModel = GetSut();

        viewModel.Title = titlePropertyValue;

        Assert.IsEmpty(viewModel.TitleErrorMessage);
    }

    [TestCase("Herman Melville")]
    public void Author_CorrectValue_ShouldNotDisplayErrorMessage(string authorPropertyValue)
    {
        var viewModel = GetSut();

        viewModel.Author = authorPropertyValue;

        Assert.IsEmpty(viewModel.AuthorErrorMessage);
    }

    [TestCase("Novel")]
    public void Genre_CorrectValue_ShouldNotDisplayErrorMessage(string genrePropertyValue)
    {
        var viewModel = GetSut();

        viewModel.Genre = genrePropertyValue;

        Assert.IsEmpty(viewModel.GenreErrorMessage);
    }

    [TestCase("1851")]
    public void PublicationYear_CorrectValue_ShouldNotDisplayErrorMessage(string publicationYearPropertyValue)
    {
        var viewModel = GetSut();

        viewModel.PublicationYear = publicationYearPropertyValue;

        Assert.IsEmpty(viewModel.PublicationYearErrorMessage);
    }

    [TestCase("Moby Dick, novel by Herman Melville.")]
    public void Description_CorrectValue_ShouldNotDisplayErrorMessage(string descriptionPropertyValue)
    {
        var viewModel = GetSut();

        viewModel.Description = descriptionPropertyValue;

        Assert.IsEmpty(viewModel.DescriptionErrorMessage);
    }

    [TestCase(" ", "Herman Melville", "Novel", "1851", "Moby Dick, novel by Herman Melville.", false)]
    [TestCase("Moby Dick", " ", "Novel", "1851", "Moby Dick, novel by Herman Melville.", false)]
    [TestCase("Moby Dick", "Herman Melville", " ", "1851", "Moby Dick, novel by Herman Melville.", false)]
    [TestCase("Moby Dick", "Herman Melville", "Novel", " ", "Moby Dick, novel by Herman Melville.", false)]
    [TestCase("Moby Dick", "Herman Melville", "Novel", "1851", " ", false)]
    [TestCase("Moby Dick", "Herman Melville", "Novel", "1851", "Moby Dick, novel by Herman Melville.", true)]
    public void AddBookCommand_ViewModelProperties_CheckCanExecute(
        string title,
        string author,
        string genre,
        string publicationYear,
        string description,
        bool expected)
    {
        var viewModel = GetSut();

        viewModel.Title = title;
        viewModel.Author = author;
        viewModel.Genre = genre;
        viewModel.PublicationYear = publicationYear;
        viewModel.Description = description;

        Assert.AreEqual(expected, viewModel.AddBookCommand.CanExecute(null));
    }

    private NewBookViewModel GetSut()
    {
        return new NewBookViewModel();
    }
}