using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
  public interface IClubRepository
  {
    IReadOnlyCollection<Club> GetAll();
    Club GetById(int id);

    Club Create(Club club);
    Club Update(Club club);
    bool Delete(Club club);
  }
}
