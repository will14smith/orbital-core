using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Models;

namespace Orbital.Web.Rounds
{
    public class RoundInputModel
    {
        public RoundInputModel()
        {
        }
        public RoundInputModel(Round round)
        {
            VariantOfId = round.VariantOfId;

            Category = round.Category;
            Name = round.Name;

            Indoor = round.Indoor;

            Targets = round.Targets.Select(x => new RoundTargetInputModel(x)).ToList();
        }

        public Guid? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }

        public bool Indoor { get; set; }

        public List<RoundTargetInputModel> Targets { get; set; }
    }
}