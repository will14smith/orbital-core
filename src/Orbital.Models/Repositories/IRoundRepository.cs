using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Models.Repositories
{
    public interface IRoundRepository : IRepository<Round>
    {
        IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId);
    }
}
