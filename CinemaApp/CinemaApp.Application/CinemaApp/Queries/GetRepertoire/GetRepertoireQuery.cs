using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetRepertoire
{
    public class GetRepertoireQuery : IRequest<IEnumerable<MovieDto>>
    {
        public List<int>? HallNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public string? SearchString { get; set; }

        public GetRepertoireQuery(List<int>? hallNumber, DateTime? startTime, string? searchString)
        {
            if (hallNumber != null && hallNumber.Any()) HallNumber = hallNumber;
            if (startTime.HasValue) StartTime = startTime;
            if(searchString != null) SearchString = searchString;
        }
    }
}
