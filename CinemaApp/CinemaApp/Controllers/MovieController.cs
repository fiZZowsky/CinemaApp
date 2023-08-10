using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Domain.Entities.Movie movie)
        {
            await _movieService.Create(movie);
            return RedirectToAction(nameof(Create)); // TODO: Refactor
        }
    }
}
