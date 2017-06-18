using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbital.Models.Domain
{
    public class Score
    {
        public Score(int id, Score score)
            : this(
                id: id,

                personId: score.PersonId,
                clubId: score.ClubId,
                roundId: score.RoundId,
                competitionId: score.CompetitionId,

                bowstyle: score.Bowstyle,

                totalScore: score.TotalScore,
                totalGolds: score.TotalGolds,
                totalHits: score.TotalHits,

                shotAt: score.ShotAt, 
                enteredAt: score.EnteredAt,
                
                targets: score.Targets)
        {
        }

        public Score(int id, int personId, int clubId, int roundId, int? competitionId, Bowstyle bowstyle, decimal totalScore, decimal totalGolds, decimal totalHits, DateTime shotAt, DateTime enteredAt, IReadOnlyCollection<ScoreTarget> targets)
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
            EnteredAt = enteredAt;

            Targets = targets;
        }

        public int Id { get; }

        public int PersonId { get; }
        public int ClubId { get; }
        public int RoundId { get; }
        public int? CompetitionId { get; }

        public Bowstyle Bowstyle { get; }

        public decimal TotalScore { get; }
        public decimal TotalGolds { get; }
        public decimal TotalHits { get; }

        public DateTime ShotAt { get; }
        public DateTime EnteredAt { get; }

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
                    && x.EnteredAt.Equals(y.EnteredAt) 
                    && x.Targets.SequenceEqual(y.Targets, new ScoreTarget.EqualWithoutId());
            }

            public int GetHashCode(Score obj)
            {
                unchecked
                {
                    var hashCode = obj.PersonId;
                    hashCode = (hashCode * 397) ^ obj.ClubId;
                    hashCode = (hashCode * 397) ^ obj.RoundId;
                    hashCode = (hashCode * 397) ^ obj.CompetitionId.GetHashCode();
                    hashCode = (hashCode * 397) ^ (int)obj.Bowstyle;
                    hashCode = (hashCode * 397) ^ obj.TotalScore.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.TotalGolds.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.TotalHits.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.ShotAt.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.EnteredAt.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.Targets?.GetHashCode() ?? 0);
                    return hashCode;
                }
            }
        }
    }
}
