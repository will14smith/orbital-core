using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface IHandicapRepository : IRepository<Handicap>
    {
        ILookup<HandicapIdentifier, Handicap> GetAllByPersonId(int personId);

        IReadOnlyDictionary<HandicapIdentifier, Handicap> GetLatestByPersonId(int personId);
        Handicap GetLatestByPersonId(HandicapIdentifier id, int personId);

        Handicap GetByScoreId(int scoreId);
    }
}