using System.Collections.Generic;

namespace Orbital.Models.Domain
{
    public class Round
    {
        public Round(int id, int variantOfId, string category, string name, bool indoor, IReadOnlyList<RoundTarget> targets)
        {
            Id = id;

            VariantOfId = variantOfId;

            Category = category;
            Name = name;
            Indoor = indoor;

            Targets = targets;
        }

        public int Id { get; private set; }

        public int VariantOfId { get; private set; }

        public string Category { get; private set; }
        public string Name { get; private set; }
        // TODO enum this?
        public bool Indoor { get; private set; }

        public IReadOnlyList<RoundTarget> Targets { get; private set; }
    }
}
