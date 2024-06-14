using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using PersonalBookLibrary.Models;
using PersonalBookLibrary.Services;
using PersonalBookLibrary.ViewModels;
using System.Text;

namespace PersonalBookLibrary.Tests;

public class BookListViewModelTest
{
    private const string BookTestData = "[\r\n  {\r\n    \"Author\": \"Herman Melville\",\r\n    \"Description\": \"Moby Dick, novel by Herman Melville.\",\r\n    \"Genre\": \"Novel\",\r\n    \"PublicationYear\": 1851,\r\n    \"Title\": \"Moby Dick\"\r\n  }\r\n]";

    private Mock<IFileSystemService> _fileSystemServiceMock;
    private Mock<IPreferencesService> _preferencesServiceMock;
    private Mock<IPlatformService> _platformServiceMock;

    [SetUp]
    public void Setup()
    {
        _fileSystemServiceMock = new Mock<IFileSystemService>();
        _preferencesServiceMock = new Mock<IPreferencesService>();
        _platformServiceMock = new Mock<IPlatformService>();
    }

    [Test]
    public async Task InitializeAsync_IsFirstLaunch_LoadsTestData()
    {
        _preferencesServiceMock
            .Setup(x => x.GetIsFirstLaunch())
            .Returns(true);

        _platformServiceMock
            .Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _fileSystemServiceMock
            .Setup(x => x.GetTestDataStreamAsync())
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes("[\r\n  {\r\n    \"Author\": \"Herman Melville\",\r\n    \"Description\": \"Moby Dick, novel by Herman Melville.\",\r\n    \"Genre\": \"Novel\",\r\n    \"PublicationYear\": 1851,\r\n    \"Title\": \"Moby Dick\"\r\n  }\r\n]")));

        var viewModel = GetSut();

        await viewModel.InitializeAsync();

        Assert.IsNotNull(viewModel.FilteredBooks);
        Assert.AreEqual(1, viewModel.FilteredBooks.Count);
        Assert.AreEqual("Moby Dick", viewModel.FilteredBooks.First().Title);
    }

    [Test]
    public async Task InitializeAsync_IsNotFirstLaunch_LoadsSavedData()
    {
        _preferencesServiceMock
            .Setup(x => x.GetIsFirstLaunch())
            .Returns(false);

        _platformServiceMock
            .Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _fileSystemServiceMock
            .Setup(x => x.GetSaveDataStreamAsync())
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(BookTestData)));

        var viewModel = GetSut();

        await viewModel.InitializeAsync();

        Assert.IsNotNull(viewModel.FilteredBooks);
        Assert.AreEqual(1, viewModel.FilteredBooks.Count);
        Assert.AreEqual("Moby Dick", viewModel.FilteredBooks.First().Title);
    }

    [TestCase("Title (ascending)", SortCategory.Title, SortMode.Ascending)]
    [TestCase("Title (descending)", SortCategory.Title, SortMode.Descending)]
    [TestCase("Author (ascending)", SortCategory.Author, SortMode.Ascending)]
    [TestCase("Author (descending)", SortCategory.Author, SortMode.Descending)]
    [TestCase("Year (ascending)", SortCategory.Year, SortMode.Ascending)]
    [TestCase("Year (descending)", SortCategory.Year, SortMode.Descending)]

    public async Task ChangeSortingCommand_ChangesSortingMode(string pickerOption, SortCategory expectedCategory, SortMode expectedMode)
    {
        _preferencesServiceMock
            .Setup(x => x.GetIsFirstLaunch())
            .Returns(false);

        _platformServiceMock
            .Setup(x => x.DisplayActionSheet(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()))
            .ReturnsAsync(pickerOption);

        _fileSystemServiceMock
            .Setup(x => x.GetSaveDataStreamAsync())
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(BookTestData)));

        var viewModel = GetSut();

        await viewModel.InitializeAsync();
        viewModel.ChangeSortingCommand.Execute(null);

        Assert.AreEqual(expectedCategory, viewModel.SortCategory);
        Assert.AreEqual(expectedMode, viewModel.SortMode);
    }


    private BookListViewModel GetSut()
    {
        return new BookListViewModel(
            _fileSystemServiceMock.Object,
            _preferencesServiceMock.Object,
            _platformServiceMock.Object);
    }
}
