using System.Collections.Generic;

namespace Orbital.Models.Domain
{
    public class ScoreTarget
    {
        public ScoreTarget(int id, ScoreTarget scoreTarget)
            : this(
            id: id,
            score: scoreTarget.Score,
            golds: scoreTarget.Golds,
            hits: scoreTarget.Hits
            )
        {
        }
        public ScoreTarget(int id, decimal score, decimal golds, decimal hits)
        {
            Id = id;
            Score = score;
            Golds = golds;
            Hits = hits;
        }


        public int Id { get; }

        public decimal Score { get; }
        public decimal Golds { get; }
        public decimal Hits { get; }

        public class EqualWithoutId : IEqualityComparer<ScoreTarget>
        {
            public bool Equals(ScoreTarget x, ScoreTarget y)
            {
                return x.Score == y.Score
                    && x.Golds == y.Golds
                    && x.Hits == y.Hits;
            }

            public int GetHashCode(ScoreTarget obj)
            {
                var hashCode = obj.Score.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Golds.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Hits.GetHashCode();
                return hashCode;
            }
        }
    }
}