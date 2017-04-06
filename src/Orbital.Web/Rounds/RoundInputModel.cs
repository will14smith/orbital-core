using System.Collections.Generic;

namespace Orbital.Web.Rounds
{
    public class RoundInputModel
    {
        public int Id { get; set; }

        public int? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public bool Indoor { get; set; }

        public List<RoundTargetInputModel> Targets { get; set; }
    }
}