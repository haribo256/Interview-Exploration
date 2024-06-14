using PersonalBookLibrary.ViewModels;

namespace PersonalBookLibrary.Pages;

public partial class NewBookPage : ContentPage
{
	public NewBookPage(NewBookViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}
}