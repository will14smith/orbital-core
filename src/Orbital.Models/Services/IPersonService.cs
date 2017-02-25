using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Services
{
    public interface IPersonService
    {
        IReadOnlyCollection<Person> GetRoot();
        IReadOnlyCollection<Person> GetByClub(Club club);

        Person Add(Person input);
        Person Update(int id, Person input);
    }
}