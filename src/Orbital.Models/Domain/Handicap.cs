using System;

namespace Orbital.Models.Domain
{
    public class Handicap
    {
        public Handicap(int id, Handicap handicap) 
            : this(
                id: id,

                personId: handicap.PersonId,
                scoreId: handicap.ScoreId,
                type: handicap.Type,
                date: handicap.Date,
                value: handicap.Value,
                identifier: handicap.Identifier
            )
        {
        }

        public Handicap(int id, int personId, int? scoreId, HandicapType type, DateTime date, int value, HandicapIdentifier identifier)
        {
            Id = id;
            PersonId = personId;
            ScoreId = scoreId;
            Type = type;
            Date = date;
            Value = value;
            Identifier = identifier;
        }

        public int Id { get; }

        public int PersonId { get; }
        public int? ScoreId { get; }

        public HandicapType Type { get; }
        public DateTime Date { get; }
        public int Value { get; }

        public HandicapIdentifier Identifier { get; }
    }
}
