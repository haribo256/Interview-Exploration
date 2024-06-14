using PersonalBookLibrary.ViewModels;

namespace PersonalBookLibrary.Pages
{
    public partial class BookListPage :
        ContentPage
    {
        public BookListViewModel? ViewModel => BindingContext as BookListViewModel;

        public BookListPage(BookListViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await ViewModel?.InitializeAsync();
        }
    }
}
