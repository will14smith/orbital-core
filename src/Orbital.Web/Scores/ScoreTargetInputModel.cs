using System;
using Orbital.Models;

namespace Orbital.Web.Scores
{
    public class ScoreTargetInputModel
    {
        public ScoreTargetInputModel()
        {
        }

        public ScoreTargetInputModel(ScoreTarget target)
        {
            Id = target.Id;

            RoundTargetId = target.RoundTargetId;

            Score = target.Score;
            Golds = target.Golds;
            Hits = target.Hits;
        }

        public Guid Id { get; set; }

        public Guid RoundTargetId { get; set; }

        public decimal Score { get; set; }
        public decimal Golds { get; set; }
        public decimal Hits { get; set; }
    }
}