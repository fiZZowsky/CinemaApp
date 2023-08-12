using CinemaApp.Application.Dtos;
using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class MovieDetailsController : Controller
    {
        private readonly IMovieDetailsService _movieDetailsService;

        public MovieDetailsController(IMovieDetailsService movieDetailsService)
        {
            _movieDetailsService = movieDetailsService;
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieDetailsDto movieDetails)
        {
            await _movieDetailsService.Create(movieDetails);
            return RedirectToAction(nameof(Create)); // TODO: Refactor
        }
    }
}
