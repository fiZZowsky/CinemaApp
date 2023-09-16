using TicketScanner.ViewModels;

namespace TicketScanner;

public partial class MainPage : ContentPage
{
    private MovieShowViewModel _viewModel; 
    public MainPage(MovieShowViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.GetMoviesShowsListCommand.Execute(null);
    }
}

