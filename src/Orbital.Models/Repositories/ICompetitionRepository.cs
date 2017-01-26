using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface ICompetitionRepository
    {
        IReadOnlyCollection<Competition> GetAll();
        Competition GetById(int id);

        Competition Create(Competition competition);
        Competition Update(Competition competition);
        bool Delete(Competition competition);
    }
}
