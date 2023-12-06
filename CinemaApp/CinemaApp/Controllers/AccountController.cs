using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.EditUserRole;
using CinemaApp.Application.CinemaApp.Commands.SendRecoveryPasswordEmail;
using CinemaApp.Application.CinemaApp.Queries.GetAllUsers;
using CinemaApp.Domain.Entities;
using CinemaApp.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IMediator mediator, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "EmailWithRecoveryPassword.html");
            var htmlContent = System.IO.File.ReadAllText("Templates\\EmailWithRecoveryPassword.html");

            SendRecoveryPasswordEmailCommand command = new SendRecoveryPasswordEmailCommand(user.Email, htmlContent, callback);
            await _mediator.Send(command);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModelDto { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelDto resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.Roles = roles;

            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CinemaApp/Account/ChangeRole/{userId}/{roleName}")]
        public async Task<IActionResult> ChangeUserRole(string userId, string roleName)
        {
            EditUserRoleCommand command = new EditUserRoleCommand(userId, roleName);
            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}
