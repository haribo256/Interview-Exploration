using PersonalBookLibrary.Models;
using PersonalBookLibrary.Navigation;
using PersonalBookLibrary.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace PersonalBookLibrary.ViewModels;

public class BookListViewModel
    : ViewModelBase, IQueryAttributable
{
    private static readonly List<string> SortModeDisplayOptions = new List<string>()
    {
        "Title (ascending)",
        "Title (descending)",
        "Author (ascending)",
        "Author (descending)",
        "Year (ascending)",
        "Year (descending)",
    };

    private readonly IFileSystemService _fileSystemService;
    private readonly IPreferencesService _preferencesService;
    private readonly IPlatformService _platformService;

    private bool _isInitialized = false;
    private List<BookViewModel> _allBooks = new List<BookViewModel>();
    private ObservableCollection<BookViewModel> _filteredBooks = new ObservableCollection<BookViewModel>();
    private string _searchFilter = string.Empty;
    private SortCategory _sortCategory = SortCategory.Title;
    private SortMode _sortMode = SortMode.Ascending;

    public BookListViewModel(
        IFileSystemService fileSystemService,
        IPreferencesService preferencesService,
        IPlatformService platformService)
    {
        _fileSystemService = fileSystemService;
        _preferencesService = preferencesService;
        _platformService = platformService;

        AddBookCommand = new Command(async () => await OnAddBookAsync());
        ChangeSortingCommand = new Command(async () => await OnChangeSortingAsync());
    }

    public ObservableCollection<BookViewModel> FilteredBooks
    {
        get => _filteredBooks;
        set => SetProperty(ref _filteredBooks, value);
    }

    public string SearchFilter
    {
        get => _searchFilter;
        set
        {
            if (SetProperty(ref _searchFilter, value))
            {
                UpdateFilteredBooks();
            }
        }
    }

    public SortCategory SortCategory
    {
        get => _sortCategory;
        private set
        {
            if (SetProperty(ref _sortCategory, value))
            {
                UpdateFilteredBooks();
            }
        }
    }

    public SortMode SortMode
    {
        get => _sortMode;
        private set
        {
            if (SetProperty(ref _sortMode, value))
            {
                UpdateFilteredBooks();
            }
        }
    }

    public ICommand AddBookCommand { get; }

    public ICommand ChangeSortingCommand { get; }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey(NavigationParameters.NewBookReturnParameter))
        {
            var newBook = query[NavigationParameters.NewBookReturnParameter] as BookViewModel;

            if (newBook != null)
            {
                _allBooks.Add(newBook);

                await SaveDataAsync();

                UpdateFilteredBooks();
            }
        }
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        Stream savedData = null;

        if (_preferencesService.GetIsFirstLaunch() == true)
        {
            var promptResult = await _platformService.DisplayPromptAsync("Application first launch", "Initialize with sample data?", "Yes", "No");

            if (promptResult)
            {
                try
                {
                    savedData = await _fileSystemService.GetTestDataStreamAsync();
                }
                catch (Exception ex)
                {
                    await _platformService.DisplayAlertAsync("Error", "Failed to load initial data.", "OK");
                    savedData = null;
                }
            }

            _preferencesService.SetIsFirstLaunch(false);
        }

        if (savedData == null)
        {
            savedData = await _fileSystemService.GetSaveDataStreamAsync();
        }

        if (savedData != null)
        {
            try
            {
                await LoadDataAsync(savedData);
            }
            catch (Exception ex)
            {
                await _platformService.DisplayAlertAsync("Error", "Failed to load saved data.", "OK");
            }
        }

        _isInitialized = true;
    }

    private async Task OnAddBookAsync()
    {
        await Shell.Current.GoToAsync(NavigationTargets.NewBook);
    }

    private async Task OnChangeSortingAsync()
    {
        var result = await _platformService.DisplayActionSheet("Select sort mode", "Cancel", null, SortModeDisplayOptions.ToArray());

        var resultIndex = SortModeDisplayOptions.IndexOf(result);

        if (resultIndex >= 0 && resultIndex < SortModeDisplayOptions.Count)
        {
            SortMode = (resultIndex % 2 == 0) ?
                SortMode.Ascending :
                SortMode.Descending;

            switch (resultIndex)
            {
                case 0:
                case 1:
                    SortCategory = SortCategory.Title;
                    break;
                case 2:
                case 3:
                    SortCategory = SortCategory.Author;
                    break;
                case 4:
                case 5:
                    SortCategory = SortCategory.Year;
                    break;
            }
        }
    }

    private void UpdateFilteredBooks()
    {
        var query = _allBooks.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchFilter))
        {
            query = query
                .Where(b =>
                    b.Title.ToLowerInvariant().Contains(SearchFilter.ToLowerInvariant()) ||
                    b.Author.ToLowerInvariant().Contains(SearchFilter.ToLowerInvariant()));
        }

        switch (SortCategory)
        {
            default:
            case SortCategory.Title:
                query = (SortMode == SortMode.Ascending) ?
                    query.OrderBy(b => b.Title) :
                    query.OrderByDescending(b => b.Title);
                break;
            case SortCategory.Author:
                query = (SortMode == SortMode.Ascending) ?
                    query.OrderBy(b => b.Author) :
                    query.OrderByDescending(b => b.Author);
                break;
            case SortCategory.Year:
                query = (SortMode == SortMode.Ascending) ?
                    query.OrderBy(b => b.PublicationYear) :
                    query.OrderByDescending(b => b.PublicationYear);
                break;
        }

        FilteredBooks = new ObservableCollection<BookViewModel>(query);
    }

    private async Task LoadDataAsync(Stream inputStream)
    {
        if (inputStream.Length == 0)
        {
            return;
        }

        var storedBooks = await JsonSerializer.DeserializeAsync<List<BookModel>>(inputStream);

        _allBooks.Clear();
        _allBooks.AddRange(
            storedBooks
                .Select(b => new BookViewModel(b))
                .ToList());

        UpdateFilteredBooks();
    }

    private async Task SaveDataAsync()
    {
        using (var dataFile = await _fileSystemService.GetSaveDataStreamAsync())
        {
            await JsonSerializer.SerializeAsync(dataFile, _allBooks);

            await dataFile.FlushAsync();
        }
    }
}
