using AutoMapper;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle;
using CinemaApp.MVC.Extensions;
using CinemaApp.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        public async Task<IActionResult> Index()
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery());
            return View(movies);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateMovieCommand command)
        {
            if(!ModelState.IsValid)
            {
                return View(command);
            }

            //await _mediator.Send(command);

            this.SetNotification("success", $"Created movie: {command.Title}");

            return RedirectToAction(nameof(Index));
        }

        [Route("CinemaApp/{encodedTitle}/Details")]
        public async Task<IActionResult> Details (string encodedTitle)
        {
            var dto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));
            return View(dto);
        }
        
        [Route("CinemaApp/{encodedTitle}/Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit (string encodedTitle)
        {
            var dto = await _mediator.Send(new GetMovieByEncodedTitleQuery(encodedTitle));

            EditMovieCommand model = _mapper.Map<EditMovieCommand>(dto);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/{encodedTitle}/Edit")]
        public async Task<IActionResult> Edit (string title, string encodedTitle, EditMovieCommand command)
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
