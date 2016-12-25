using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
  public class InMemoryClubRepository : IClubRepository
  {
    private List<Club> _clubs;

    public InMemoryClubRepository()
    {
      _clubs = new List<Club>();
    }

    public IReadOnlyCollection<Club> GetAll() { return _clubs; }
    public Club GetById(int id) { return _clubs.First(x => x.Id == id); }

    public Club Create(Club club) { _clubs.Add(club); return club; }
    public Club Update(Club club) { Delete(club); return Create(club); }
    public bool Delete(Club club) { _clubs.Remove(club); return true; }
  }
}
