using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Schema.Competitions
{
    public interface ICompetitionService
    {
        IReadOnlyCollection<Competition> GetRoot();
        Competition GetById(int id);

        Competition Add(Competition input);
        Competition Update(int id, Competition input);
    }
}