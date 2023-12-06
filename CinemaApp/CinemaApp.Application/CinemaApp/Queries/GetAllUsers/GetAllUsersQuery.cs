using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
