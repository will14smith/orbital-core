using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Schema.Clubs
{
  public interface IClubService
  {
    IReadOnlyCollection<Club> GetRoot();
    Club GetById(int id);
  }
}