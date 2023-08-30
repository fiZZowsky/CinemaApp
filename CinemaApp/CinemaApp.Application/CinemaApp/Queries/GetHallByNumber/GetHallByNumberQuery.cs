using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetHallByNumber
{
    public class GetHallByNumberQuery : IRequest<HallDto>
    {
        public int Number { get; set; }

        public GetHallByNumberQuery(int number)
        {
            Number = number;
        }
    }
}
