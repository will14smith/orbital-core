using Orbital.Models;

namespace Orbital.Web.Rounds
{
    public class RoundTargetInputModel
    {
        public RoundTargetInputModel()
        {
        }
        public RoundTargetInputModel(RoundTarget target)
        {
            ScoringType = target.ScoringType;

            DistanceValue = target.Distance.Value;
            DistanceUnit = target.Distance.Unit;
            FaceSizeValue = target.FaceSize.Value;
            FaceSizeUnit = target.FaceSize.Unit;

            ArrowCount = target.ArrowCount;
        }

        public ScoringType ScoringType { get; set; }

        public decimal DistanceValue { get; set; }
        public LengthUnit DistanceUnit { get; set; }
        public decimal FaceSizeValue { get; set; }
        public LengthUnit FaceSizeUnit { get; set; }

        public int ArrowCount { get; set; }
    }
}