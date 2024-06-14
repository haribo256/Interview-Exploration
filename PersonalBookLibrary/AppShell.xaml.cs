using PersonalBookLibrary.Navigation;
using PersonalBookLibrary.Pages;

namespace PersonalBookLibrary
{
    public partial class AppShell
        : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(NavigationTargets.BookList, typeof(BookListPage));
            Routing.RegisterRoute(NavigationTargets.NewBook, typeof(NewBookPage));
        }
    }
}
