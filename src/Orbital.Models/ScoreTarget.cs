using System;
using System.Collections.Generic;

namespace Orbital.Models
{
    public class ScoreTarget
    {
        public ScoreTarget(Guid id, Guid roundTargetId, decimal score, decimal golds, decimal hits)
        {
            Id = id;

            RoundTargetId = roundTargetId;

            Score = score;
            Golds = golds;
            Hits = hits;
        }

        public Guid Id { get; }

        public Guid RoundTargetId { get; }

        public decimal Score { get; }
        public decimal Golds { get; }
        public decimal Hits { get; }

        public class EqualWithoutId : IEqualityComparer<ScoreTarget>
        {
            public bool Equals(ScoreTarget x, ScoreTarget y)
            {
                return x.RoundTargetId == y.RoundTargetId
                    && x.Score == y.Score
                    && x.Golds == y.Golds
                    && x.Hits == y.Hits;
            }

            public int GetHashCode(ScoreTarget obj)
            {
                var hashCode = obj.RoundTargetId.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Score.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Golds.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Hits.GetHashCode();
                return hashCode;
            }
        }
    }
}