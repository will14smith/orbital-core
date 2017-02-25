using System.Collections.Generic;
using System.Linq;

namespace Orbital.Models.Domain
{
    public class Round
    {
        public Round(int id, Round round)
            : this(
            id: id,
            variantOfId: round.VariantOfId,
            category: round.Category,
            name: round.Name,
            indoor: round.Indoor,
            targets: round.Targets
            )
        {
        }

        public Round(int id, int? variantOfId, string category, string name, bool indoor, IReadOnlyCollection<RoundTarget> targets)
        {
            Id = id;

            VariantOfId = variantOfId;

            Category = category;
            Name = name;
            Indoor = indoor;

            Targets = targets;
        }

        public int Id { get; }

        public int? VariantOfId { get; }

        public string Category { get; }
        public string Name { get; }
        // TODO enum this?
        public bool Indoor { get; }

        public IReadOnlyCollection<RoundTarget> Targets { get; }

        public class EqualWithoutId : IEqualityComparer<Round>
        {
            public bool Equals(Round x, Round y)
            {
                return x.VariantOfId == y.VariantOfId
                    && string.Equals(x.Category, y.Category)
                    && string.Equals(x.Name, y.Name)
                    && x.Indoor == y.Indoor
                    && x.Targets.SequenceEqual(y.Targets, new RoundTarget.EqualWithoutId());
            }

            public int GetHashCode(Round obj)
            {
                unchecked
                {
                    var hashCode = obj.VariantOfId.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Category.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Name.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Indoor.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Targets.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
