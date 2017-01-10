using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Schema.Rounds
{
    public interface IRoundService
    {
        IReadOnlyCollection<Round> GetRoot();
        
        Round Add(Round input);
        Round Update(int id, Round input);
    }
}
