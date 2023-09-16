using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketScanner.Entities;
using TicketScanner.Services;

namespace TicketScanner.ViewModels
{
    public partial class MovieShowViewModel : ObservableObject
    {
        private readonly IMovieShowService _movieShowService;
        public ObservableCollection<MovieShow> MoviesShows { get; } = new ObservableCollection<MovieShow>();
        
        public MovieShowViewModel(IMovieShowService movieShowService)
        {
            _movieShowService = movieShowService;
        }

        [ICommand]
        public async void GetMoviesShowsList()
        {
            MoviesShows.Clear();
            var moviesShows = await _movieShowService.GetMovieShows();
            if(moviesShows?.Count > 0)
            {
                foreach(var show in moviesShows)
                {
                    MoviesShows.Add(show);
                }
            }
        }
    }
}
