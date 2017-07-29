using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace Orbital.Models
{
    public class Score
    {
        public Score(
            Guid id,
            Guid personId, Guid clubId, Guid roundId, Guid? competitionId,
            Bowstyle bowstyle, 
            decimal totalScore, decimal totalGolds, decimal totalHits, 
            Instant shotAt, 
            IReadOnlyCollection<ScoreTarget> targets)
        {
            Id = id;

            PersonId = personId;
            ClubId = clubId;
            Bowstyle = bowstyle;

            CompetitionId = competitionId;
            RoundId = roundId;

            TotalScore = totalScore;
            TotalGolds = totalGolds;
            TotalHits = totalHits;

            ShotAt = shotAt;

            Targets = targets;
        }

        public Guid Id { get; }

        public Guid PersonId { get; }
        public Guid ClubId { get; }
        public Guid RoundId { get; }
        public Guid? CompetitionId { get; }

        public Bowstyle Bowstyle { get; }

        public decimal TotalScore { get; }
        public decimal TotalGolds { get; }
        public decimal TotalHits { get; }

        public Instant ShotAt { get; }

        public IReadOnlyCollection<ScoreTarget> Targets { get; }

        public class EqualWithoutId : IEqualityComparer<Score>
        {
            public bool Equals(Score x, Score y)
            {
                return x.PersonId == y.PersonId 
                    && x.ClubId == y.ClubId 
                    && x.RoundId == y.RoundId 
                    && x.CompetitionId == y.CompetitionId 
                    && x.Bowstyle == y.Bowstyle
                    && x.TotalScore == y.TotalScore
                    && x.TotalGolds == y.TotalGolds
                    && x.TotalHits == y.TotalHits 
                    && x.ShotAt.Equals(y.ShotAt) 
                    && x.Targets.SequenceEqual(y.Targets, new ScoreTarget.EqualWithoutId());
            }

            public int GetHashCode(Score obj)
            {
                unchecked
                {
                    var hashCode = obj.PersonId.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.ClubId.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.RoundId.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.CompetitionId.GetHashCode();
                    hashCode = (hashCode * 397) ^ (int)obj.Bowstyle;
                    hashCode = (hashCode * 397) ^ obj.TotalScore.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.TotalGolds.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.TotalHits.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.ShotAt.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.Targets?.GetHashCode() ?? 0);
                    return hashCode;
                }
            }
        }
    }
}
