using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Schema.Competitions
{
    public interface ICompetitionService
    {
        IReadOnlyCollection<Competition> GetRoot();
        Competition GetById(int id);
    }
}