using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Schema.People
{
  public interface IPersonService
  {
    IReadOnlyCollection<Person> GetRoot();
    IReadOnlyCollection<Person> GetByClub(Club club);
  }
}