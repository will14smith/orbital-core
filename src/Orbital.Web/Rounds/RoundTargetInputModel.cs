using Orbital.Models.Domain;

namespace Orbital.Web.Rounds
{
    public class RoundTargetInputModel
    {
        public RoundTargetInputModel()
        {
        }
        public RoundTargetInputModel(RoundTarget target)
        {
            throw new System.NotImplementedException();
        }

        public ScoringType ScoringType { get; set; }

        public decimal DistanceValue { get; set; }
        public LengthUnit DistanceUnit { get; set; }
        public decimal FaceSizeValue { get; set; }
        public LengthUnit FaceSizeUnit { get; set; }

        public int ArrowCount { get; set; }
    }
}