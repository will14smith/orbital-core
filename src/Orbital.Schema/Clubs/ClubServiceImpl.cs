using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.Clubs
{
  public class ClubServiceImpl : IClubService
  {
    private IClubRepository _clubRepository;

    public ClubServiceImpl(IClubRepository clubRepository)
    {
      _clubRepository = clubRepository;
    }

    public IReadOnlyCollection<Club> GetRoot()
    {
      return _clubRepository.GetAll();
    }
  }
}