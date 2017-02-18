using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Orbital.Models.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local", Justification = "Private setters are needed for serialisation")]
    public class Score
    {
        // needed for deserialisation
        public Score() { }

        public Score(int id, Score score)
            : this(
                id: id,
                score: score,
                targets: score.Targets
            )
        {
        }

        public Score(int id, Score score, IReadOnlyCollection<ScoreTarget> targets)
            : this(
                id: id,

                personId: score.PersonId,
                clubId: score.ClubId,
                bowstyle: score.Bowstyle,

                competitionId: score.CompetitionId,
                roundId: score.RoundId,

                totalScore: score.TotalScore,
                totalGolds: score.TotalGolds,
                totalHits: score.TotalHits,

                shotAt: score.ShotAt,
                enteredAt: score.EnteredAt,

                targets: targets
            )
        {
        }

        public Score(int id,
            int personId, int clubId, Bowstyle bowstyle,
            int? competitionId, int roundId,
            decimal totalScore, decimal totalGolds, decimal totalHits,
            DateTime shotAt, DateTime enteredAt,
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
            EnteredAt = enteredAt;

            Targets = targets;
        }

        public int Id { get; private set; }

        public int PersonId { get; private set; }
        public int ClubId { get; private set; }
        public Bowstyle Bowstyle { get; private set; }

        public int? CompetitionId { get; private set; }
        public int RoundId { get; private set; }

        public decimal TotalScore { get; private set; }
        public decimal TotalGolds { get; private set; }
        public decimal TotalHits { get; private set; }

        public DateTime ShotAt { get; private set; }
        public DateTime EnteredAt { get; private set; }

        public IReadOnlyCollection<ScoreTarget> Targets { get; private set; }
    }
}
