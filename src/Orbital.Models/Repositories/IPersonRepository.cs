using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        IReadOnlyCollection<Person> GetAllByClubId(int clubId);
    }
}
