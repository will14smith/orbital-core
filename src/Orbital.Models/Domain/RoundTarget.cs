namespace Orbital.Models.Domain
{
    public class RoundTarget
    {
        public RoundTarget(int id, ScoringType scoringType, Length distance, Length targetSize, int arrowCount)
        {
            Id = id;

            ScoringType = scoringType;

            Distance = distance;
            TargetSize = targetSize;

            ArrowCount = arrowCount;
        }

        public int Id { get; private set; }

        public ScoringType ScoringType { get; private set; }

        public Length Distance { get; private set; }
        public Length TargetSize { get; private set; }

        public int ArrowCount { get; private set; }
    }
}
