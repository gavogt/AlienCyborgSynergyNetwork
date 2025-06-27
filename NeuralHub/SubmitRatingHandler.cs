
using Microsoft.EntityFrameworkCore;
using MediatR;
using AlienCyborgSynergyNetwork.Shared;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Hubs
{
    public class SubmitRatingHandler: IRequestHandler<SubmitRatingCommand, Guid>
    {
        private readonly RatingDBContext _db;
        public SubmitRatingHandler(RatingDBContext db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(SubmitRatingCommand cmd, CancellationToken cancellationToken)
        {
            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                SessionId = cmd.SessionId,
                Stability = cmd.Stability,
                Efficiency = cmd.Efficiency,
                Cohesion = cmd.Cohension,
                TimeStamp = DateTime.UtcNow
            };
            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
            return rating.Id;
        }
    }
}
