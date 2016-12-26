using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
  public interface IPersonRepository
  {
    IReadOnlyCollection<Person> GetAll();
    IReadOnlyCollection<Person> GetAllByClubId(int clubId);
    Person GetById(int id);

    Person Create(Person person);
    Person Update(Person person);
    bool Delete(Person person);
  }
}
