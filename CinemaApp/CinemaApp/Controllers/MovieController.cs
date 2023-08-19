using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle;
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

        [Route("CinemaApp/{encodedTitle}/Details")]
        public async Task<IActionResult> Details (string encodedTitle)
        {
            var dto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            return View(dto);
        }
    }
}
