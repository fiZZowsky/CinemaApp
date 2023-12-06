using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditUserRole
{
    public class EditUserRoleCommandHandler : IRequestHandler<EditUserRoleCommand>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IUserContext _userContext;

        public EditUserRoleCommandHandler(IIdentityRepository identityRepository, IUserContext userContext)
        {
            _identityRepository = identityRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditUserRoleCommand request, CancellationToken cancellationToken)
        {
            //var currentUser = _userContext.GetCurrentUser();
            //if (currentUser == null || !currentUser.IsInRole("Admin"))
            //{
            //    return Unit.Value;
            //}

            await _identityRepository.ChangeUserRole(request.UserId, request.RoleName);

            return Unit.Value;
        }
    }
}
