using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class RoundTarget
    {
        // needed for deserialisation
        public RoundTarget() { }

        public RoundTarget(int id, RoundTarget target)
            : this(
                id: id,
                scoringType: target.ScoringType,
                distance: target.Distance,
                faceSize: target.FaceSize,
                arrowCount: target.ArrowCount
            )
        {
        }

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

        public class EqualWithoutId : IEqualityComparer<RoundTarget>
        {
            public bool Equals(RoundTarget x, RoundTarget y)
            {
                return x.ScoringType == y.ScoringType
                    && Equals(x.Distance, y.Distance)
                    && Equals(x.FaceSize, y.FaceSize)
                    && x.ArrowCount == y.ArrowCount;
            }

            public int GetHashCode(RoundTarget obj)
            {
                unchecked
                {
                    var hashCode = (int)obj.ScoringType;
                    hashCode = (hashCode * 397) ^ obj.Distance.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.FaceSize.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.ArrowCount;
                    return hashCode;
                }
            }
        }
    }
}
