using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieShowService _movieShowService;

        public MovieController(IMovieShowService movieShowService)
        {
            _movieShowService = movieShowService;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieShowService.GetAll();
            return View(movies);
        }
    }
}
