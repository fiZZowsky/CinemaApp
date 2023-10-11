using AutoMapper;
using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateMovieShow;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Commands.EditMovieShow;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatingById;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatings;
using CinemaApp.Application.CinemaApp.Queries.GetAllHalls.GetAllHalls;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle;
using CinemaApp.Application.CinemaApp.Queries.GetMovieShowByEncodedTitle;
using CinemaApp.Application.CinemaApp.Queries.GetRepertoire;
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
            var movies = await _mediator.Send(new GetRepertoireQuery(null, DateTime.Today, null));
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            var halls = await _mediator.Send(new GetAllHallsQuery());

            ViewBag.AgeRatings = ageRatings;
            ViewBag.Halls = halls;

            var today = DateTime.Today;
            var calendar = new List<DateTime>();

            for (int i = 0; i < 14; i++)
            {
                calendar.Add(today.AddDays(i));
            }

            ViewBag.Calendar = calendar;

            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> SortShowsList(string? hallNumber, DateTime? repertoireDate, string? searchString)
        {
            IEnumerable<MovieDto> movies = new List<MovieDto>();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = await _mediator.Send(new GetRepertoireQuery(null, null, searchString));
            }

            List<int>? selectedHalls = new List<int>();
            if (!string.IsNullOrWhiteSpace(hallNumber))
            {
                selectedHalls = hallNumber.Split(',').Select(int.Parse).ToList();
            }

            movies = await _mediator.Send(new GetRepertoireQuery(selectedHalls, repertoireDate, searchString));

            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            ViewBag.AgeRatings = ageRatings;

            return PartialView("_ShowsListPartial", movies);
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
        [Route("CinemaApp/{encodedTitle}/DetailsShow")]
        public async Task<IActionResult> DetailsShow(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieShowByEncodedTitleQuery(encodedTitle));
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
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList();

                this.SetNotification("error", "Incorrect data has been entered for the movie: " + string.Join(", ", errors));

                return RedirectToAction(nameof(Create));
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
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList();
                this.SetNotification("error", "Incorrect data has been entered for the show: " + string.Join(", ", errors));
                return RedirectToAction(nameof(CreateShow));
            }

            await _mediator.Send(command);
            this.SetNotification("success", $"Created new movie show");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("CinemaApp/{encodedTitle}/EditMovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMovie(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            var ageRatingSelectList = new SelectList(ageRatings, "Id", "MinimumAge");

            ViewBag.AgeRatingSelectList = ageRatingSelectList;

            EditMovieCommand model = _mapper.Map<EditMovieCommand>(movieDto);
            return View(model);
        }

        [HttpGet]
        [Route("CinemaApp/{encodedTitle}/EditShow")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditShow(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieShowByEncodedTitleQuery(encodedTitle));
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            var halls = await _mediator.Send(new GetAllHallsQuery());

            var ageRatingSelectList = new SelectList(ageRatings, "Id", "MinimumAge");
            var hallsSelectList = new SelectList(halls, "Id", "Number");

            ViewBag.AgeRatingSelectList = ageRatingSelectList;
            ViewBag.HallsSelectList = hallsSelectList;

            EditMovieShowCommand model = _mapper.Map<EditMovieShowCommand>(movieDto);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/{encodedTitle}/EditMovie")]
        public async Task<IActionResult> EditMovie(EditMovieCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                this.SetNotification("error", "Incorrect data has been entered for the movie: " + string.Join(", ", errors));
                return View(command);
            }

            await _mediator.Send(command);
            this.SetNotification("success", $"Successfully edited {command.Title}.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/{encodedTitle}/EditShow")]
        public async Task<IActionResult> EditShow(EditMovieShowCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                this.SetNotification("error", "Incorrect data has been entered for the show: " + string.Join(", ", errors));
                return View(command);
            }

            await _mediator.Send(command);
            this.SetNotification("success", $"Successfully edited show for {command.Title}.");
            return RedirectToAction(nameof(Index));
        }
    }
}
