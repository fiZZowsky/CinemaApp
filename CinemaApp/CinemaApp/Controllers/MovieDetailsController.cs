using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAllMoviesDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.MVC.Controllers
{
    public class MovieDetailsController : Controller
    {
        private readonly IMediator _mediator;

        public MovieDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _mediator.Send(new GetAllMoviesDetailsQuery());
            return View(movies);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieDetailsCommand command)
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
