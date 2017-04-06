using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Web.Lengths;

namespace Orbital.Web.Rounds
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepository;

        public RoundService(IRoundRepository roundRepository)
        {
            _roundRepository = roundRepository;
        }

        public IReadOnlyCollection<RoundViewModel> GetAll()
        {
            return _roundRepository.GetAll().Select(ToViewModel).ToList();
        }

        public IReadOnlyCollection<RoundViewModel> GetAllByVariant(int parentId)
        {
            return _roundRepository.GetAllVariantsById(parentId).Select(ToViewModel).ToList();
        }

        public RoundViewModel GetById(int id)
        {
            var round = _roundRepository.GetById(id);

            return round == null ? null : ToViewModel(round);
        }

        public RoundViewModel Create(RoundInputModel input)
        {
            var round = FromInputModel(input);

            var result = _roundRepository.Create(round);

            return ToViewModel(result);
        }

        public RoundViewModel Update(RoundInputModel input)
        {
            var round = FromInputModel(input);

            var result = _roundRepository.Update(round);

            // TODO will probably need to trigger a bunch of stuff - recalculate handicaps, records?

            return ToViewModel(result);
        }

        public bool Delete(int id)
        {
            var round = _roundRepository.GetById(id);

            return _roundRepository.Delete(round);
        }

        private static RoundViewModel ToViewModel(Round round)
        {
            return new RoundViewModel
            {
                Id = round.Id,

                VariantOfId = round.VariantOfId,

                Category = round.Category,
                Name = round.Name,
                Indoor = round.Indoor,

                Targets = round.Targets.Select(ToViewModel).ToList()
            };
        }
        private static RoundTargetViewModel ToViewModel(RoundTarget target)
        {
            return new RoundTargetViewModel
            {
                Id = target.Id,

                ScoringType = target.ScoringType,

                Distance = target.Distance.ToView(),
                FaceSize = target.FaceSize.ToView(),

                ArrowCount = target.ArrowCount,
            };
        }

        private static Round FromInputModel(RoundInputModel round)
        {
            return new Round(
                round.Id,

                round.VariantOfId,

                round.Category,
                round.Name,
                round.Indoor,

                round.Targets.Select(FromInputModel).ToList()
            );
        }
        private static RoundTarget FromInputModel(RoundTargetInputModel target)
        {
            return new RoundTarget(
                target.Id,

                target.ScoringType,

                target.Distance.FromInput(),
                target.FaceSize.FromInput(),

                target.ArrowCount
            );
        }
    }
}