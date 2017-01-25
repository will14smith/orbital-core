using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Schema.Rounds
{
    public interface IRoundService
    {
        IReadOnlyCollection<Round> GetRoot();
        IReadOnlyCollection<Round> GetVariants(int parentId);
        IReadOnlyCollection<Round> GetByCompetition(Competition competition);
        Round GetById(int id);

        Round Add(Round input);
        Round Update(int id, Round input);
    }
}
