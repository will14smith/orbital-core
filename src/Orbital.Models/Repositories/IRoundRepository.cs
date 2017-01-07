using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Models.Repositories
{
    public interface IRoundRepository
    {
        IReadOnlyCollection<Round> GetAll();
        IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId);
        Round GetById(int id);

        Round Create(Round round);
        Round Update(Round round);
        bool Delete(Round round);
    }
}
