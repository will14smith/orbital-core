using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Web.Badges
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;

        public BadgeService(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        public IReadOnlyCollection<BadgeViewModel> GetAll()
        {
            return _badgeRepository.GetAll().Select(ToViewModel).ToList();
        }

        public BadgeViewModel GetById(int id)
        {
            var badge = _badgeRepository.GetById(id);

            return badge == null ? null : ToViewModel(badge);
        }

        public BadgeViewModel Create(BadgeInputModel input)
        {
            var badge = FromInputModel(input);

            var result = _badgeRepository.Create(badge);

            return ToViewModel(result);
        }

        public BadgeViewModel Update(BadgeInputModel input)
        {
            var badge = FromInputModel(input);

            var result = _badgeRepository.Update(badge);

            return ToViewModel(result);
        }

        public bool Delete(int id)
        {
            var badge = _badgeRepository.GetById(id);

            return _badgeRepository.Delete(badge);
        }

        private static BadgeViewModel ToViewModel(Badge badge)
        {
            return new BadgeViewModel
            {
                Id = badge.Id,

                Name = badge.Name,
                Category = badge.Category,
                Description = badge.Description,

                Algorithm = badge.Algorithm,

                Multiple = badge.Multiple,

                ImageUrl = badge.ImageUrl
            };
        }

        private static Badge FromInputModel(BadgeInputModel badge)
        {
            return new Badge(badge.Id, badge.Name, badge.Description, badge.Algorithm, badge.Category, badge.Multiple, badge.ImageUrl);
        }
    }
}
