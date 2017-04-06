using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Web.Models;

namespace Orbital.Web.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public IReadOnlyCollection<ClubViewModel> GetAll()
        {
            return _clubRepository.GetAll().Select(ToViewModel).ToList();
        }

        public ClubViewModel GetById(int id)
        {
            var club = _clubRepository.GetById(id);

            return club == null ? null : ToViewModel(club);
        }

        public ClubViewModel Create(ClubInputModel input)
        {
            var club = FromInputModel(input);

            var result = _clubRepository.Create(club);

            return ToViewModel(result);
        }

        public ClubViewModel Update(ClubInputModel input)
        {
            var club = FromInputModel(input);

            var result = _clubRepository.Update(club);

            return ToViewModel(result);
        }

        public bool Delete(int id)
        {
            var club = _clubRepository.GetById(id);

            return _clubRepository.Delete(club);
        }

        private static ClubViewModel ToViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name
            };
        }

        private static Club FromInputModel(ClubInputModel club)
        {
            return new Club(club.Id, club.Name);
        }
    }
}
