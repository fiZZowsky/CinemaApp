using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditUserRole
{
    public class EditUserRoleCommand : IRequest
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public EditUserRoleCommand(string userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}
