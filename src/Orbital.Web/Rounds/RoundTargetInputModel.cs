using Orbital.Models.Domain;
using Orbital.Web.Lengths;

namespace Orbital.Web.Rounds
{
    public class RoundTargetInputModel
    {
        public int Id { get; set; }

        public ScoringType ScoringType { get; set; }

        public LengthInputModel Distance { get; set; }
        public LengthInputModel FaceSize { get; set; }

        public int ArrowCount { get; set; }
    }
}