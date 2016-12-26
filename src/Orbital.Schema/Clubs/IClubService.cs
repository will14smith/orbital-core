using System.Collections.Generic;
using GraphQL.Types;
using Orbital.Models.Domain;

namespace Orbital.Schema.Clubs
{
  public interface IClubService
  {
    IReadOnlyCollection<Club> GetRoot();
  }
}