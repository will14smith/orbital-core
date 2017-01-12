using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Round
    {
        // needed for deserialisation
        public Round() { }

        public Round(int id, Round round)
            : this(
            id: id,
            round: round,
            targets: round.Targets
            )
        {
        }
        public Round(int id, Round round, IReadOnlyList<RoundTarget> targets)
            : this(
            id: id,
            variantOfId: round.VariantOfId,
            category: round.Category,
            name: round.Name,
            indoor: round.Indoor,
            targets: targets
            )
        {
            Id = id;
        }

        public Round(int id, int? variantOfId, string category, string name, bool indoor, IReadOnlyList<RoundTarget> targets)
        {
            Id = id;

            VariantOfId = variantOfId;

            Category = category;
            Name = name;
            Indoor = indoor;

            Targets = targets;
        }

        public int Id { get; private set; }

        public int? VariantOfId { get; private set; }

        public string Category { get; private set; }
        public string Name { get; private set; }
        // TODO enum this?
        public bool Indoor { get; private set; }

        public IReadOnlyList<RoundTarget> Targets { get; private set; }
    }
}
