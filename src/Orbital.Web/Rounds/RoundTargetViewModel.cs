using Orbital.Models.Domain;
using Orbital.Web.Lengths;

namespace Orbital.Web.Rounds
{
    public class RoundTargetViewModel
    {
        public int Id { get; set; }

        public ScoringType ScoringType { get; set; }

        public LengthViewModel Distance { get; set; }
        public LengthViewModel FaceSize { get; set; }

        public int ArrowCount { get; set; }
    }
}