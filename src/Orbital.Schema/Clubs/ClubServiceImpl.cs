using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.Clubs
{
    public class ClubServiceImpl : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubServiceImpl(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public IReadOnlyCollection<Club> GetRoot()
        {
            return _clubRepository.GetAll();
        }

        public Club GetById(int id)
        {
            return _clubRepository.GetById(id);
        }

        public Club Add(Club input)
        {
            return _clubRepository.Create(input);
        }

        public Club Update(int id, Club input)
        {
            var club = new Club(id, input.Name);

            return _clubRepository.Update(club);
        }
    }
}