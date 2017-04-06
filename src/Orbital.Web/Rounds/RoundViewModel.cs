using System.Collections.Generic;
using Halcyon.HAL.Attributes;
using Newtonsoft.Json;

namespace Orbital.Web.Rounds
{
    public class RoundViewModel
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public bool Indoor { get; set; }

        [HalEmbedded("targets")]
        public List<RoundTargetViewModel> Targets { get; set; }
    }
}