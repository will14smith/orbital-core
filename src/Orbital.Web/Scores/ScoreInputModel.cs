using System;
using System.Collections.Generic;
using Orbital.Models;

namespace Orbital.Web.Scores
{
    public class ScoreInputModel
    {
        public ScoreInputModel()
        {
        }

        public ScoreInputModel(Score score)
        {
            throw new NotImplementedException();
        }

        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Guid ClubId { get; set; }
        public Guid RoundId { get; set; }
        public Guid? CompetitionId { get; set; }

        public Bowstyle Bowstyle { get; set; }

        public decimal TotalScore { get; set; }
        public decimal TotalGolds { get; set; }
        public decimal TotalHits { get; set; }

        public DateTime ShotAt { get; set; }

        public List<ScoreTargetInputModel> Targets { get; set; }
    }
}