using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class ScoreTarget
    {
        // needed for deserialisation
        public ScoreTarget(){}

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


        public int Id { get; private set; }

        public decimal Score { get; private set; }
        public decimal Golds { get; private set; }
        public decimal Hits { get; private set; }
    }
}