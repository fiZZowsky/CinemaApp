using AutoMapper;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateMovieShow;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatingById;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatings;
using CinemaApp.Application.CinemaApp.Queries.GetAllHalls.GetAllHalls;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using CinemaApp.Application.CinemaApp.Queries.GetAllMoviesShows;
using CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle;
using CinemaApp.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.MVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MovieController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await _mediator.Send(new GetAllMoviesShowsQuery());
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());

            ViewBag.AgeRatings = ageRatings;

            return View(movies);
        }

        [HttpGet]
        [Route("CinemaApp/{encodedTitle}/Details")]
        public async Task<IActionResult> Details(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            var ageRatingDto = await _mediator.Send(new GetAgeRatingByIdQuery(movieDto.AgeRatingId));

            ViewBag.AgeRatingName = ageRatingDto.MinimumAge;

            return View(movieDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());

            var ageRatingSelectList = new SelectList(ageRatings, "Id", "MinimumAge");

            ViewBag.AgeRatingSelectList = ageRatingSelectList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateMovieCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(command);

            this.SetNotification("success", $"Created movie: {command.Title}");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShow()
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery());
            var halls = await _mediator.Send(new GetAllHallsQuery());
            var movieSelectList = new SelectList(movies, "MovieId", "Title");
            var hallsSelectList = new SelectList(halls, "Id", "Number");

            ViewBag.MovieSelectList = movieSelectList;
            ViewBag.HallsSelectList = hallsSelectList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShow(CreateMovieShowCommand command)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification("error", $"Incorrect data has been entered for the show");
                return RedirectToAction(nameof(CreateShow));
            }

            await _mediator.Send(command);
            this.SetNotification("success", $"Created new movie show");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("CinemaApp/{encodedTitle}/Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));

            EditMovieCommand model = _mapper.Map<EditMovieCommand>(movieDto);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/{encodedTitle}/Edit")]
        public async Task<IActionResult> Edit(EditMovieCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
