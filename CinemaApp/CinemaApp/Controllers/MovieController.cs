using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateRating;
using CinemaApp.Application.CinemaApp.Commands.DeleteMovieRating;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatingById;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatings;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle;
using CinemaApp.Application.CinemaApp.Queries.GetMovieRatings;
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
        private readonly IUserContext _userContext;

        public MovieController(IMediator mediator, IMapper mapper, IUserContext userContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery());
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());

            ViewBag.AgeRatings = ageRatings;

            return View(movies);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SortMoviesList(string? searchString)
        {
            IEnumerable<MovieDto> movies = new List<MovieDto>();

            movies = await _mediator.Send(new GetRepertoireQuery(null, null, searchString));

            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            ViewBag.AgeRatings = ageRatings;

            return PartialView("_MoviesListPartial", movies);
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
        [Route("CinemaApp/{encodedTitle}/Details")]
        public async Task<IActionResult> Details(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            var ageRatingDto = await _mediator.Send(new GetAgeRatingByIdQuery(movieDto.AgeRatingId));

            ViewBag.AgeRating = ageRatingDto.MinimumAge;

            return View(movieDto);
        }

        [HttpGet]
        [Route("CinemaApp/{encodedTitle}/EditMovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string encodedTitle)
        {
            var movieDto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            var ageRatings = await _mediator.Send(new GetAgeRatingsQuery());
            var ageRatingSelectList = new SelectList(ageRatings, "Id", "MinimumAge");

            ViewBag.AgeRatingSelectList = ageRatingSelectList;

            EditMovieCommand model = _mapper.Map<EditMovieCommand>(movieDto);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/{encodedTitle}/EditMovie")]
        public async Task<IActionResult> Edit(EditMovieCommand command)
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
        [Authorize]
        [Route("CinemaApp/MovieRating")]
        public async Task<IActionResult> CreateRating(CreateRatingCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                this.SetNotification("error", string.Join(", ", errors));
                return BadRequest(ModelState);
            }

            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("CinemaApp/{movieId}/MovieRatings")]
        public async Task<IActionResult> GetRatings(int movieId)
        {
            var data = await _mediator.Send(new GetMovieRatingsQuery() { MovieId = movieId });
            return Ok(data);
        }

        [HttpDelete]
        [Authorize]
        [Route("CinemaApp/{movieId}/DeleteRating/{id}/{createdByUserId}")]
        public async Task<IActionResult> DeleteRating(int id, int movieId, string createdByUserId)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || (!currentUser.IsInRole("Admin") && currentUser.Id != createdByUserId))
            {
                return BadRequest(ModelState);
            }

            DeleteMovieRatingCommand command = new();
            command.Id = id;
            command.MovieId = movieId;
            command.CreatedByUserId = createdByUserId;

            await _mediator.Send(command);
            return Ok();
        }
    }
}
