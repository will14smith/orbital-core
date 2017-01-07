namespace Orbital.Models.Domain
{
    public class RoundTarget
    {
        public RoundTarget(int id, ScoringType scoringType, Length distance, Length faceSize, int arrowCount)
        {
            Id = id;

            ScoringType = scoringType;

            Distance = distance;
            FaceSize = faceSize;

            ArrowCount = arrowCount;
        }

        public int Id { get; private set; }

        public ScoringType ScoringType { get; private set; }

        public Length Distance { get; private set; }
        public Length FaceSize { get; private set; }

        public int ArrowCount { get; private set; }
    }
}
