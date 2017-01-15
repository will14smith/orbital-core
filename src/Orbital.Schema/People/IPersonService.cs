using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Schema.People
{
    public interface IPersonService
    {
        IReadOnlyCollection<Person> GetRoot();
        IReadOnlyCollection<Person> GetByClub(Club club);

        Person Add(Person input);
        Person Update(int id, Person input);
    }
}