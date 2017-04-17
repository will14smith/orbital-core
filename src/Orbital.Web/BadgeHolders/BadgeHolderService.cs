using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Web.BadgeHolders
{
    public class BadgeHolderService : IBadgeHolderService
    {
        private readonly IBadgeHolderRepository _badgeRepository;

        public BadgeHolderService(IBadgeHolderRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        public IReadOnlyCollection<BadgeHolderViewModel> GetAllByBadgeId(int badgeId)
        {
            return _badgeRepository.GetAllByBadgeId(badgeId).Select(ToViewModel).ToList();
        }

        public IReadOnlyCollection<BadgeHolderViewModel> GetAllByPersonId(int personId)
        {
            return _badgeRepository.GetAllByPersonId(personId).Select(ToViewModel).ToList();
        }

        public BadgeHolderViewModel GetById(int id)
        {
            var badge = _badgeRepository.GetById(id);

            return badge == null ? null : ToViewModel(badge);
        }

        public BadgeHolderViewModel Create(BadgeHolderInputModel input)
        {
            var badge = FromInputModel(input);

            var result = _badgeRepository.Create(badge);

            return ToViewModel(result);
        }

        public BadgeHolderViewModel Update(BadgeHolderInputModel input)
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

        private static BadgeHolderViewModel ToViewModel(BadgeHolder holder)
        {
            return new BadgeHolderViewModel
            {
                Id = holder.Id,

                BadgeId = holder.BadgeId,
                PersonId = holder.PersonId,

                AwardedOn = holder.AwardedOn,
                ConfirmedOn = holder.ConfirmedOn,
                MadeOn = holder.MadeOn,
                DeliveredOn = holder.DeliveredOn,
            };
        }

        private static BadgeHolder FromInputModel(BadgeHolderInputModel holder)
        {
            return new BadgeHolder(holder.Id, holder.BadgeId, holder.PersonId, holder.AwardedOn, holder.ConfirmedOn, holder.MadeOn, holder.DeliveredOn);
        }
    }
}
