using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery());
            return View(movies);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieCommand command)
        {
            if(!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
